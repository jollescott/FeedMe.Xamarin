using System;
using System.Runtime.Remoting.Contexts;
using Facebook.AudienceNetwork;
using FeedMe.iOS.Renderers;
using Foundation;
using Microsoft.AppCenter.Crashes;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FeedMe.Controls.NativeAdView), typeof(FacebookNativeAdRenderer))]
namespace FeedMe.iOS.Renderers
{
    public class FacebookNativeAdRenderer : ViewRenderer<Controls.NativeAdView, UIStackView>
    {
        private NativeAdsManager _manager;
        private NativeAdScrollView _scrollView;

        public void OnAdsLoaded()
        {
            if (_scrollView != null)
                Control.RemoveArrangedSubview(_scrollView);

            try
            {
                _scrollView = new NativeAdScrollView(_manager, NativeAdViewType.GenericHeight300);
                Control?.AddArrangedSubview(_scrollView);
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
                string placementId = "IMG_16_9_LINK#2068149499897372_2149242801788041";
#else
                string placementId = "2068149499897372_2149242801788041";
#endif

                _manager = new NativeAdsManager(placementId, 1/* Hur många ads som ska laddas.*/);
                _manager.Delegate = new FeedMeNativeDelegate
                {
                    AdLoaded = OnAdsLoaded
                };
                SetNativeControl(new UIStackView());
            }

            if (e.NewElement != null)
            {
                _manager.LoadAds();
            }

            if (e.OldElement != null)
                _scrollView?.Dispose();
        }
    }

    internal class FeedMeNativeDelegate : NativeAdsManagerDelegate
    {
        public Action AdLoaded { get; set; }

        public override void NativeAdsFailedToLoad(NSError error)
        {
        }

        public override void NativeAdsLoaded()
        {
            AdLoaded?.Invoke();
        }
    }
}