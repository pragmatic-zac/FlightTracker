using FlightTracker.Common;
using MassTransit;

namespace FlightTracker.FlightHistory.API.Workers
{
    public class HistoryIngestWorker : IConsumer<PiAwareLog>
    {
        readonly ILogger<HistoryIngestWorker> _logger;

        public HistoryIngestWorker(ILogger<HistoryIngestWorker> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PiAwareLog> context)
        {
            _logger.LogInformation("Recieved message");

            var test = context;

            return Task.CompletedTask;
        }
    }
}
