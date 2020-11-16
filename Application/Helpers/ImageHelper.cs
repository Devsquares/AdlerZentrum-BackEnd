using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Application.Helpers
{
    public class ImageHelper
    {
        
        public static string Base64ToImage(string base64String, string serverPath, string product)
        {
            string filePath = GetFilePath(serverPath, product);
            var bytes = Convert.FromBase64String(base64String);
            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
            return product + ".jpg";
        }

        public static string GetFilePath(string serverPath, string ImgName)
        {
            string imagesPath = serverPath +  "/ImageStorage"; //Path
         
            //Check if directory exist
            if (!System.IO.Directory.Exists(imagesPath))
            {
                System.IO.Directory.CreateDirectory(imagesPath); //Create directory if it doesn't exist
            }

            string imageName = ImgName + ".jpg";

            //set the image path
            string imgPath = Path.Combine(imagesPath, imageName);
            return imgPath;
        }

        public static string GetFullFilePath(string serverPath, string ImgName)
        {
            string imagesPath = serverPath + "/ImageStorage"; //Path

            //Check if directory exist
            if (!System.IO.Directory.Exists(imagesPath))
            {
                System.IO.Directory.CreateDirectory(imagesPath); //Create directory if it doesn't exist
            }

            string imageName = ImgName + ".jpg";

            //set the image path
            string imgPath = Path.Combine(imagesPath, imageName);
            return imgPath;
        }
    }
}
