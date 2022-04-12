using TVMaze.Application;

namespace TVMaze.API.Middlewares
{
    public class ScrapperMiddleware
    {
        private readonly RequestDelegate _next;

        public ScrapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // IMessageWriter is injected into InvokeAsync
        public async Task InvokeAsync(HttpContext httpContext, ITVShowService svc)
        {
            //await svc.GetUpdatedTvShows();
            await _next(httpContext);
        }
    }
    public static class ScrapperMiddlewareExtensions
    {
        public static IApplicationBuilder UseScrapperMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ScrapperMiddleware>();
        }
    }
}
