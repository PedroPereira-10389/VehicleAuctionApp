namespace VehicleAuctionApp.Interfaces
{
    public interface IDialogService
    {
        Task<string> ShowActionSheet(string title, string cancel, string destruction, params string[] buttons);
        Task ShowAlert(string title, string message, string cancel);
    }
}
