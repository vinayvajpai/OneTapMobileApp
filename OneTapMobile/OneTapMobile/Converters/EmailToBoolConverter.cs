using OneTapMobile.Global;
using System;
using System.Globalization;
using Xamarin.Forms;
namespace OneTapMobile.Converters
{
    public class EmailToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string)value))
                return true;

            else if (Helper.ValidateEmail((string)value)) // length > 0 ?
                return true;            // some data has been entered
            
            else
                return false;            // input is empty
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
