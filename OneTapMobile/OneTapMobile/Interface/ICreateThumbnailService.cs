using OneTapMobile.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OneTapMobile.Services
{
    public interface ICreateThumbnailService
    {
        ImageSource CreateThumnails(string url, long usecond);
    }
}
