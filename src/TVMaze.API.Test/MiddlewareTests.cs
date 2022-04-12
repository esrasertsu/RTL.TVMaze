using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TVMaze.API.Test
{
    public class MiddlewareTests
    {
        [Fact]
        public async Task MiddlewareTest_ReturnsNotFoundForRequest()
        {
            //using var host = await new HostBuilder()
            //    .ConfigureWebHost(webBuilder =>
            //    {
            //        webBuilder
            //            .UseTestServer()
            //            .ConfigureServices(services =>
            //            {
            //                services.AddMyServices();
            //            })
            //            .Configure(app =>
            //            {
            //                app.UseMiddleware<MyMiddleware>();
            //            });
            //    })
            //    .StartAsync();

            //var response = await host.GetTestClient().GetAsync("/");

            //Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
