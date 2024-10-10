using Newtonsoft.Json;
using OneTapMobile.Global;
using OneTapMobile.Models;
using OneTapMobile.Popups;
using OneTapMobile.Views;
using OneTapMobile.Views.AI;
using PCLStorage;
using Plugin.Media.Abstractions;
using RestSharp;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class SelectVideoThumbViewModel : BaseViewModel
    {
        #region properties

        public INavigation nav;

        private ImageSource thumbnail1;
        public ImageSource Thumbnail1
        {
            get { return thumbnail1; }
            set { thumbnail1 = value; }
        }


        private ImageSource thumbnail2;
        public ImageSource Thumbnail2
        {
            get { return thumbnail2; }
            set { thumbnail2 = value; }
        }


        private ImageSource thumbnail3;
        public ImageSource Thumbnail3
        {
            get { return thumbnail3; }
            set { thumbnail3 = value; }
        }

        private FileResult _VideoFile;
        public FileResult VideoFile
        {
            get { return _VideoFile; }
            set
            {
                _VideoFile = value;
                OnPropertyChanged("VideoFile");
            }
        }



        private string thumbnail1Border;
        public string Thumbnail1Border
        {
            get { return thumbnail1Border; }
            set
            {
                thumbnail1Border = value;
                OnPropertyChanged("Thumbnail1Border");
            }
        }

        private string thumbnail2Border;
        public string Thumbnail2Border
        {
            get
            {
                return thumbnail2Border;
            }
            set
            {
                thumbnail2Border = value;
                OnPropertyChanged("Thumbnail2Border");
            }
        }

        private string thumbnail3Border;
        public string Thumbnail3Border
        {
            get
            {
                return thumbnail3Border;
            }
            set
            {
                thumbnail3Border = value;
                OnPropertyChanged("Thumbnail3Border");
            }
        }

        #endregion

        #region commands

        private Command backCommand;
        public ICommand BackCommand
        {
            get
            {
                if (backCommand == null)
                {
                    backCommand = new Command(Back);
                }

                return backCommand;
            }
        }

        private Command helpCommand;
        public ICommand HelpCommand
        {
            get
            {
                if (helpCommand == null)
                {
                    helpCommand = new Command(Help);
                }

                return helpCommand;
            }
        }

        private Command thumbnail1_Tapped;
        public ICommand Thumbnail1_Tapped
        {
            get
            {
                if (thumbnail1_Tapped == null)
                {
                    thumbnail1_Tapped = new Command(PerformThumbnail1_Tapped);
                }

                return thumbnail1_Tapped;
            }
        }

        private Command thumbnail2_Tapped;
        public ICommand Thumbnail2_Tapped
        {
            get
            {
                if (thumbnail2_Tapped == null)
                {
                    thumbnail2_Tapped = new Command(PerformThumbnail2_Tapped);
                }

                return thumbnail2_Tapped;
            }
        }

        private Command thumbnail3_Tapped;
        public ICommand Thumbnail3_Tapped
        {
            get
            {
                if (thumbnail3_Tapped == null)
                {
                    thumbnail3_Tapped = new Command(PerformThumbnail3_Tapped);
                }

                return thumbnail3_Tapped;
            }
        }

        private Command changeVideoCommand;
        public ICommand ChangeVideoCommand
        {
            get
            {
                if (changeVideoCommand == null)
                {
                    changeVideoCommand = new Command(ChangeVideo);
                }

                return changeVideoCommand;
            }
        }

        private Command selectThumbnailCommand;
        public ICommand SelectThumbnailCommand
        {
            get
            {
                if (selectThumbnailCommand == null)
                {
                    selectThumbnailCommand = new Command(SelectThumbnail);
                }

                return selectThumbnailCommand;
            }
        }

        #endregion

        #region methods
        async private void SelectThumbnail()
        {
            try
            {
                if (IsTap)
                {
                    return;
                }
                else
                {
                    IsTap = true;
                    await nav.PushAsync(new TapAndFillView());
                    //await nav.PushAsync(new AdProfileTemplateView(Helper.videoCampaign.SelectedThumb));
                }
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsTap = false;
            }

        }

        private void Back()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                nav.PopAsync();
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        private void Help()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                nav.PushAsync(new HelpGuideView());
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        private void ChangeVideo()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;
                nav.PopAsync();
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        private void PerformThumbnail1_Tapped()
        {

            Helper.videoCampaign.SelectedThumb = thumbnail1;
            Helper.videoCampaign.Thumbname = "Thumbnail1.png";
            Thumbnail1Border = "#AC47ED";
            Thumbnail2Border = "#0000ffff";
            Thumbnail3Border = "#0000ffff";
        }

        private void PerformThumbnail2_Tapped()
        {
            Helper.videoCampaign.SelectedThumb = thumbnail2;
            Helper.videoCampaign.Thumbname = "Thumbnail2.png";
            Thumbnail1Border = "#0000ffff";
            Thumbnail2Border = "#AC47ED";
            Thumbnail3Border = "#0000ffff";
        }

        private void PerformThumbnail3_Tapped()
        {
            Helper.videoCampaign.SelectedThumb = thumbnail3;
            Helper.videoCampaign.Thumbname = "Thumbnail3.png";
            Thumbnail1Border = "#0000ffff";
            Thumbnail2Border = "#0000ffff";
            Thumbnail3Border = "#AC47ED";
        }

        //this method will be used to select bydefault thumbnail

        public void SelectDefaultThumb()
        {
                PerformThumbnail1_Tapped();
        }

        #endregion

    }
}
