using VehicleAuctionApp.ViewModels;

namespace VehicleAuctionApp.Views.Components;

public partial class SortPopupPage : ContentPage
{
    public SortPopupPage(AuctionDetailPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}