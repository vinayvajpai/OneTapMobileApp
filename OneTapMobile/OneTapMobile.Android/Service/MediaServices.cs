using System;
using System.IO;
using Android.Graphics;
using OneTapMobile.Interface;
using OneTapMobile.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MediaService))]
namespace OneTapMobile.Services
{
    public class MediaService : IMediaService
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height, string format)
        {
            // Load the bitmap 
            BitmapFactory.Options options = new BitmapFactory.Options();// Create object of bitmapfactory's option method for further option use
            options.InPurgeable = true; // inPurgeable is used to free up memory while required
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);

            float newHeight = 0;
            float newWidth = 0;

            var originalHeight = originalImage.Height;
            var originalWidth = originalImage.Width;

            if (originalHeight > originalWidth)
            {
                newHeight = height;
                float ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                float ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, true);

            originalImage.Recycle();

            using (MemoryStream ms = new MemoryStream())
            {
                if (format == "gif")
                {
                    resizedImage.Compress(Bitmap.CompressFormat.Webp, 100, ms);
                }
                else if (format == "png")
                {
                    resizedImage.Compress(Bitmap.CompressFormat.Png, 100, ms);
                }
                else if (format == "jpg")
                {
                    resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                }
                else
                {
                    resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                }

                resizedImage.Recycle();

                return ms.ToArray();
            }

        }
    }
}
