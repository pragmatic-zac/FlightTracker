using System.Text.Json.Serialization;

namespace FlightTracker.Common
{
    /// <summary>
    /// This is used in the FlightTracker.DataFetch, which has slightly different format than Conduit messages
    /// </summary>
    public class ADSBPing
    {
        [JsonPropertyName("hex")]
        public string? Hex { get; set; }

        [JsonPropertyName("flight")]
        public string? Flight { get; set; }

        [JsonPropertyName("alt_baro")]
        public int? BarometricAltitude { get; set; }

        [JsonPropertyName("alt_geom")]
        public int? GeometricAltitude { get; set; }

        [JsonPropertyName("gs")]
        public double? GroundSpeed { get; set; }

        [JsonPropertyName("track")]
        public double? Track { get; set; }

        [JsonPropertyName("baro_rate")]
        public int? BarometricRateOfChange { get; set; }

        [JsonPropertyName("squawk")]
        public string? Squawk { get; set; }

        [JsonPropertyName("emergency")]
        public string? Emergency { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("nav_qnh")]
        public double? NavQnh { get; set; }

        [JsonPropertyName("nav_altitude_mcp")]
        public int? NavAltitudeMcp { get; set; }

        [JsonPropertyName("nav_modes")]
        public List<string>? NavModes { get; set; }

        [JsonPropertyName("lat")]
        public double? Latitude { get; set; }

        [JsonPropertyName("lon")]
        public double? Longitude { get; set; }

        [JsonPropertyName("nic")]
        public int? Nic { get; set; }

        [JsonPropertyName("rc")]
        public int? RC { get; set; }

        [JsonPropertyName("seen_pos")]
        public double? SeenPosition { get; set; }

        [JsonPropertyName("version")]
        public int? Version { get; set; }

        [JsonPropertyName("nic_baro")]
        public int? NicBarometric { get; set; }

        [JsonPropertyName("nac_p")]
        public int? NacP { get; set; }

        [JsonPropertyName("nac_v")]
        public int? NacV { get; set; }

        [JsonPropertyName("sil")]
        public int? Sil { get; set; }

        [JsonPropertyName("sil_type")]
        public string? SilType { get; set; }

        [JsonPropertyName("gva")]
        public int? Gva { get; set; }

        [JsonPropertyName("sda")]
        public int? Sda { get; set; }

        //[JsonPropertyName("mlat")]
        //public List<object> mlat { get; set; }

        //[JsonPropertyName("alt_baro")]
        //public List<object> tisb { get; set; }

        [JsonPropertyName("messages")]
        public int? Messages { get; set; }

        [JsonPropertyName("seen")]
        public double? Seen { get; set; }

        [JsonPropertyName("rssi")]
        public double? Rssi { get; set; }
    }
}