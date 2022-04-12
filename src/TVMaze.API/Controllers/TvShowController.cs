using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Wrap;
using TVMaze.API.Attributes;
using TVMaze.Application;
using TVMaze.Core;

namespace TVMaze.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TvShowController : ControllerBase
    {
        private ITVShowService _service { get; }
       // private IBusControl _bus;
       private readonly AsyncFallbackPolicy<IActionResult> _fallbackPolicy;
       private static readonly int MAX_RETRY_COUNT = 1;

        #region Policies

        private static AsyncCircuitBreakerPolicy<IActionResult> _circuitBreaker = Policy<IActionResult>.Handle<Exception>().
                                                                                   AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromSeconds(30),
                                                                                                                   2, TimeSpan.FromMinutes(1));

        private static AsyncRetryPolicy<IActionResult> _retryPolicy = Policy<IActionResult>.Handle<Exception>().WaitAndRetryAsync(MAX_RETRY_COUNT,
                                retryCount => TimeSpan.FromSeconds(Math.Pow(3, retryCount)/3)); // Jittering strategy...

        private static AsyncPolicyWrap<IActionResult> _wrapPolicy = Policy.WrapAsync(_circuitBreaker, _retryPolicy);

        private static AsyncBulkheadPolicy _bulkhead = Policy.BulkheadAsync(1, 2, (ctx) =>
        {
            throw new Exception("All slots are filled.");
        });

        #endregion
        public TvShowController(ITVShowService service
            //, IBusControl bus
            )
        {
            _service = service;
            //  _bus = bus;
            _fallbackPolicy = Policy<IActionResult>.Handle<Exception>().FallbackAsync(Content("We're experiencing some issue,Please try again later "));
        }

        [HttpGet]
        public async Task<TvShowWithCast> Get(int id)
        {
            return await _service.GetTvShow(id);
          
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteShow(int id)
        {
            return await _fallbackPolicy.ExecuteAsync(async () =>
            {

                bool isDeleted = await _service.DeleteTvShow(id);
                if (isDeleted)
                    return Accepted();
                else throw new Exception("Couldn't deleted item");

            }
            );
           

        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TvShowWithCast>), StatusCodes.Status200OK)]
        [LimitRequests(MaxRequests = 19, TimeWindow = 9)]
        public async Task<IEnumerable<TvShowWithCast>> GetAllTvShows(int page)
        {
            var emptyExecutionSlots = _bulkhead.BulkheadAvailableCount;
            var emptyQueueSlots = _bulkhead.QueueAvailableCount;

            return await _bulkhead.ExecuteAsync(async () =>
            {
                return await _service.GetTvShows(page);
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TvShowWithCast show)
        {
            //var uri = new Uri("rabbitmq://localhost/create_tvshow");
            //var endpoint = await _bus.GetSendEndpoint(uri);

            //await endpoint.Send(show);
            var addedProduct = await _service.AddTvShow(show);
            return Accepted("Added Show");
        }
    }
}
