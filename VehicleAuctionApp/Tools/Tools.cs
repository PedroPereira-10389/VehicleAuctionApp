namespace VehicleAuctionApp.Tools
{
    public class Helper
    {

        public ImageSource ConvertByteArrayToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            // Converta o byte[] em MemoryStream
            var stream = new MemoryStream(imageData);

            // Retorne o ImageSource
            return ImageSource.FromStream(() => stream);
        }
    }


}
