using Android.Media;
using OneTapMobile.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.Graphics;
using OneTapMobile.Droid.Service;
using System.Diagnostics;
using System;
using OneTapMobile.Global;
using PCLStorage;

[assembly: Xamarin.Forms.Dependency(typeof(CreateThumbnailService))]
namespace OneTapMobile.Droid.Service
{
    public class CreateThumbnailService : ICreateThumbnailService
    {
        IFolder folderName = PCLStorage.FileSystem.Current.LocalStorage;

        public ImageSource CreateThumnails(string url, long Divsecond)
        {
            string filename = "Thumbnail1.png";
            long usecond = 0;
            MediaMetadataRetriever retriever = new MediaMetadataRetriever();
            retriever.SetDataSource(url);
            var TotalTime = retriever.ExtractMetadata(MetadataKey.Duration);
            var lengthseconds = Convert.ToInt32(TotalTime) / 1000;
            if (Divsecond == 1)
            {
                usecond =Convert.ToInt64(lengthseconds);
                filename = "Thumbnail1.png";
            }
            if (Divsecond == 2)
            {
                usecond =Convert.ToInt64(lengthseconds) /2;
                filename = "Thumbnail2.png";
            }
            if (Divsecond == 3)
            {
                usecond =Convert.ToInt64(lengthseconds) /3;
                filename = "Thumbnail3.png";
            }

            long FrameCaptureAt = usecond * 1000000;

           // Bitmap bitmap = retriever.GetFrameAtTime(usecond);
            Bitmap bitmap = retriever.GetScaledFrameAtTime(FrameCaptureAt,Option.ClosestSync,1000,1000);
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png,0, stream);
                byte[] bitmapData = stream.ToArray();

                IFolder folder = PCLStorage.FileSystem.Current.LocalStorage;
                //var sdCardPath = Environment.GetFolderPath(Environment.SpecialFolder.); 

                Helper.thumbfolder = folder.Path;

                var filePath = System.IO.Path.Combine(Helper.thumbfolder, filename);


                    //if(File.Exists(filename))
                    //    File.Delete(filePath);

                var imgstream = new FileStream(filePath, FileMode.Create);
                
               bitmap.Compress(Bitmap.CompressFormat.Png, 100, imgstream);
                stream.Close();

                Helper.BitmapData = bitmapData;
                return  ImageSource.FromStream(() => new MemoryStream(bitmapData));
            }
            return null;
        }

    }
}