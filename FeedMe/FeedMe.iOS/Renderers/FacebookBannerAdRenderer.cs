using System;
using Facebook.AudienceNetwork;
using FeedMe.iOS.Renderers;
using Microsoft.AppCenter.Crashes;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FeedMe.Controls.BannerAdView), typeof(FacebookBannerAdRenderer))]
namespace FeedMe.iOS.Renderers
{
    public class FacebookBannerAdRenderer : UIViewController
    {
        public FacebookBannerAdRenderer(Context context) : base(context) { }

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

        protected override void OnElementChanged(ElementChangedEventArgs<Controls.BannerAdView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                SetNativeControl(CreateAdView());

            if (e.OldElement != null)
                Control.Destroy();
        }

        private AdView CreateAdView()
        {
# if DEBUG
            string placementId = "IMG_16_9_LINK#2068149499897372_2138868712825450";
#else
            string placementId = "2068149499897372_2138868712825450";
# endif

            //var adView = new AdView(Context, placementId, AdSize.BannerHeight50);

            var adView = new AdView(placementId, AdSizes.BannerHeight50, this);
            adView.Frame = new CoreGraphics.CGRect(0, 0, 320, 50);

            adView.LoadAd();

            return adView;
        }
    }
}
