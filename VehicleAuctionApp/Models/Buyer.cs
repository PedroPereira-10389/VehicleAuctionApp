using System.Text.Json.Serialization;

namespace VehicleAuctionApp.Models
{
    public class Buyer
    {
        [JsonPropertyName("id")]
        public Guid BuyerId { get; set; }

        [JsonPropertyName("name")]
        public string FullName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }
}
