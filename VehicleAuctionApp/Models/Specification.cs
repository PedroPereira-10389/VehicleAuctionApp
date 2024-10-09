using System.Text.Json.Serialization;

namespace VehicleAuctionApp.Models
{
    public class Specification
    {
        [JsonPropertyName("vehicleType")]
        public required string VehicleType { get; set; }

        [JsonPropertyName("colour")]
        public required string Colour { get; set; }

        [JsonPropertyName("fuel")]
        public required string Fuel { get; set; }
        [JsonPropertyName("transmission")]
        public required string Transmission { get; set; }

        [JsonPropertyName("numberOfDoors")]
        public int NumberOfDoors { get; set; }

        [JsonPropertyName("co2Emissions")]
        public required string CO2Emissions { get; set; }

        [JsonPropertyName("noxEmissions")]
        public int NOXEmissions { get; set; }

        [JsonPropertyName("numberOfKeys")]
        public int NumberOfKeys { get; set; }
    }
}
