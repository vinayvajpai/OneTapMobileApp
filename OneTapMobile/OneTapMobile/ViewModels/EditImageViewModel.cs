using OneTapMobile.Global;
using OneTapMobile.Interface;
using OneTapMobile.Views.ErrorPage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Forms;
using OneTapMobile.Views;
using OneTapMobile.Views.AI;

namespace OneTapMobile.ViewModels
{
    public class EditImageViewModel : BaseViewModel
    {
        #region properties

        public INavigation nav;

        double mX = 0f;
        double mY = 0f;
        double mRatioPan = -0.0015f;
        double mRatioZoom = 0.8f;

        public string ImageUrl { get; set; } = "http://loremflickr.com/600/600/nature?filename=crop_transformation.jpg";

        private Xamarin.Forms.ImageSource _CroppedImage;
        public Xamarin.Forms.ImageSource CroppedImage
        {
            get
            {
                return _CroppedImage;
            }
            set
            {
                _CroppedImage = value;
                OnPropertyChanged("CroppedImage");
            }
        }

        //private List<ITransformation> transformations;
        //public List<ITransformation> Transformations
        //{
        //    get
        //    {
        //        return transformations;
        //    }
        //    set
        //    {
        //        transformations = value;
        //        OnPropertyChanged("Transformations");
        //    }
        //}

        //private bool _IsLoading;
        //public bool IsLoading
        //{
        //    get
        //    {
        //        return _IsLoading;
        //    }
        //    set
        //    {
        //        _IsLoading = value;
        //        OnPropertyChanged("IsLoading");
        //    }
        //}

        #endregion

        #region commands
        private Command backCommand;
        public Command BackCommand
        {
            get
            {
                if (backCommand == null)
                {
                    backCommand = new Command(PerformBackBtn);
                }

                return backCommand;
            }
        }


        private ICommand helpCommand;
        public ICommand HelpCommand
        {
            get
            {
                if (helpCommand == null)
                {
                    helpCommand = new Command(HelpMethod);
                }

                return helpCommand;
            }
        }


        //private Command viewImageCommand;
        //public ICommand ViewImageCommand
        //{
        //    get
        //    {
        //        if (viewImageCommand == null)
        //        {
        //            viewImageCommand = new Command(ViewImage);
        //        }

        //        return viewImageCommand;
        //    }
        //}

        private ICommand changeImageCommand;
        public ICommand ChangeImageCommand
        {
            get
            {
                if (changeImageCommand == null)
                {
                    changeImageCommand = new Command(ChangeImage);
                }

                return changeImageCommand;
            }
        }




        private ICommand continueBtnCommand;
        public ICommand ContinueBtnCommand
        {
            get
            {
                if (continueBtnCommand == null)
                {
                    continueBtnCommand = new Command(ContinueBtn);
                }

                return continueBtnCommand;
            }
        }

        #endregion

        #region methods
        private void HelpMethod()
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

        private void PerformBackBtn()
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


        //private void ViewImage()
        //{
        //}

        //public void Reload()
        //{
        //    CurrentZoomFactor = 1d;
        //    CurrentXOffset = 0d;
        //    CurrentYOffset = 0d;
        //}

        //void ReloadImage()
        //{
        //    Transformations = new List<ITransformation>() {
        //        new CropTransformation(CurrentZoomFactor, CurrentXOffset, CurrentYOffset, 1f, 1f)
        //    };

        //    MessagingCenter.Send<object, object>(this, "Refresh", "");
        //}

        //public double CurrentZoomFactor { get; set; }

        //public double CurrentXOffset { get; set; }

        //public double CurrentYOffset { get; set; }

        //internal void OnPanUpdated(PanUpdatedEventArgs e)
        //{
        //    if (e.StatusType == GestureStatus.Completed)
        //    {
        //        mX = CurrentXOffset;
        //        mY = CurrentYOffset;
        //    }
        //    else if (e.StatusType == GestureStatus.Running)
        //    {
        //        CurrentXOffset = (e.TotalX * mRatioPan) + mX;
        //        CurrentYOffset = (e.TotalY * mRatioPan) + mY;
        //        ReloadImage();
        //    }
        //}

        //internal void OnPinchUpdated(PinchGestureUpdatedEventArgs e)
        //{
        //    if (e.Status == GestureStatus.Completed)
        //    {
        //        mX = CurrentXOffset;
        //        mY = CurrentYOffset;
        //    }
        //    else if (e.Status == GestureStatus.Running)
        //    {
        //        CurrentZoomFactor += (e.Scale - 1) * CurrentZoomFactor * mRatioZoom;
        //        CurrentZoomFactor = Math.Max(1, CurrentZoomFactor);

        //        CurrentXOffset = (e.ScaleOrigin.X * mRatioPan) + mX;
        //        CurrentYOffset = (e.ScaleOrigin.Y * mRatioPan) + mY;
        //        ReloadImage();
        //    }
        //}
        private void ChangeImage()
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

        private async void ContinueBtn()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                if (Helper.imageCampaign.Image == null)
                    Helper.imageCampaign.Image = CroppedImage;

                //await nav.PushAsync(new AdProfileTemplateView(Helper.imageCampaign.Image));
                await nav.PushAsync(new TapAndFillView());
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }

        }
        #endregion
    }
}
