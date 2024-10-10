using OneTapMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OneTapMobile.Services
{
    public interface IFBCampListService
    {
            Task FBCampList(INavigation nav, FBCampListRequestModel fBCampListRequestModel, Action<List<Campaign>> response);
    }
}
