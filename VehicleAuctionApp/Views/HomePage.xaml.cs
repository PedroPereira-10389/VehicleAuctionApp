using VehicleAuctionApp.ViewModels;

namespace VehicleAuctionApp.Views;

public partial class HomePage : ContentPage
{
    private HomePageViewModel _viewModel;
    public HomePage()
	{
		InitializeComponent();
        _viewModel = new HomePageViewModel();
        BindingContext = _viewModel;
    }
}