using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvMaze.Infrastructure.EventBus
{
    public static class Extension
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMq = new RabbitMqOption();
            configuration.GetSection("rabbitmq").Bind(rabbitMq);

            // establish connection with rabbitMQ..
            services.AddMassTransit(x => {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitMq.ConnectionString), hostcfg => {
                        hostcfg.Username(rabbitMq.Username);
                        hostcfg.Password(rabbitMq.Password);
                    });
                    cfg.ConfigureEndpoints(provider);
                }));
               // x.AddRequestClient<GetProductById>();
            });

           // services.AddMassTransitHostedService();

            return services;
        }
    }
}
