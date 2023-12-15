using Microsoft.AspNetCore.Hosting;

namespace fs_12_team_1_BE.Utilities
{
    public class ImageSaverUtil
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageSaverUtil(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string SaveImageToFile(string inputStr, Guid Id)
        {
            byte[] decodedByteArray = Convert.FromBase64CharArray(
                inputStr.ToCharArray(), 0, inputStr.Length);
            string filePath = _webHostEnvironment.WebRootPath + "\\images\\" + Id +".png";
            
            FileStream stream = new FileStream(@filePath, FileMode.Create, FileAccess.Write);
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(decodedByteArray);
                writer.Flush();
                writer.Close();
            }
            stream.Close();
            Uri absoluteUri = new Uri(filePath);
            Uri rootUri = new Uri(_webHostEnvironment.WebRootPath);
            
            
            Uri relativeUri = rootUri.MakeRelativeUri(absoluteUri);
            return Uri.UnescapeDataString(relativeUri.ToString()).ToString().Substring(7);

        }
    }
}
