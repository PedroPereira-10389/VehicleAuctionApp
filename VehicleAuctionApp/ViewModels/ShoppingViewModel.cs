using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VehicleAuctionApp.Models;
using VehicleAuctionApp.Services;

namespace VehicleAuctionApp.ViewModels;

public partial class ShoppingPageViewModel : ObservableObject
{
    #region Properties
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private Buyer? _buyer;

    [ObservableProperty]
    private Vehicle? _selectedVehicle;
    private readonly LoadingService _loadingService;
    #endregion

    #region Constructors
    public ShoppingPageViewModel(Vehicle vehicle)
    {
        _loadingService = new LoadingService();
        SelectedVehicle = vehicle;
    }
    #endregion

    #region Methods
    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private async Task SubmitOnClicked(Vehicle? vehicle)
    {
        IsBusy = true;
        vehicle!.Buyer = _buyer;
        await _loadingService.UpdateVehicleBuyerAsync(vehicle, _buyer!);
        IsBusy = false;

    }

    private bool CanSubmit(Vehicle? vehicle)
    {
        return vehicle != null;
    }
    #endregion


}
