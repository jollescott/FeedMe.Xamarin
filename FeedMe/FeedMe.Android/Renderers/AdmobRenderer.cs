using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FeedMe.Controls;
using FeedMe.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdmobView), typeof(AdmobViewRenderer))]
namespace FeedMe.Droid.Renderers
{
    public class AdmobViewRenderer : ViewRenderer<AdmobView, AdView>
    {
        public AdmobViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<AdmobView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control == null)
                SetNativeControl(CreateAdView());
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(AdView.AdUnitId))
                Control.AdUnitId = Element.AdUnit;
        }

        private AdView CreateAdView()
        {
            var adView = new AdView(Context)
            {
                AdSize = AdSize.LargeBanner,
                AdUnitId = Element.AdUnit
            };

            adView.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

#if DEBUG
            adView.LoadAd(new AdRequest.Builder()
                .AddTestDevice("0DF7A98B3CDD737BC14D8BFE75FB5362")
                .Build());
#else
            adView.LoadAd(new AdRequest.Builder()
                .Build());
#endif

            return adView;
        }
    }
}