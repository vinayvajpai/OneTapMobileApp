using System;
using System.Collections.Generic;
using OneTapMobile.ViewModels;
using Xamarin.Forms;
namespace OneTapMobile.Models
{
    public class FBPageCategoryModel
    {
        public ImageSource ListBlowerImage { get; set; }
        public string Description { get; set; }
        public ImageSource ListRightTickImage { get; set; }
    }
    public class AllCategory : BaseViewModel
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
    public class CatResult
    {
        public List<AllCategory> all_categories { get; set; }
        public List<string> user_selected_categories { get; set; }
    }
    public class CatResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public CatResult result { get; set; }
    }
    public class SubmitCatRequestModel
    {
        public string user_id { get; set; }
        public List<string> selected_categories { get; set; }
        public bool is_other { get; set; }
        public string new_category { get; set; }
    }
    public class CatSubmitResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
}
