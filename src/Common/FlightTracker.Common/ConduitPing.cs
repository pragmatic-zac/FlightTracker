using System.Text.Json.Serialization;

namespace FlightTracker.Common
{
    public class ConduitPing
    {
        [JsonPropertyName("hex")]
        public string Hex { get; set; }

        [JsonPropertyName("callsign")]
        public string Callsign { get; set; }

        [JsonPropertyName("altitude")]
        public int Altitude { get; set; }

        [JsonPropertyName("groundSpeed")]
        public int GroundSpeed { get; set; }

        [JsonPropertyName("track")]
        public int Track { get; set; }

        [JsonPropertyName("latitude")]
        public long Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public long Longitude { get; set; }

        [JsonPropertyName("verticalRate")]
        public int VerticalRate { get; set; }

        [JsonPropertyName("squawk")]
        public int Squawk { get; set; }

        [JsonPropertyName("emergency")]
        public int Emergency { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
    }
}
