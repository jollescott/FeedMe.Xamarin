﻿using System;
using Android.Content;
using FeedMe.Droid.Renderers;
using Microsoft.AppCenter.Crashes;
using Xamarin.Facebook.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(FeedMe.Controls.AdView), typeof(FaceBookAdViewRenderer))]
namespace FeedMe.Droid.Renderers
{
    public class FaceBookAdViewRenderer : ViewRenderer<Controls.AdView, Xamarin.Facebook.Ads.AdView>, IAdListener
    {
        public FaceBookAdViewRenderer(Context context) : base(context) { }

        public void OnAdClicked(IAd p0)
        {
        }

        public void OnAdLoaded(IAd p0)
        {
        }

        public void OnError(IAd p0, AdError p1)
        {
            Crashes.TrackError(new Exception("Failed to load ad. Error code: " + p1.ErrorCode));
        }

        public void OnLoggingImpression(IAd p0)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Controls.AdView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                SetNativeControl(CreateAdView());

            if (e.NewElement != null)
            {
                Control.SetAdListener(this);
                //Control.LoadAd();
            }

            if (e.OldElement != null)
                Control.Destroy();
        }

        private Xamarin.Facebook.Ads.AdView CreateAdView()
        {
# if DEBUG
            string placementId = "IMG_16_9_LINK#2068149499897372_2138868712825450";
#else
            string placementId = "2068149499897372_2138868712825450";
# endif

            var adView = new Xamarin.Facebook.Ads.AdView(Context, placementId, AdSize.BannerHeight90);

            AdSettings.AddTestDevice("c3d7b4ba-e06e-465b-9db3-1008974c7ebb");

            adView.LoadAd();

            return adView;
        }
    }
}