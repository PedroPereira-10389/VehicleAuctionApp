using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VehicleAuctionApp.Models;
using VehicleAuctionApp.Models.Components;

namespace VehicleAuctionApp.ViewModels
{
    public class VehiclesPageViewModel : INotifyPropertyChanged
    {
        #region Properties
        private List<Vehicle>? _vehicles;
        private List<CustomColumnDefinition> _auctionColumnDefinitions;
        private Vehicle? _selectedVehicle;
        private int _currentPage = 1;
        public ObservableCollection<Vehicle> FilteredVehicles { get; set; } = new ObservableCollection<Vehicle>();
        private int _selectedVehiclesPerPage;
        private bool _canGoToNextPage;
        private bool _canGoToPreviousPage;

        private Command _nextPageCommand;
        public ICommand NextPageCommand => _nextPageCommand ??= new Command(GoToNextPage, () => CanGoToNextPage);

        private Command _previousPageCommand;
        public ICommand PreviousPageCommand => _previousPageCommand ??= new Command(GoToPreviousPage, () => CanGoToPreviousPage);
        public List<Vehicle> Vehicles
        {
            get => _vehicles!;
            set
            {
                _vehicles = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPages));
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

        public int SelectedVehiclesPerPage
        {
            get => _selectedVehiclesPerPage;
            set
            {
                _selectedVehiclesPerPage = value;
                OnPropertyChanged();
                _currentPage = 1;
                //LoadInitialVehicles();
            }
        }
        public int TotalPages => (int)Math.Ceiling((double)Vehicles.Count / SelectedVehiclesPerPage);
        public string CurrentPageDisplay => $"Page {_currentPage} of {TotalPages}";

        public bool CanGoToNextPage
        {
            get => _canGoToNextPage;
            set
            {
                if (_canGoToNextPage != value)
                {
                    _canGoToNextPage = value;
                    OnPropertyChanged();
                    _nextPageCommand?.ChangeCanExecute();
                }
            }
        }

        public bool CanGoToPreviousPage
        {
            get => _canGoToPreviousPage;
            set
            {
                if (_canGoToPreviousPage != value)
                {
                    _canGoToPreviousPage = value;
                    OnPropertyChanged();
                    _previousPageCommand?.ChangeCanExecute();
                }
            }
        }
        #endregion

        #region Constructor
        public VehiclesPageViewModel()
        {
            _auctionColumnDefinitions = new List<CustomColumnDefinition>();

            InitializeColumnDefinitions();
            SelectedVehiclesPerPage = 10;
            _ = LoadPageDataAsync();
            _nextPageCommand = new Command(GoToNextPage, () => CanGoToNextPage);
            _previousPageCommand = new Command(GoToPreviousPage, () => CanGoToPreviousPage);
        }
        #endregion

        #region Methods
        private async Task LoadPageDataAsync()
        {
            await LoadAuctions();
        }

        private void LoadInitialVehicles()
        {
            if (!Vehicles.Any()) return;

            FilteredVehicles.Clear();
            var initialVehicles = Vehicles.Skip((_currentPage - 1) * SelectedVehiclesPerPage).Take(SelectedVehiclesPerPage).ToList();
            foreach (var vehicle in initialVehicles)
            {
                FilteredVehicles.Add(vehicle);
            }

            CanGoToNextPage = _currentPage < TotalPages;
            CanGoToPreviousPage = _currentPage > 1;

            OnPropertyChanged(nameof(CurrentPageDisplay));
            OnPropertyChanged(nameof(TotalPages));
        }

        private async Task LoadAuctions()
        {
            var auctionList = App.AuctionList;
            if (auctionList != null)
            {
                Vehicles = new List<Vehicle>();
                foreach (var auction in auctionList)
                {
                    foreach (var vehicle in auction.Vehicles)
                    {
                        Vehicles.Add(vehicle);
                    }
                }

                await Task.Run(() => LoadInitialVehicles());
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

        private void InitializeColumnDefinitions()
        {
            AuctionColumnDefinitions = new List<CustomColumnDefinition>
            {
                new CustomColumnDefinition { Header = "Make", BindingProperty = "Make", IsBold = true, Width = 100 },
                new CustomColumnDefinition { Header = "Model", BindingProperty = "Model", IsBold = true, Width = 100 },
                new CustomColumnDefinition { Header = "Auction", BindingProperty = "AuctionDateAndTime", IsBold = true, Width = 100 },
            };
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

        private void GoToNextPage()
        {
            if (_currentPage < TotalPages)
            {
                _currentPage++;
                LoadInitialVehicles();
            }
        }

        private void GoToPreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadInitialVehicles();
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
