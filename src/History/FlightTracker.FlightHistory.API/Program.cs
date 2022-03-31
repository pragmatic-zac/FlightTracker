//using FlightTracker.FlightHistory.API.Repository;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IFlightRepository, FlightRepository>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseAuthorization();

//app.MapControllers();

//app.Run();


using System.Reflection;
using FlightTracker.FlightHistory.API.Repository;
using MassTransit;

namespace TrackFetcher
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<IFlightRepository, FlightRepository>();

                    services.AddControllers();

                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();

                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);

                        x.UsingRabbitMq((ctx, cfg) =>
                        {
                            cfg.Host("amqp://guest:guest@track-rabbitmq:5672");

                            cfg.ConfigureEndpoints(ctx);
                        });
                    });
                });
    }
}