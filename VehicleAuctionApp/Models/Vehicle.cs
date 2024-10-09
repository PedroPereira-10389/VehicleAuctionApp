using System.Text.Json.Serialization;

namespace VehicleAuctionApp.Models
{
    public class Vehicle
    {
        [JsonPropertyName("make")]
        public required string Make { get; set; }

        [JsonPropertyName("model")]
        public required string Model { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("engineSize")]
        public required string EngineSize { get; set; }

        [JsonPropertyName("fuel")]
        public required string FuelType { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("mileage")]
        public int Mileage { get; set; }

        [JsonPropertyName("auctionDateTime")]
        public required string AuctionDateAndTimeRaw { get; set; }

        public DateTime AuctionDateAndTime
        {
            get
            {
                return DateTime.Parse(AuctionDateAndTimeRaw);
            }
        }

        [JsonPropertyName("startingBid")]
        public decimal StartingBid { get; set; }

        [JsonPropertyName("favourite")]
        public bool Favourite { get; set; }

        [JsonPropertyName("details")]
        public required Details Details { get; set; }

    }
}
