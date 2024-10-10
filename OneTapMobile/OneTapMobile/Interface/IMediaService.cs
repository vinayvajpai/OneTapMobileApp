using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Interface
{
    public interface IMediaService
    {
        byte[] ResizeImage(byte[] imageData, float width, float height, string format);
    }
}
