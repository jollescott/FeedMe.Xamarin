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

[assembly: ExportRenderer(typeof(AdmobView), typeof(AdmobViewRenderer))]
namespace FeedMe.Droid.Renderers
{
    public class AdmobViewRenderer : ViewRenderer<AdmobView, AdView>
    {
        public AdmobViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<AdmobView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                SetNativeControl(CreateAdView());

            if (e.NewElement != null)
                Control.AdListener = new FeedMeAdListener();

            if (e.OldElement != null)
                Control.AdListener = null;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(AdmobView.AdUnit))
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

    internal class FeedMeAdListener : AdListener
    {
        public override void OnAdFailedToLoad(int errorCode)
        {
            base.OnAdFailedToLoad(errorCode);
            Crashes.TrackError(new Exception("Failed to load ad: " + errorCode.ToString()));
        }
    }
}