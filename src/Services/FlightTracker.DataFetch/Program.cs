using MassTransit;
using System.Reflection;

namespace FlightTracker.DataFetch
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
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        // By default, sagas are in-memory, but should be changed to a durable
                        // saga repository.
                        x.SetInMemorySagaRepositoryProvider();

                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);
                        x.AddSagaStateMachines(entryAssembly);
                        x.AddSagas(entryAssembly);
                        x.AddActivities(entryAssembly);

                        x.UsingRabbitMq((ctx, cfg) =>
                        {
                            cfg.Host("amqp://guest:guest@track-rabbitmq:5672");

                            // this creates and subscribes the consumers
                            // just here as an example, they will be in different projects
                            // cfg.ConfigureEndpoints(ctx);
                        });
                    });

                    // this is only for example
                    // services.AddHostedService<Worker>();

                    services.AddHostedService<Worker>();
                });
    }
}