using AVFoundation;
using CoreGraphics;
using CoreImage;
using CoreMedia;
using Foundation;
using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.iOS.Service;
using OneTapMobile.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(CreateThumbnailService))]
namespace OneTapMobile.iOS.Service
{
    public class CreateThumbnailService : ICreateThumbnailService
    {
        public ImageSource CreateThumnails(string url, long usecond)
        {
            string filename = "Thumbnail1.png";
            var asset = AVAsset.FromUrl(NSUrl.FromFilename(url));
            AVAssetImageGenerator imageGenerator = new AVAssetImageGenerator(asset);
            imageGenerator.AppliesPreferredTrackTransform = true;
            CMTime actualTime = asset.Duration;
            NSError error;

            var VideoDetails = AVAsset.FromUrl(NSUrl.FromFilename(url));
            var TotalTime = Convert.ToInt64( VideoDetails.Duration.Seconds);
            var FrameCaptureAt = Convert.ToInt64(Convert.ToInt64(TotalTime) / usecond);

            var capturedat = new CMTime(FrameCaptureAt, 1);

            CGImage cgImage = imageGenerator.CopyCGImageAtTime(capturedat, out actualTime, out error);

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "Orders", "ThumbsFolder");
            Helper.thumbfolder = documentsPath;
            Directory.CreateDirectory(documentsPath);

            if (usecond == 1)
            {
                filename = "Thumbnail1.png";

                //string filePath1 = Path.Combine(documentsPath, "VideoToBeUpload.mp4");
                //byte[] bArray1 = new byte[url.Length];
                //using (FileStream fs = new FileStream(filePath1, FileMode.OpenOrCreate))
                //{
                //    int length = bArray1.Length;
                //    fs.Write(bArray1, 0, length);
                //}



            }
            if (usecond == 2)
            {
                filename = "Thumbnail2.png";
            }
            if (usecond == 3)
            {
                filename = "Thumbnail3.png";
            }

            // DependencyService.Get<ISaveFileService>().SavePicture(filename, Imagestream, "imagesFolder");

            var apple = new UIImage(cgImage).AsPNG().AsStream();

            var SimageSource = ImageSource.FromStream(() => new UIImage(cgImage).AsPNG().AsStream());


            string filePath = Path.Combine(documentsPath, filename);
            byte[] bArray = new byte[apple.Length];
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (apple)
                {
                    apple.Read(bArray, 0, (int)apple.Length);
                }
                int length = bArray.Length;
                fs.Write(bArray, 0, length);
            }



            return SimageSource;
        }
    }
}