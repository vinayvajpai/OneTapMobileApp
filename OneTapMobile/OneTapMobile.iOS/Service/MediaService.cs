using Foundation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UIKit;

namespace OneTapMobile.iOS.Service
{
    public class MediaService
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height, string format)
        {

            UIImage originalImage = ImageFromByteArray(imageData);

            var originalHeight = originalImage.Size.Height;
            var originalWidth = originalImage.Size.Width;

            nfloat newHeight = 0;
            nfloat newWidth = 0;

            if (originalHeight > originalWidth)
            {
                newHeight = height;
                nfloat ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                nfloat ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            width = (float)newWidth;
            height = (float)newHeight;

            UIGraphics.BeginImageContext(new SizeF(width, height));
            originalImage.Draw(new RectangleF(0, 0, width, height));
            var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            if (format == "gif")
            {
                var bytesImagen = resizedImage.AsJPEG().ToArray();
                resizedImage.Dispose();
                return bytesImagen;
            }
            else if (format == "png")
            {
                var bytesImagen = resizedImage.AsPNG().ToArray();
                resizedImage.Dispose();
                return bytesImagen;
            }
            else if (format == "jpg")
            {
                var bytesImagen = resizedImage.AsJPEG().ToArray();
                resizedImage.Dispose();
                return bytesImagen;
            }
            else
            {
                var bytesImagen = resizedImage.AsJPEG().ToArray();
                resizedImage.Dispose();
                return bytesImagen;
            }

        }

        private UIImage ImageFromByteArray(byte[] imageData)
        {
            if (imageData == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(imageData));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
    }
}