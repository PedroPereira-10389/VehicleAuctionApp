using VehicleAuctionApp.Models;
using VehicleAuctionApp.Services;
using VehicleAuctionApp.ViewModels;

namespace VehicleAuctionApp.Views.Auctions;

public partial class AuctionDetails : ContentPage
{
    private AuctionDetailPageViewModel _viewModel;

    public AuctionDetails(Auction selectedAuction)
    {
        InitializeComponent();
        var dialogService = new DialogService(this);
        _viewModel = new AuctionDetailPageViewModel(selectedAuction, dialogService, Navigation);
        BindingContext = _viewModel;
    }
}