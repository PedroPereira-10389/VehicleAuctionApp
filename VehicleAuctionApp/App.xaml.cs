using VehicleAuctionApp.Models;
using VehicleAuctionApp.Services;
using VehicleAuctionApp.Views;

namespace VehicleAuctionApp
{
    public partial class App : Application
    {
        public static List<Auction>? AuctionList { get; private set; }
        public static List<Vehicle>? Vehicles { get; set; }
        private readonly LoadingService _loadingService;

        public App()
        {
            InitializeComponent();
            _loadingService = new LoadingService();
            MainPage = new LoadingPage();
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await LoadData();
        }

        private async Task LoadData()
        {
            var loadingService = new LoadingService();
            string filePath = await loadingService.CheckFileExistsOrCreate("vehicles_dataset.json");
            (Vehicles, AuctionList) = await _loadingService.LoadingFile(filePath);
            MainPage = new AppShell();
        }
    }
}
