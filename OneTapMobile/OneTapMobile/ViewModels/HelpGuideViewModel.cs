using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OneTapMobile.ViewModels
{
    public class HelpGuideViewModel : BaseViewModel
    {
        #region properties

        public INavigation nav;

        #endregion

        #region commands
        private Command _SendContactUsEmail;

        public ICommand SendContactUsEmail
        {
            get
            {
                if (_SendContactUsEmail == null)
                {
                    _SendContactUsEmail = new Command(SendContactUsEmailCommand);
                }

                return _SendContactUsEmail;
            }
        }

        private Command backCommand;

        public ICommand BackCommand
        {
            get
            {
                if (backCommand == null)
                {
                    backCommand = new Command(BackCommandMethod);
                }

                return backCommand;
            }
        }
        #endregion

        #region methods
        private async void BackCommandMethod(object obj)
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

              await nav.PopAsync();
            }
            catch (Exception ex)
            {
                IsTap = false;
                Debug.WriteLine(ex.Message);
            }
        }

        private void SendContactUsEmailCommand()
        {
            try
            {
                if (IsTap)
                    return;
                IsTap = true;

                List<string> Recipients = new List<string>();

                Recipients.Add("info@onetapsocial.com");

                var message = new EmailMessage
                {
                    To = Recipients,
                };

                Email.ComposeAsync(message);
            }

            catch(FeatureNotSupportedException fnsEx)
            {
                IsTap = false;
                Clipboard.SetTextAsync("info@onetapsocial.com");
                Debug.WriteLine(fnsEx.Message);
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
