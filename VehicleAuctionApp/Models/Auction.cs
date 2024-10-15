using System.Text.Json.Serialization;

namespace VehicleAuctionApp.Models
{
    public class Auction
    {

        [JsonPropertyName("auctionDateTime")]
        public required string? DateAndTimeRaw { get; set; }

        public DateTime DateTime
        {
            get
            {
                return DateTime.Parse(DateAndTimeRaw!);
            }
        }

        [JsonPropertyName("Vehicles")]
        public required List<Vehicle> Vehicles { get; set; }

        public int VehiclesCount => Vehicles?.Count ?? 0;
    }
}
