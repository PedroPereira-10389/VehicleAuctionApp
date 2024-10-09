using VehicleAuctionApp.ViewModels;

namespace VehicleAuctionApp.Views.Components;

public partial class FilterPopupPage : ContentPage
{
    public FilterPopupPage(AuctionDetailPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}