using System;
using System.Runtime.Remoting.Contexts;
using Facebook.AudienceNetwork;
using FeedMe.iOS.Renderers;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FeedMe.Controls.NativeAdView), typeof(FacebookNativeAdRenderer))]
namespace FeedMe.iOS.Renderers
{
    public class FacebookNativeAdRenderer : ViewRenderer<Controls.NativeAdView, LinearLayout>, NativeAdsManager.IListener
    {
        private NativeAdsManager _manager;
        private NativeAdScrollView _scrollView;

        public void OnAdsLoaded()
        {
            if (_scrollView != null)
                Control.RemoveView(_scrollView);

            try
            {
                _scrollView = new NativeAdScrollView(_manager, NativeAdViewType.GenericHeight300);
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
                _manager = new NativeAdsManager("YOUR_PLACEMENT_ID", 1/* Hur många ads som ska laddas.*/);
                _manager.SetListener(this);
                SetNativeControl(new LinearLayout(Context));
            }

            if (e.NewElement != null)
            {
                _manager.LoadAds();
            }

            if (e.OldElement != null)
                _scrollView?.Dispose();
        }
    }
}