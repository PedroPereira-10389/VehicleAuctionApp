using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VehicleAuctionApp.Interfaces;
using VehicleAuctionApp.Models;
using VehicleAuctionApp.Views.Components;
using VehicleAuctionApp.Views.Vehicles;

namespace VehicleAuctionApp.ViewModels
{
    public class AuctionDetailPageViewModel : INotifyPropertyChanged
    {
        #region Fields
        private int _selectedVehiclesPerPage;
        private int _currentPage = 1;
        public decimal _filterStartingBid;
        public string _filterMake = string.Empty;
        public string _filterModel = string.Empty;
        private string _selectedSortOption = string.Empty;
        private bool _isDescending;
        private bool _isBusy;
        private bool _canGoToNextPage;
        private bool _canGoToPreviousPage;
        private readonly IDialogService _dialogService;
        private readonly INavigation _navigation;
        private int _totalFilteredVehicles;
        private string _daysRemaining = string.Empty;
        #endregion

        #region Properties
        private Auction? _auction;
        public ObservableCollection<Vehicle> FilteredVehicles { get; set; } = new ObservableCollection<Vehicle>();
        public ObservableCollection<string> Makes { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Models { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<int> VehiclesPerPageOptions { get; set; } = new ObservableCollection<int> { 5, 10, 20, 50 };
        public List<string> SortOptions { get; set; } = new List<string>
        {
            "Make", "Starting Bid", "Mileage", "Auction Date"
        };
        public bool ShowFavouritesOnly { get; set; }

        public decimal MaxBid
        {
            get
            {

                return SelectedAuction.Vehicles.Max(v => v.StartingBid);
            }

        }

        public List<Vehicle> Vehicles => SelectedAuction?.Vehicles ?? new List<Vehicle>();
        public ICommand VehicleTappedCommand { get; }
        public ICommand ApplyFilterCommand { get; }
        public ICommand ApplySortCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand OpenFilterPopupCommand { get; }

        public ICommand OpenSortPopupCommand { get; }

        public Auction SelectedAuction
        {
            get => _auction!;
            set
            {
                _auction = value;
                OnPropertyChanged();
                PopulateMakesAndModels();
                CalculateDaysRemaining();
            }
        }

        private void PopulateMakesAndModels()
        {
            Makes.Clear();
            Models.Clear();

            if (SelectedAuction?.Vehicles != null)
            {
                var uniqueMakes = SelectedAuction.Vehicles.Select(v => v.Make).Distinct().ToList();
                foreach (var make in uniqueMakes)
                {
                    Makes.Add(make);
                }

                var uniqueModels = SelectedAuction.Vehicles.Select(v => v.Model).Distinct().ToList();
                foreach (var model in uniqueModels)
                {
                    Models.Add(model);
                }
            }
        }

        public string FilterMake
        {
            get => _filterMake;
            set
            {
                _filterMake = value;
                OnPropertyChanged();
            }
        }

        public string FilterModel
        {
            get => _filterModel;
            set
            {
                _filterModel = value;
                OnPropertyChanged();
            }
        }

        public decimal FilterStartingBid
        {
            get => _filterStartingBid;
            set
            {
                if (_filterStartingBid != value)
                {
                    _filterStartingBid = value;
                    OnPropertyChanged(nameof(FilterStartingBid));
                }
            }
        }

        public bool IsDescending
        {
            get => _isDescending;
            set
            {
                _isDescending = value;
                OnPropertyChanged();
            }
        }

        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                _selectedSortOption = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
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
                LoadInitialVehicles();
            }
        }

        public int TotalPages => (int)Math.Ceiling((double)_totalFilteredVehicles / SelectedVehiclesPerPage);
        public string CurrentPageDisplay => $"Page {_currentPage} of {TotalPages}";

        public bool CanGoToNextPage
        {
            get => _canGoToNextPage;
            set
            {
                _canGoToNextPage = value;
                OnPropertyChanged();
            }
        }

        public bool CanGoToPreviousPage
        {
            get => _canGoToPreviousPage;
            set
            {
                _canGoToPreviousPage = value;
                OnPropertyChanged();
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                LoadInitialVehicles();
            }
        }

        public string DaysRemaining
        {
            get => _daysRemaining;
            set
            {
                _daysRemaining = value;
                OnPropertyChanged(nameof(DaysRemaining));
            }
        }
        #endregion

        #region Constructor
        public AuctionDetailPageViewModel(Auction auction, IDialogService dialogService, INavigation navigation)
        {
            _dialogService = dialogService;
            _navigation = navigation;
            SelectedAuction = auction;
            SelectedVehiclesPerPage = 5;
            _totalFilteredVehicles = auction.VehiclesCount;
            VehicleTappedCommand = new Command<Vehicle>(OnVehicleTapped);
            ApplyFilterCommand = new Command(ApplyFilters);
            ApplySortCommand = new Command(ApplySort);
            NextPageCommand = new Command(GoToNextPage);
            PreviousPageCommand = new Command(GoToPreviousPage);
            OpenFilterPopupCommand = new Command(OpenFilterPopup);
            OpenSortPopupCommand = new Command(OpenSortPopup);
            LoadInitialVehicles();
        }
        #endregion

        #region Methods
        private void LoadInitialVehicles()
        {
            if (SelectedAuction == null || !SelectedAuction.Vehicles.Any()) return;

            FilteredVehicles.Clear();
            var initialVehicles = Vehicles.Skip((_currentPage - 1) * SelectedVehiclesPerPage).Take(SelectedVehiclesPerPage).ToList();
            foreach (var vehicle in initialVehicles)
            {
                FilteredVehicles.Add(vehicle);
            }

            CanGoToNextPage = _currentPage < TotalPages;
            CanGoToPreviousPage = _currentPage > 1;

            OnPropertyChanged(nameof(CurrentPageDisplay));
            OnPropertyChanged(nameof(FilteredVehicles));

        }

        private async void OnVehicleTapped(Vehicle vehicle)
        {
            if (vehicle == null)
                return;

            if (Application.Current?.MainPage?.Navigation != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new VehicleDetails(vehicle));
            }
        }

        private async void ApplyFilters()
        {
            await _navigation.PopModalAsync();
            IsBusy = true;

            var filtered = Vehicles.Where(v =>
                (string.IsNullOrEmpty(FilterMake) || v.Make == FilterMake) &&
                (string.IsNullOrEmpty(FilterModel) || v.Model == FilterModel) &&
                (FilterStartingBid == 0 || v.StartingBid <= FilterStartingBid) &&
                (!ShowFavouritesOnly || v.Favourite == ShowFavouritesOnly)
            ).ToList();
            CurrentPage = 1;
            _totalFilteredVehicles = filtered.Count;
            FilteredVehicles.Clear();
            var paginatedVehicles = filtered.Skip((CurrentPage - 1) * SelectedVehiclesPerPage).Take(SelectedVehiclesPerPage).ToList();

            foreach (var vehicle in paginatedVehicles)
            {
                FilteredVehicles.Add(vehicle);
            }

            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(CurrentPageDisplay));
            OnPropertyChanged(nameof(FilteredVehicles));
            IsBusy = false;
        }

        private async void ApplySort()
        {
            await _navigation.PopModalAsync();
            IsBusy = true;
            await Task.Run(() =>
            {

                IEnumerable<Vehicle> sortedVehicles = Vehicles;

                switch (SelectedSortOption)
                {
                    case "Make":
                        sortedVehicles = IsDescending
                            ? sortedVehicles.OrderByDescending(v => v.Make)
                            : sortedVehicles.OrderBy(v => v.Make);
                        break;
                    case "Starting Bid":
                        sortedVehicles = IsDescending
                            ? sortedVehicles.OrderByDescending(v => v.StartingBid)
                            : sortedVehicles.OrderBy(v => v.StartingBid);
                        break;
                    case "Mileage":
                        sortedVehicles = IsDescending
                            ? sortedVehicles.OrderByDescending(v => v.Mileage)
                            : sortedVehicles.OrderBy(v => v.Mileage);
                        break;
                    case "Auction Date":
                        sortedVehicles = IsDescending
                            ? sortedVehicles.OrderByDescending(v => v.AuctionDateAndTime)
                            : sortedVehicles.OrderBy(v => v.AuctionDateAndTime);
                        break;
                }

                var sortedList = sortedVehicles.ToList();
                var paginatedVehicles = sortedList
                    .Skip((CurrentPage - 1) * SelectedVehiclesPerPage)
                    .Take(SelectedVehiclesPerPage)
                    .ToList();

                Application.Current!.Dispatcher.Dispatch(() =>
                {
                    FilteredVehicles.Clear();

                    foreach (var vehicle in paginatedVehicles)
                    {
                        FilteredVehicles.Add(vehicle);
                    }

                    OnPropertyChanged(nameof(TotalPages));
                    OnPropertyChanged(nameof(CurrentPageDisplay));
                    OnPropertyChanged(nameof(FilteredVehicles));
                });
            });

            IsBusy = false;
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

        private async void OpenFilterPopup()
        {

            await _navigation.PushModalAsync(new FilterPopupPage(this));
        }

        private async void OpenSortPopup()
        {

            await _navigation.PushModalAsync(new SortPopupPage(this));
        }

        private void CalculateDaysRemaining()
        {
            var timeSpan = SelectedAuction.DateTime - DateTime.Now;

            if (timeSpan.TotalDays > 0)
            {
                DaysRemaining = $"{(int)timeSpan.TotalDays} day(s) remaining";
            }
            else
            {
                DaysRemaining = "Auction is ongoing!";
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
