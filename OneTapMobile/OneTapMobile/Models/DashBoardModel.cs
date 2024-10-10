using OneTapMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
namespace OneTapMobile.Models
{
    public class CampaignData : BaseViewModel
    {
        public string CampTitle { get; set; }
        public string CampObjective { get; set; }
        public string CampSpentAmount { get; set; }
        public string TotalCampAmount { get; set; }
        public string PercentSpentAmount { get; set; }
        public float CampaignProgress { get; set; }


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
}
