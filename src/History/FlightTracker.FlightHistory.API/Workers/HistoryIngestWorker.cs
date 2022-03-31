using FlightTracker.Common;
using FlightTracker.FlightHistory.API.Models;
using FlightTracker.FlightHistory.API.Repository;
using MassTransit;
using Microsoft.Extensions.Caching.Memory;

namespace FlightTracker.FlightHistory.API.Workers
{
    public class HistoryIngestWorker : IConsumer<PiAwareLog>
    {
        readonly ILogger<HistoryIngestWorker> _logger;
        private readonly IMemoryCache _cache;
        private readonly IFlightRepository _repository;

        public HistoryIngestWorker(ILogger<HistoryIngestWorker> logger, IMemoryCache cache, IFlightRepository repository)
        {
            _logger = logger;
            _cache = cache;
            _repository = repository;
        }

        // TODO: add expiration on cache
        public async Task Consume(ConsumeContext<PiAwareLog> context)
        {
            var message = context.Message;

            foreach (var aicraft in message.Aircraft)
            {
                int existingId = 0;

                _cache.TryGetValue(aicraft.Hex, out existingId);

                if (existingId != 0)
                {
                    // plane has already been added to the DB, append this to the existing flight
                    await _repository.AddFlightStatus(aicraft, existingId);
                }
                else
                {
                    // this is a new instance of this hex (at this point in time)
                    _logger.LogInformation("New flight seen: {hex}", aicraft.Hex);

                    // guaranteed to get hex every time, but everything else may be null
                    // TODO: what if flight number is null on first ping and we get it later on? need a way to patch it
                    var flightToAdd = new Flight
                    {
                        Hex = aicraft.Hex,
                        FlightNumber = aicraft.Flight
                    };

                    // add the flight to the db and get the ID back
                    int newId = await _repository.AddFlight(flightToAdd);

                    // add this ping to the new flight
                    await _repository.AddFlightStatus(aicraft, newId);

                    // add that ID to cache
                    _cache.Set(aicraft.Hex, newId);
                }
            }

            return;
        }
    }
}
