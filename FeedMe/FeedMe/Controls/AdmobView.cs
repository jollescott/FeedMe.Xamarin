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

        public AdmobView()
        {
#if DEBUG
            AdUnit = "ca-app-pub-3940256099942544/6300978111";  // not real ads
            //AdUnit = "ca-app-pub-4571482486671250/2065611163";  // real ads
#else
            AdUnit = "ca-app-pub-4571482486671250/2065611163";
#endif
        }
    }
}
