using Microsoft.Extensions.Caching.Memory;

namespace FlightTracker.FlightHistory.API.Cache
{
    public class FlightCache
    {
        public MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());
    }
}
