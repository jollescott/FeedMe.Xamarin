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
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FeedMe.Controls.AdView), typeof(AdmobBannerViewRenderer))]
namespace FeedMe.Droid.Renderers
{
    public class AdmobBannerViewRenderer : ViewRenderer<Controls.AdView, Android.Gms.Ads.AdView>
    {
        public AdmobBannerViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Controls.AdView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                SetNativeControl(CreateAdView());

            if (e.NewElement != null)
                Control.AdListener = new FeedMeBannerAdListener();

            if (e.OldElement != null)
                Control.AdListener = null;
        }

        private Android.Gms.Ads.AdView CreateAdView()
        {
#if DEBUG
            var adUnit = "ca-app-pub-3940256099942544/6300978111";  // not real ads
#else
            var adUnit = "ca-app-pub-4571482486671250/2065611163";
#endif

            var adView = new Android.Gms.Ads.AdView(Context)
            {
                AdSize = AdSize.LargeBanner,
                AdUnitId = adUnit
            };

            adView.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

            adView.LoadAd(new AdRequest.Builder()
                 .AddTestDevice("0DF7A98B3CDD737BC14D8BFE75FB5362")
                 .Build());

            return adView;
        }
    }

    internal class FeedMeBannerAdListener : AdListener
    {
        public override void OnAdFailedToLoad(int errorCode)
        {
            base.OnAdFailedToLoad(errorCode);
            Crashes.TrackError(new Exception("Failed to load ad: " + errorCode.ToString()));
        }
    }
}