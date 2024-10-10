﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace OneTapMobile.CustomControl
{
    public class GradientLabel : Label
    {
        public static readonly BindableProperty TextColor1Property = BindableProperty.Create(
        propertyName: nameof(TextColor1),
        returnType: typeof(Color),
        declaringType: typeof(Color),
        defaultValue: Color.FromHex("#7729B6"));

        public Color TextColor1
        {
            get => (Color)GetValue(TextColor1Property);
            set => SetValue(TextColor1Property, value);
        }
        
        public static readonly BindableProperty TextColor2Property = BindableProperty.Create(
            propertyName: nameof(TextColor2),
            returnType: typeof(Color),
            declaringType: typeof(Color),
            defaultValue: Color.FromHex("#A13AD5"));
        
        public Color TextColor2
        {
            get => (Color)GetValue(TextColor2Property);
            set => SetValue(TextColor2Property, value);
        }
    }
}
