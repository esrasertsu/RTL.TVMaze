using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using TVMaze.Core;

namespace TVMaze.Application.Repository
{
    public class TVShowRepository :ITVShowReposiory
    {
        private IMongoDatabase _database;
        private IMongoCollection<TvShowWithCast> _collection;
        private readonly IDistributedCache _redisCache;
        private int pagesize = 5;
        private readonly IHttpClientFactory _httpClientFactory;


        public TVShowRepository(IMongoDatabase database, IDistributedCache redisCache, IHttpClientFactory httpClientFactory)
        {
            _database = database;
            _collection = database.GetCollection<TvShowWithCast>("TvShows", null);

            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
            _httpClientFactory = httpClientFactory;

        }

        public async Task<bool> AddTvShow(TvShowWithCast show)
        {
            await _collection.InsertOneAsync(show);
            return true;
        }

        public async Task<bool> DeleteTvShow(int showId)
        {
            FilterDefinition<TvShowWithCast> filter = Builders<TvShowWithCast>.Filter.Eq(q => q.Id, showId);

            DeleteResult deleteResult = await _collection.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged &&
                deleteResult.DeletedCount >0;
        }

        public async Task<IEnumerable<TvShowWithCast>> GetTvShows(int page)
        {

            var alltvshows = await _redisCache.GetStringAsync("allTvShows");
            var lastUpdated = await _redisCache.GetStringAsync("lastUpdate");

            if (String.IsNullOrEmpty(alltvshows) || (DateTime.Now - DateTime.Parse(lastUpdated)).TotalSeconds > 5)
            {
                await GetTvMazeShowsFromApiAsync(page);
                return InitializeTvShowsWithCast(page);
            }
            else
            {
                var shows = JsonConvert.DeserializeObject<IEnumerable<TvShowWithCast>>(alltvshows);
                var paginatedShows = shows!.Skip((page -1) * pagesize).Take(pagesize);
                return paginatedShows;

            }


        }

        private async Task GetTvMazeShowsFromApiAsync(int page)
        {

            string requestURL = "http://api.tvmaze.com/shows";

            var (status, json) = await RequestJson("showList", new Uri(requestURL, UriKind.Absolute))
               .ConfigureAwait(false);

            if (status != HttpStatusCode.OK)
            {
                throw new Exception("problem occured");
            }
            else
            {
                var cacheData = JsonConvert.DeserializeObject<IEnumerable<TvShowWithCast>>(json);

                var paginatedShows = cacheData.Skip((page -1) * pagesize).Take(pagesize);

                foreach (dynamic show in paginatedShows)
                {

                    var url = "http://api.tvmaze.com/shows/"+ show.Id + "/cast";

                    var (stat, castJson) = await RequestJson($"cast{show.Name}", new Uri(url, UriKind.Absolute))
                       .ConfigureAwait(false);

                    List<CastMember> castMembers = new List<CastMember>();
                    
                    var casts = JArray.Parse(castJson);


                    foreach (dynamic cast in casts)
                    {

                        var person = cast.person;
                        var member = new CastMember
                        {
                            Id = person.id,
                            Name = person.name,
                            Birthday = person.birthday
                        };

                        castMembers.Add(member);
                        cacheData!.First(x => x.Id == show.Id).Cast.Add(member);
                    }

                    var newItem = new TvShowWithCast
                    {
                        Id = show.Id,
                        Name = show.Name,
                        Cast = castMembers
                    };

                    try
                    {
                        await _collection.InsertOneAsync(newItem);
                    }
                    catch (Exception e)
                    {
                        await _collection.ReplaceOneAsync(filter: g => g.Id == newItem.Id, replacement: newItem);

                    }

                }

                await _redisCache.SetStringAsync("allTvShows", JsonConvert.SerializeObject(cacheData));
                await _redisCache.SetStringAsync("lastUpdate", DateTime.Now.ToString());
            }

      
        }

        private IEnumerable<TvShowWithCast> InitializeTvShowsWithCast(int page)
        {

            var countFacet = AggregateFacet.Create("count",
              PipelineDefinition<TvShowWithCast, AggregateCountResult>.Create(new[]
              {
                    PipelineStageDefinitionBuilder.Count<TvShowWithCast>()
              }));

            var dataFacet = AggregateFacet.Create("data",
           PipelineDefinition<TvShowWithCast, TvShowWithCast>.Create(new[]
           {
                    PipelineStageDefinitionBuilder.Sort(Builders<TvShowWithCast>.Sort.Ascending(x => x.Name)),
                    PipelineStageDefinitionBuilder.Skip<TvShowWithCast>((page - 1) * pagesize),
                    PipelineStageDefinitionBuilder.Limit<TvShowWithCast>(pagesize)
           }));

            var filter = Builders<TvShowWithCast>.Filter.Empty;
            var aggregation =  _collection.Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToList();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var totalPages = (int)count / pagesize;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<TvShowWithCast>();

            return data;
        }

        public async Task<TvShowWithCast> GetTvShow(int showId)
        {
            var show = _collection.AsQueryable().FirstOrDefault(x => x.Id == showId);
            if (show == null)
                throw new Exception("show not found");
            await Task.CompletedTask;
            return show;

        }


        private async Task<ApiResponse> RequestJson(string key, Uri relativePath)
        {
            using (var httpClient = _httpClientFactory.CreateClient(key))
            {
                dynamic response = await httpClient.GetAsync(relativePath).ConfigureAwait(false);

               
                if (response.StatusCode.ToString() == "OK")
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    return new ApiResponse(HttpStatusCode.OK, responseBody);
                }
                else if (response.StatusCode.ToString() == "TooManyRequests")
                {
                    return new ApiResponse(response.StatusCode, string.Empty);
                }
                else if (response.StatusCode.ToString() == "NotFound")
                {
                    return new ApiResponse(response.StatusCode, string.Empty);
                }
                else
                {
                    return new ApiResponse(response.StatusCode, string.Empty);
                }
                
            }
        }


    }
}
