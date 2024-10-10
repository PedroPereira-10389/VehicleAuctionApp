namespace VehicleAuctionApp.Tools
{
    public class Helper
    {

        public ImageSource? ConvertByteArrayToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            var stream = new MemoryStream(imageData);

            return ImageSource.FromStream(() => stream);
        }
    }


}
