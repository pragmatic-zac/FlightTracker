using FlightTracker.Common;
using MassTransit;
using System.Text.Json;

namespace FlightTracker.DataFetch
{
    public class Worker : BackgroundService
    {
        private HttpClient _httpClient;
        private readonly ILogger<Worker> _logger;
        readonly IBus _bus;

        public Worker(ILogger<Worker> logger, IBus bus)
        {
            _bus = bus;
            _logger = logger;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var streamTask = _httpClient.GetStreamAsync("http://192.168.1.190/dump1090-fa/data/aircraft.json");

                var aircraft = await JsonSerializer.DeserializeAsync<PiAwareLog>(await streamTask);

                _logger.LogInformation("Fetched {number} aircraft", aircraft.Aircraft.Count);

                await _bus.Publish(aircraft, stoppingToken);

                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}