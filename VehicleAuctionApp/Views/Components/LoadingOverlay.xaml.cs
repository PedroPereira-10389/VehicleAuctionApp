namespace VehicleAuctionApp.Views.Components;

public partial class LoadingOverlay : ContentView
{
	public LoadingOverlay()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty IsBusyProperty =
		   BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(LoadingOverlay), false, propertyChanged: OnIsBusyChanged);

	public bool IsBusy
	{
		get => (bool)GetValue(IsBusyProperty);
		set => SetValue(IsBusyProperty, value);
	}

	private static void OnIsBusyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (LoadingOverlay)bindable;
		bool isBusy = (bool)newValue;
		control.OverlayGrid.IsVisible = isBusy;
	}
}