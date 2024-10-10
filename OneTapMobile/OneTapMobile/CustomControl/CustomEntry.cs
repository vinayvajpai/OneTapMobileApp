using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace OneTapMobile.CustomControl
{
    public class CustomEntry : Entry
    {
        public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor),
            typeof(Color), typeof(CustomEntry), Color.Gray);
        // Gets or sets BorderColor value  
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create(nameof(BorderWidth), typeof(int),
            typeof(CustomEntry), Device.OnPlatform<int>(0, 0, 0));
        // Gets or sets BorderWidth value  
        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }
        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius),
            typeof(double), typeof(CustomEntry), Device.OnPlatform<double>(6, 7, 7));
        // Gets or sets CornerRadius value  
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
        BindableProperty.Create(nameof(IsCurvedCornersEnabled),
            typeof(bool), typeof(CustomEntry), true);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool IsCurvedCornersEnabled
        {
            get => (bool)GetValue(IsCurvedCornersEnabledProperty);
            set => SetValue(IsCurvedCornersEnabledProperty, value);
        }

        public static readonly BindableProperty ShowSingleLineProperty =
        BindableProperty.Create(nameof(ShowSingleLine),
            typeof(bool), typeof(CustomEntry), false);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool ShowSingleLine
        {
            get => (bool)GetValue(ShowSingleLineProperty);
            set => SetValue(ShowSingleLineProperty, value);
        }

        public static readonly BindableProperty NoLineProperty =
       BindableProperty.Create(nameof(NoLine),
           typeof(bool), typeof(CustomEntry), false);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool NoLine
        {
            get => (bool)GetValue(NoLineProperty);
            set => SetValue(NoLineProperty, value);
        }

        public static readonly BindableProperty IsIconProperty =
      BindableProperty.Create(nameof(IsIcon),
          typeof(bool), typeof(CustomEntry), false);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool IsIcon
        {
            get => (bool)GetValue(IsIconProperty);
            set => SetValue(IsIconProperty, value);
        }

        public static readonly BindableProperty ShowSpecialCharProperty =
      BindableProperty.Create(nameof(ShowSpecialChar),
          typeof(bool), typeof(CustomEntry), false);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool ShowSpecialChar
        {
            get => (bool)GetValue(ShowSpecialCharProperty);
            set => SetValue(ShowSpecialCharProperty, value);
        }

        public static readonly BindableProperty PlaceholderTextColorProperty =
     BindableProperty.Create(nameof(ShowSpecialChar),
         typeof(Color), typeof(CustomEntry), Color.LightGray, propertyChanged: PlaceholderTextColorPropertyChanged);
        // Gets or sets IsCurvedCornersEnabled value  
        public Color PlaceholderTextColor
        {
            get => (Color)GetValue(PlaceholderTextColorProperty);
            set => SetValue(PlaceholderTextColorProperty, value);
        }

        private static void PlaceholderTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }
    }
}
