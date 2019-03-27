﻿using System;
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

[assembly: ExportRenderer(typeof(BannerAdView), typeof(AdmobBanerViewRenderer))]
namespace FeedMe.Droid.Renderers
{
    public class AdmobBanerViewRenderer : ViewRenderer<BannerAdView, AdView>
    {
        public AdmobBanerViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<BannerAdView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                SetNativeControl(CreateAdView());

            if (e.NewElement != null)
                Control.AdListener = new FeedMeBanerAdListener();

            if (e.OldElement != null)
                Control.AdListener = null;
        }

        private AdView CreateAdView()
        {
#if DEBUG
            var adUnit = "ca-app-pub-3940256099942544/6300978111";  // not real ads
#else
            var adUnit = "ca-app-pub-4571482486671250/2065611163";
#endif

            var adView = new AdView(Context)
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

    internal class FeedMeBanerAdListener : AdListener
    {
        public override void OnAdFailedToLoad(int errorCode)
        {
            base.OnAdFailedToLoad(errorCode);
            Crashes.TrackError(new Exception("Failed to load ad: " + errorCode.ToString()));
        }
    }
}