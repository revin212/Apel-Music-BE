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
            string filePath = _webHostEnvironment.WebRootPath + Id +".png";
            FileStream fstrm = new FileStream(@"C:\winnt_copy.bmp", FileMode.CreateNew, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(fstrm);
            writer.Write(decodedByteArray);
            writer.Close();
            fstrm.Close();
            return (filePath);
        }
    }
}
