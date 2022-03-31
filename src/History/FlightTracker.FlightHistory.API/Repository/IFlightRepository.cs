using FlightTracker.Common;
using FlightTracker.FlightHistory.API.Models;

namespace FlightTracker.FlightHistory.API.Repository
{
    public interface IFlightRepository
    {
        public Task<int> AddFlight(Flight flight);

        public Task<bool> AddFlightStatus(ADSBPing adsPing, int flightId);

        public Task<IEnumerable<ADSBPing>> GetTrackByFlightId(int flightId);
    }
}
