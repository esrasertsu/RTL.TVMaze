using System.Net;
using TVMaze.API.Attributes;

namespace TVMaze.API.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //var endpoint = context.GetEndpoint();
            //var decorator = endpoint?.Metadata.GetMetadata<LimitRequests>();
            //if (decorator is null)
            //{
            //    await _next(context);
            //    return;
            //}
            //var key = GenerateClientKey(context);
            //var clientStatistics = await GetClientStatisticsByKey(key);
            //if (clientStatistics != null &&
            //       DateTime.UtcNow < clientStatistics.LastSuccessfulResponseTime.AddSeconds(decorator.TimeWindow) &&
            //       clientStatistics.NumberOfRequestsCompletedSuccessfully == rateLimitingDecorator.MaxRequests)
            //{
            //    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            //    return;
            //}
            //await UpdateClientStatisticsStorage(key, rateLimitingDecorator.MaxRequests);
            await _next(context);
        }

        private static string GenerateClientKey(HttpContext context)
             => $"{context.Request.Path}_{context.Connection.RemoteIpAddress}";


        //private async Task<ClientStatistics> GetClientStatisticsByKey(string key)
        //{
        //    return await _cache.GetCacheValueAsync<ClientStatistics>(key);
        //}
    }


    
    public class ClientStatistics
    {
        public DateTime LastSuccessfulResponseTime { get; set; }
        public int NumberOfRequestsCompletedSuccessfully { get; set; }
    }

}
