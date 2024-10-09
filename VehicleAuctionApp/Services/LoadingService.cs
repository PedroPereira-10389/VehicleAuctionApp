using System.Text.Json;
using VehicleAuctionApp.Models;

namespace VehicleAuctionApp.Services
{
    public class LoadingService
    {
        public async Task<List<Auction>> LoadingFile(string? filePath)
        {
            List<Auction> AuctionList = new List<Auction>();
            try
            {
                using Stream stream = await FileSystem.OpenAppPackageFileAsync(filePath!);
                using StreamReader reader = new StreamReader(stream);
                string jsonString = await reader.ReadToEndAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var vehicles = JsonSerializer.Deserialize<List<Vehicle>>(jsonString, options) ?? new List<Vehicle>();
                AuctionList = vehicles.OrderBy(v => v.Make).GroupBy(v => new { v.AuctionDateAndTime })
                                      .Select(g => new Auction
                                      {
                                          DateAndTimeRaw = g.Key.AuctionDateAndTime.ToString(),
                                          Vehicles = g.ToList()
                                      })
                                      .ToList();
                Console.WriteLine($"Conteúdo do JSON: {jsonString}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar dados: {ex.Message}");
                AuctionList = new List<Auction>();
            }

            return AuctionList;
        }
    }
}
