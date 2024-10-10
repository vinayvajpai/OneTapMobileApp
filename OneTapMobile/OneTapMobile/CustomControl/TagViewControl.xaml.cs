using OneTapMobile.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace OneTapMobile.CustomControl
{
    public partial class TagViewControl : ContentView
    {
        public event EventHandler<string> deleteTagEvent;
        public TagViewControl()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TagIndexProperty = BindableProperty.Create(nameof(TagIndex), typeof(int), typeof(TagViewControl), 0);

        //Gets or sets IsCurvedCornersEnabled value  
        public int TagIndex
        {
            get => (int)GetValue(TagIndexProperty);
            set => SetValue(TagIndexProperty, value);
        }

        public static readonly BindableProperty TagTitleProperty = BindableProperty.Create(nameof(TagIndex), typeof(string), typeof(TagViewControl), string.Empty, propertyChanged: TagTitlePropertyChanged);

        //Gets or sets IsCurvedCornersEnabled value  
        public string TagTitle
        {
            get => (string)GetValue(TagTitleProperty);
            set => SetValue(TagTitleProperty, value);
        }

        public static readonly BindableProperty TagKeyProperty = BindableProperty.Create(nameof(TagIndex), typeof(string), typeof(TagViewControl), string.Empty, propertyChanged: TagKeyPropertyChanged);

        //Gets or sets IsCurvedCornersEnabled value  
        public string Tagkey
        {
            get => (string)GetValue(TagKeyProperty);
            set => SetValue(TagKeyProperty, value);
        }

        private static void TagTitlePropertyChanged(BindableObject bindable, object oldVal, object newVal)
        {
            var tagview = bindable as TagViewControl;
            tagview.lblTagTitle.Text = Convert.ToString(newVal);
        }

        private static void TagKeyPropertyChanged(BindableObject bindable, object oldVal, object newVal)
        {
            var tagview = bindable as TagViewControl;
            tagview.TagKey.Text = Convert.ToString(newVal);
        }

        void RemoveTag_Tapped(System.Object sender, System.EventArgs e)
        {
            //if (deleteTagEvent != null)
            //{
            //    deleteTagEvent.Invoke(sender, TagTitle);
            //    ((FlexLayout)(this.Parent)).Children.Remove(this);
            //}

            if (!string.IsNullOrWhiteSpace(TagTitle))
            {
                //deleteTagEvent.Invoke(sender, TagTitle);
                ((FlexLayout)(this.Parent)).Children.Remove(this);

            }
        }
    }
}
