using OneTapMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace OneTapMobile.Models
{
    public class PageCategoryList : BaseViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        private bool _NotChecked = true;
        public bool NotChecked
        {
            get
            {
                return _NotChecked;
            }
            set
            {
                _NotChecked = value;
                OnPropertyChanged("NotChecked");
            }
        }
        private bool _PageChecked = false;
        public bool PageChecked
        {
            get
            {
                return _PageChecked;
            }
            set
            {
                _PageChecked = value;
                OnPropertyChanged("PageChecked");
            }
        }
        private Color _SelectionColor = Color.Transparent;
        public Color SelectionColor
        {
            get
            {
                return _SelectionColor;
            }
            set
            {
                _SelectionColor = value;
                OnPropertyChanged("SelectionColor");
            }
        }
    }
    public class FBResult : BaseViewModel
    {
        public string page_id { get; set; }
        public string page_name { get; set; }
        public List<PageCategoryList> page_category_list { get; set; }
        private bool _NotChecked = true;
        public bool NotChecked
        {
            get
            {
                return _NotChecked;
            }
            set
            {
                _NotChecked = value;
                OnPropertyChanged("NotChecked");
            }
        }
        private bool _PageChecked = false;
        public bool PageChecked
        {
            get
            {
                return _PageChecked;
            }
            set
            {
                _PageChecked = value;
                OnPropertyChanged("PageChecked");
            }
        }
        private Color _SelectionColor = Color.Transparent;
        public Color SelectionColor
        {
            get
            {
                return _SelectionColor;
            }
            set
            {
                _SelectionColor = value;
                OnPropertyChanged("SelectionColor");
            }
        }
    }
    public class FBPageResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<FBResult> result { get; set; }
    }
    public class FacebookPageAccess
    {
        public string access_token { get; set; }
        public int OneTapId { get; set; }
    }
    public class FacebookPagesModel
    {
        public int user_id { get; set; }
        public string fb_access_token { get; set; }
    }
}
