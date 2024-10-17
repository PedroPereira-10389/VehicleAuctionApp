using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using VehicleAuctionApp.Models;
using VehicleAuctionApp.Services;

namespace VehicleAuctionApp.ViewModels;

public partial class ShoppingPageViewModel : INotifyPropertyChanged
{
    #region Properties
    private bool _isBusy = false;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged(nameof(IsBusy));
        }
    }


    private Vehicle? _selectedVehicle;
    private readonly LoadingService _loadingService;

    public ICommand SubmitCommand { get; }
    #endregion

    #region Constructors
    public ShoppingPageViewModel(Vehicle vehicle)
    {
        _loadingService = new LoadingService();
        _selectedVehicle = vehicle;
        SubmitCommand = new Command(async () => await Submit());
    }
    #endregion

    #region Methods
    private async Task Submit()
    {
        try
        {
                IsBusy = true;
           

            // Simulate a delay (for demo purposes)
            await Task.Delay(3000);

            // Actual logic (commented lines should be restored when using actual services)
            // _selectedVehicle.Buyer = _buyer;
            // await _loadingService.UpdateVehicleBuyerAsync(_selectedVehicle, _buyer!);
        }
        finally
        {
           
                IsBusy = false;
        
        }
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
