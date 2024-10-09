using VehicleAuctionApp.ViewModels;

namespace VehicleAuctionApp.Views.Vehicles;

public partial class VehiclesInit : ContentPage
{
    public VehiclesInit()
    {
        InitializeComponent();
        BindingContext = new VehiclesPageViewModel();
    }
}