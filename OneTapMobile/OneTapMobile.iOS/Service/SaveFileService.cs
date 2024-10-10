using System;
using System.IO;
using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.iOS.Service;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveFileService))]
namespace OneTapMobile.iOS.Service
{
    public class SaveFileService: ISaveFileService
    {
        public void SavePicture(string name, Stream data, string location = "temp")
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "Orders", location);
            Directory.CreateDirectory(documentsPath);

           string filePath = Path.Combine(documentsPath, name);

            byte[] bArray = new byte[data.Length];
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (data)
                {
                    data.Read(bArray, 0, (int)data.Length);
                }
                int length = bArray.Length;
                fs.Write(bArray, 0, length);
            }
        }
    }
}
