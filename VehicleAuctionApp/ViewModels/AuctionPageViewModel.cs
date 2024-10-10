using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VehicleAuctionApp.Models;

namespace VehicleAuctionApp.ViewModels
{
    public class AuctionPageViewModel : INotifyPropertyChanged
    {
        #region Properties
        private List<Auction>? _auctions;
        private Auction? _selectedAuction;
        public List<Auction> Auctions
        {
            get => _auctions!;
            set
            {
                _auctions = value;
                OnPropertyChanged();
            }
        }

        public Auction? SelectedAuction
        {
            get => _selectedAuction;
            set
            {
                if (_selectedAuction != value)
                {
                    _selectedAuction = value;
                    OnPropertyChanged();
                    if (_selectedAuction != null)
                    {
                        NavigateToDetails(_selectedAuction);
                    }
                }
            }
        }

        public ICommand NavigateToDetailsCommand { get; }
        #endregion

        #region Constructor
        public AuctionPageViewModel()
        {
            LoadAuctions();
            NavigateToDetailsCommand = new Command<Auction>(NavigateToDetails);
        }
        #endregion

        #region Methods
        private void LoadAuctions()
        {
            Auctions = App.AuctionList ?? new List<Auction>();
        }


        private async void NavigateToDetails(Auction auction)
        {
            if (auction == null)
                return;

            if (Application.Current?.MainPage?.Navigation != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.Auctions.AuctionDetails(auction));
            }
            SelectedAuction = null;

        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}
