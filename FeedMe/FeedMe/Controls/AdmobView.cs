using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FeedMe.Controls
{
    public class AdmobView : View
    {
        public static readonly BindableProperty AdUnitProperty =
            BindableProperty.Create(nameof(AdUnit), typeof(string), typeof(AdmobView), string.Empty);

        public string AdUnit
        {
            get => (string)GetValue(AdUnitProperty);
            set => SetValue(AdUnitProperty, value);
        }
    }
}
