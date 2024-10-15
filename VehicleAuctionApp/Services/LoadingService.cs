using System.Text.Json;
using VehicleAuctionApp.Models;

namespace VehicleAuctionApp.Services
{
    public class LoadingService
    {
        public async Task<(List<Vehicle> vehicles, List<Auction> auctions)> LoadingFile(string? localJsonFilePath)
        {
            List<Auction> AuctionList = new List<Auction>();
            List<Vehicle> Vehicles = new List<Vehicle>();
            try
            {
                string jsonString = await File.ReadAllTextAsync(localJsonFilePath!);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Vehicles = JsonSerializer.Deserialize<List<Vehicle>>(jsonString, options) ?? new List<Vehicle>();
                AuctionList = Vehicles.OrderBy(v => v.Make).GroupBy(v => new { v.AuctionDateAndTime })
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

            return (Vehicles, AuctionList);
        }

        public async Task<string> CheckFileExistsOrCreate(string? fileName)
        {
            string localJsonFilePath = Path.Combine(FileSystem.AppDataDirectory, fileName!);
            if (!File.Exists(localJsonFilePath))
            {
                try
                {
                    using var stream = await FileSystem.OpenAppPackageFileAsync("vehicles_dataset.json");
                    using var reader = new StreamReader(stream);
                    string json = await reader.ReadToEndAsync();

                    await File.WriteAllTextAsync(localJsonFilePath, json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao copiar o arquivo JSON: {ex.Message}");
                }
            }

            return localJsonFilePath;
        }

        public async Task UpdateVehicleBuyerAsync(Vehicle vehicle, Buyer buyer)
        {
            var vehicles = App.Vehicles;
            var vehicleToUpdate = vehicles?.FirstOrDefault(v => v == vehicle);
            if (vehicleToUpdate != null)
            {
                vehicleToUpdate.Buyer = buyer;
                string jsonFilePath = Path.Combine(FileSystem.AppDataDirectory, "vehicles_dataset.json");
                string updatedJson = JsonSerializer.Serialize(vehicles, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(jsonFilePath, updatedJson);
            }
        }
    }
}
