using System.Text.Json.Serialization;

namespace VehicleAuctionApp.Models
{
    public class Details
    {
        [JsonPropertyName("specification")]
        public required Specification Specification { get; set; }

        [JsonPropertyName("ownership")]
        public OwnerShip? Ownership { get; set; }

        [JsonPropertyName("equipment")]
        public List<string>? Equipment { get; set; }
    }
}
