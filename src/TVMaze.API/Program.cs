using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using MassTransit;
using StackExchange.Redis;
using TvMaze.Infrastructure.EventBus;
using TvMaze.Infrastructure.Mongo;
using TVMaze.API.Handlers;
using TVMaze.API.Middlewares;
using TVMaze.Application;
using TVMaze.Application.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddMongoDb(builder.Configuration);
//builder.Services.AddRabbitMq(builder.Configuration);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

builder.Services.AddScoped<ITVShowReposiory, TVShowRepository>();
builder.Services.AddScoped<ITVShowService, TVShowService>();
//builder.Services.AddScoped<CreateTvShowHandler>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "TvMaze Show API Endpoints",
        Version = "v1",
        Description = "These API Endpoints are available to TvMaze related data"
    });
});


//var rabbitMqOption = new RabbitMqOption();

//builder.Configuration.GetSection("rabbitmq").Bind(rabbitMqOption);
//builder.Services.AddMassTransit(x =>
//{
//    x.AddConsumer<CreateTvShowHandler>();
//    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
//    {
//        cfg.Host(new Uri(rabbitMqOption.ConnectionString), hostconfig =>
//        {
//            hostconfig.Username(rabbitMqOption.Username);
//            hostconfig.Password(rabbitMqOption.Password);
//        });

//        cfg.ReceiveEndpoint("create_tvshow", ep =>
//        {
//            ep.PrefetchCount = 16;
//            ep.UseMessageRetry(retryConf => { retryConf.Interval(2, 100); });
//            ep.ConfigureConsumer<CreateTvShowHandler>(provider);
//        });
//    }));
//});

builder.Services.AddControllers();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TvShow API");
    });
}

//app.UseScrapperMiddleware();


app.UseAuthorization();

app.MapControllers();

//var busControl = ((IApplicationBuilder)app).ApplicationServices.GetService<IBusControl>();
//busControl.Start();

var dbInitializer = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IDatabaseInitializer>();
dbInitializer.InitializeAsync();


app.Run();
