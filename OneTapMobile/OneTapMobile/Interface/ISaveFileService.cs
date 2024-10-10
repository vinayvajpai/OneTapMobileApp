using System;
using System.IO;

namespace OneTapMobile.Interface
{
    public interface ISaveFileService
    {
        void SavePicture(string name, Stream data, string location = "temp");
    }
}
