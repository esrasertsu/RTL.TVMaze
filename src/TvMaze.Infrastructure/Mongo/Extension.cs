using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvMaze.Infrastructure.Mongo
{
    public static class Extension
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoConfig = new MongoDBSettings();

            var configSection = configuration.GetSection("databaseSettings");

            configSection.Bind(mongoConfig);

            services.AddSingleton<IMongoClient>(client => {
                return new MongoClient(mongoConfig.ConnectionString);
            });
            services.AddSingleton<IMongoDatabase>(client => {
                var mongoClient = client.GetRequiredService<IMongoClient>();
                return mongoClient.GetDatabase(mongoConfig.DatabaseName);
            });

            services.AddSingleton<IDatabaseInitializer, MongoInitializer>();
        }
    }
}
