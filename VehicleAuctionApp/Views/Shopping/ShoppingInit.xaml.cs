using VehicleAuctionApp.Models;
using VehicleAuctionApp.ViewModels;

namespace VehicleAuctionApp.Views.Shopping;

public partial class ShoppingInit : ContentPage
{
	public ShoppingInit(Vehicle vehicle)
	{
		InitializeComponent();
		BindingContext = new ShoppingPageViewModel(vehicle);
	}
}