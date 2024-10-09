using System.ComponentModel;
using System.Windows.Input;
using VehicleAuctionApp.Models;

namespace VehicleAuctionApp.ViewModels
{

    public class VehicleDetailsViewModel : INotifyPropertyChanged
    {
        #region Properties
        private ImageSource? _imageSource;
        private Vehicle? _vehicle;
        private string _auctionDate;
        private string _auctionTime;
        private DateTime _vehicleAuctionDateTime;

        public ImageSource? ImageSource
        {
            get => _imageSource!;
            set
            {
                if (_imageSource != value)
                {
                    _imageSource = value;
                    OnPropertyChanged(nameof(ImageSource));
                }
            }

        }

        public bool Favourite
        {
            get => _vehicle!.Favourite;
            set
            {
                if (_vehicle!.Favourite != value)
                {
                    _vehicle.Favourite = value;
                    OnPropertyChanged(nameof(Favourite));
                    OnPropertyChanged(nameof(FavouriteIcon));
                }
            }
        }

        public string FavouriteIcon => Favourite ? "star_filled.png" : "star_outline.png";
        public ICommand ToggleFavouriteCommand { get; }

        public Vehicle Vehicle
        {
            get => _vehicle!;
            set
            {

                _vehicle = value;
                OnPropertyChanged(nameof(Vehicle));

            }
        }

        public string TimeUntilAuction
        {
            get
            {
                var timeRemaining = _vehicleAuctionDateTime - DateTime.Now;

                if (timeRemaining.TotalSeconds <= 0)
                    return "Auction has already ended";

                return $"{timeRemaining.Days} days, {timeRemaining.Hours} hours remaining";
            }
        }

        public string AuctionDate => _auctionDate;
        public string AuctionTime => _auctionTime;
        #endregion

        #region Constructor
        public VehicleDetailsViewModel(Vehicle vehicle)
        {
            ImageSource = ImageSource.FromFile("dotnet_bot.png");
            Vehicle = vehicle;
            _auctionDate = vehicle.AuctionDateAndTime.ToString("dd/MM/yyyy");
            _auctionTime = vehicle.AuctionDateAndTime.ToString("hh:mm tt");
            _vehicleAuctionDateTime = vehicle.AuctionDateAndTime;
            ToggleFavouriteCommand = new Command(ToggleFavourite);

        }
        #endregion

        #region Methods
        private void ToggleFavourite()
        {
            Favourite = !Favourite;
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
