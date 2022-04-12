using MassTransit;
using TVMaze.Application;
using TVMaze.Core;

namespace TVMaze.API.Handlers
{
    public class CreateTvShowHandler : IConsumer<TvShowWithCast>
    {
        ITVShowService _service;
        public CreateTvShowHandler(ITVShowService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<TvShowWithCast> context)
        {
            var show = new TvShowWithCast
            {
                Id = context.Message.Id,
                Name = context.Message.Name,
                Cast = context.Message.Cast
            };

            await _service.AddTvShow(show);
        }
    }
}
