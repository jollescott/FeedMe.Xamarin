using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using FeedMe.Controls;
using FeedMe.Droid.Renderers;
using Microsoft.AppCenter.Crashes;
using Xamarin.Facebook.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Xamarin.Facebook.Ads.NativeAdScrollView;

[assembly: ExportRenderer(typeof(FeedMe.Controls.NativeAdView), typeof(FacebookNativeAdRenderer))]
namespace FeedMe.Droid.Renderers
{
    public class FacebookNativeAdRenderer : ViewRenderer<Controls.NativeAdView, LinearLayout>, NativeAdsManager.IListener
    {
        private NativeAdsManager _manager;
        private NativeAdScrollView _scrollView;

        public FacebookNativeAdRenderer(Context context) : base(context) { }

#pragma warning disable CS0618 // Type or member is obsolete
        public FacebookNativeAdRenderer(System.IntPtr ptr, Android.Runtime.JniHandleOwnership owner)
        {
        }
#pragma warning restore CS0618 // Type or member is obsolete

        public void OnAdError(AdError p0)
        {
        }

        public void OnAdsLoaded()
        {
            if (_scrollView != null)
                Control.RemoveView(_scrollView);

            try
            {
                _scrollView = new NativeAdScrollView(Context, _manager, Xamarin.Facebook.Ads.NativeAdView.Type.Height300);
                Control?.AddView(_scrollView);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(new Exception("Load native ad error. Exception message: " + ex.Message));
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Controls.NativeAdView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
#if DEBUG
                string placementId = "IMG_16_9_LINK#2068149499897372_2140286706016984";
#else
                string placementId = "2068149499897372_2140286706016984";
                #endif

                _manager = new NativeAdsManager(Context, placementId, 1/* Hur många ads som ska laddas.*/);
                _manager.SetListener(this);
                SetNativeControl(new LinearLayout(Context));
            }

            if(e.NewElement != null)
            {
                _manager.LoadAds();
            }

            if (e.OldElement != null)
                _scrollView?.Dispose();
        }
    }
}