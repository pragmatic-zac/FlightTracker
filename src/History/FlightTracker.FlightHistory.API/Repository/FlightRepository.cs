using FlightTracker.Common;
using Npgsql;
using Dapper;
using FlightTracker.FlightHistory.API.Models;

namespace FlightTracker.FlightHistory.API.Repository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly string _connectionString;

        public FlightRepository()
        {
            _connectionString = "Server=FlightHistoryDb;Port=5432;Database=FlightHistoryDb;User Id=admin;Password=admin1234;";
        }

        public async Task<int> AddFlight(Flight flight)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            string sql = @"INSERT INTO Flight (Hex, FlightNumber) VALUES (@Hex, @FlightNumber) RETURNING Id";

            var p = new
            {
                Hex = flight.Hex,
                FlightNumber = flight.FlightNumber,
            };

            int result = await connection.ExecuteScalarAsync<int>(sql, param: p);

            return result;
        }

        public async Task<bool> AddFlightStatus(ADSBPing adsPing, int flightId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            string sql = @"INSERT INTO FlightStatus 
                                (FlightId, BarometricAltitude, GeometricAltitude, GroundSpeed, 
                                    Track, BarometricRateOfChange, Squawk, Latitude, Longitude) 
                            VALUES (@FlightId, @BarometricAltitude, @GeometricAltitude, @GroundSpeed, @Track,
                                    @BarometricRateOfChange, @Squawk, @Latitude, @Longitude) RETURNING Id";

            var p = new
            {
                FlightId = flightId,
                BarometricAltitude = adsPing.BarometricAltitude,
                GeometricAltitude = adsPing.GeometricAltitude,
                GroundSpeed = adsPing.GroundSpeed,
                Track = adsPing.Track,
                BarometricRateOfChange = adsPing.BarometricRateOfChange,
                Squawk = adsPing.Squawk,
                Latitude = adsPing.Latitude,
                Longitude = adsPing.Longitude,
            };

            int result = await connection.ExecuteAsync(sql, param: p);

            return result > 0;
        }

        public async Task<IEnumerable<ADSBPing>> GetTrackByFlightId(int flightId)
        {
            throw new NotImplementedException();
        }

    }
}
