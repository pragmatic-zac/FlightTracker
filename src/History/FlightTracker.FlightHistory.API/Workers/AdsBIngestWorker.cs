using FlightTracker.Common;
using MassTransit;

namespace FlightTracker.FlightHistory.API.Workers
{
    public class AdsBIngestWorker : IConsumer<ConduitPing>
    {
        private readonly ILogger<AdsBIngestWorker> _logger;

        public AdsBIngestWorker(ILogger<AdsBIngestWorker> logger)
        {
            _logger = logger;
        }
        
        public Task Consume(ConsumeContext<ConduitPing> context)
        {
            var message = context.Message;

            // for now just log this to ensure we're getting it as expected
            Console.WriteLine(message.Flight);

            return Task.CompletedTask;
        }
    }
}
