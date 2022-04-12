using TVMaze.Application.Repository;
using TVMaze.Core;

namespace TVMaze.Application
{
    public class TVShowService : ITVShowService
    {
        public TVShowService(ITVShowReposiory repository)
        {
            _repository = repository;
        }

        private ITVShowReposiory _repository;

        public async Task<bool> AddTvShow(TvShowWithCast show)
        {
            return await _repository.AddTvShow(show);
        }

        public async Task<bool> DeleteTvShow(int id)
        {
            return await _repository.DeleteTvShow(id);
        }

        public async Task<TvShowWithCast> GetTvShow(int showId)
        {
            var product = await _repository.GetTvShow(showId);
            return product;
        }

        public async Task<IEnumerable<TvShowWithCast>> GetTvShows(int page)
        {
            return await _repository.GetTvShows(page);
        }
       
    }
}
