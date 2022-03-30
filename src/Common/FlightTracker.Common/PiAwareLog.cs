using FlightTracker.Common;
using System.Text.Json.Serialization;

namespace FlightTrack.Common
{
    public class PiAwareLog
    {
        [JsonPropertyName("now")]
        public decimal Now { get; set; }

        [JsonPropertyName("messages")]
        public int Messages { get; set; }

        [JsonPropertyName("aircraft")]
        public List<ADSBPing> Aircraft { get; set; }
    }
}