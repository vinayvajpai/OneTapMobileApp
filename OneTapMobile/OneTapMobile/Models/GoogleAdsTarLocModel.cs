using OneTapMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.Models
{
    public class GoAdsTarLocRequestModel
    {
        public string search_text { get; set; }
    }

    public class GoAdsTarLocResult : BaseViewModel
    {
        public int id { get; set; }
        public string name { get; set; }

        private string _Tick = "RightTickGray";
        public string Tick
        {
            get
            {
                return _Tick;
            }
            set
            {
                _Tick = value;
                OnPropertyChanged("Tick");
            }
        }
    }

    public class GoAdsTarLocResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<GoAdsTarLocResult> result { get; set; }
    }

}
