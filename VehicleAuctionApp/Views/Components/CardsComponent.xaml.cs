using System.Windows.Input;
using VehicleAuctionApp.Models;

namespace VehicleAuctionApp.Views.Components;

public partial class CardsComponent : Frame
{
    public CardsComponent()
    {
        InitializeComponent();

    }

    public static readonly BindableProperty VehicleProperty =
           BindableProperty.Create(
               nameof(Vehicle),
               typeof(Vehicle),
               typeof(CardsComponent),
               propertyChanged: OnVehicleChanged

             );

    public Vehicle Vehicle
    {
        get => (Vehicle)GetValue(VehicleProperty);
        set => SetValue(VehicleProperty, value);
    }

    public static readonly BindableProperty TapCommandProperty =
           BindableProperty.Create(
               nameof(TapCommand),
               typeof(ICommand),
               typeof(CardsComponent),
               default(ICommand));

    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    private static void OnVehicleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CardsComponent)bindable;
        if (newValue is Vehicle vehicle)
        {
            control.BindingContext = vehicle;

        }
        else
        {
            control.BindingContext = null;
            System.Diagnostics.Debug.WriteLine("CardsComponent received an invalid object for binding.");
        }
    }

}
