using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VehicleAuctionApp.Models;
using VehicleAuctionApp.Models.Components;


namespace VehicleAuctionApp.ViewModels
{
    public class HomePageViewModel
    {
        #region Properties
        private DateTime _nextAuctionDate;
        private string? _daysRemaining;
        private List<CustomColumnDefinition> _auctionColumnDefinitions;
        private Vehicle? _selectedVehicle;
        private Vehicle? _highestBidVehicle;
        public ObservableCollection<Vehicle>? MostFavoritedVehicles { get; set; }
        public string DaysRemaining
        {
            get => _daysRemaining!;
            set
            {
                _daysRemaining = value;
                OnPropertyChanged(nameof(DaysRemaining));
            }
        }

        public DateTime NextAuctionDate
        {
            get => _nextAuctionDate;
            set
            {
                _nextAuctionDate = value;
                OnPropertyChanged(nameof(NextAuctionDate));
                CalculateDaysRemaining();
            }
        }

        public Vehicle? HighestBidVehicle
        {
            get => _highestBidVehicle;
            set
            {
                _highestBidVehicle = value;
                OnPropertyChanged(nameof(HighestBidVehicle));

            }
        }

        public Vehicle? SelectedVehicle
        {
            get => _selectedVehicle!;
            set
            {
                if (_selectedVehicle != value)
                {
                    _selectedVehicle = value;
                    OnPropertyChanged();
                    if (_selectedVehicle != null)
                    {
                        NavigateToDetails(_selectedVehicle);
                    }
                }
            }
        }

        public List<CustomColumnDefinition> AuctionColumnDefinitions
        {
            get => _auctionColumnDefinitions;
            set
            {
                if (_auctionColumnDefinitions != value)
                {
                    _auctionColumnDefinitions = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Constructor
        public HomePageViewModel()
        {
            _auctionColumnDefinitions = new List<CustomColumnDefinition>();
            InitializeColumnDefinitions();
            LoadData();
        }
        #endregion

        #region Methods
        private void InitializeColumnDefinitions()
        {
            AuctionColumnDefinitions = new List<CustomColumnDefinition>
            {
                new CustomColumnDefinition { Header = "Make", BindingProperty = "Make", IsBold = true, Width = 100 },
                new CustomColumnDefinition { Header = "Model", BindingProperty = "Model", IsBold = true, Width = 100 },
                new CustomColumnDefinition { Header = "Year", BindingProperty = "Year", IsBold = true, Width = 100 },
            };
        }

        private void LoadData()
        {
            var auctions = App.AuctionList ?? new List<Auction>();
            var nexAuction = auctions
                .Where(a => a.DateTime >= DateTime.Now)
                .OrderBy(a => a.DateTime)
                .FirstOrDefault() ?? new Auction { DateAndTimeRaw = "", Vehicles = new List<Vehicle>() };
            NextAuctionDate = nexAuction.DateTime;
            List<Vehicle> vehiclesList = nexAuction.Vehicles;
            foreach (var vehicle in vehiclesList)
            {
                vehicle.PropertyChanged += Vehicle_PropertyChanged;
            }
            MostFavoritedVehicles = new ObservableCollection<Vehicle>(vehiclesList.Where(v => v.Favourite));
            HighestBidVehicle = vehiclesList.OrderByDescending(v => v.StartingBid).FirstOrDefault();
        }

        private async void NavigateToDetails(Vehicle vehicle)
        {
            if (vehicle == null)
                return;
            SelectedVehicle = null;
            if (Application.Current?.MainPage?.Navigation != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.Vehicles.VehicleDetails(vehicle));
            }
        }


        private void CalculateDaysRemaining()
        {
            var timeSpan = NextAuctionDate - DateTime.Now;

            if (timeSpan.TotalDays > 0)
            {
                DaysRemaining = $"{(int)timeSpan.TotalDays} day(s) remaining";
            }
            else
            {
                DaysRemaining = "Auction started!";
            }
        }

        private void Vehicle_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Vehicle.Favourite))
            {
                var vehicle = sender as Vehicle;
                if (vehicle != null)
                {
                    if (vehicle.Favourite)
                    {
                        if (!MostFavoritedVehicles!.Contains(vehicle))
                        {
                            MostFavoritedVehicles!.Add(vehicle);
                        }
                    }
                    else
                    {
                        if (MostFavoritedVehicles!.Contains(vehicle))
                        {
                            MostFavoritedVehicles.Remove(vehicle);
                        }
                    }
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}
