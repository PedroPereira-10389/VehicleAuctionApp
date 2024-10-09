using VehicleAuctionApp.Models;
using VehicleAuctionApp.ViewModels;

namespace VehicleAuctionApp.Views.Vehicles;

public partial class VehicleDetails : ContentPage
{
    public VehicleDetails(Vehicle vehicle)
    {
        InitializeComponent();
        BindingContext = new VehicleDetailsViewModel(vehicle);
    }



}