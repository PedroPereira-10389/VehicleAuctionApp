namespace VehicleAuctionApp.Views.Components
{
	public partial class LoadingOverlay : ContentView
	{
		public LoadingOverlay()
		{
			InitializeComponent();
			BindingContext = this;
		}

		public static readonly BindableProperty IsOverlayVisibleProperty =
			BindableProperty.Create(nameof(IsOverlayVisible), typeof(bool), typeof(LoadingOverlay), false, propertyChanged: OnIsOverlayVisibleChanged);

		public bool IsOverlayVisible
		{
			get => (bool)GetValue(IsOverlayVisibleProperty);
			set => SetValue(IsOverlayVisibleProperty, value);
		}

		private static void OnIsOverlayVisibleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var control = (LoadingOverlay)bindable;
			bool isVisible = (bool)newValue;
			control.OverlayGrid.IsVisible = isVisible;
		}
	}
}
