using System;
using Facebook.AudienceNetwork;
using FeedMe.Controls;
using FeedMe.iOS.Renderers;
using Foundation;
using Microsoft.AppCenter.Crashes;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FeedMe.Controls.BannerAdView), typeof(FacebookBannerAdRenderer))]
namespace FeedMe.iOS.Renderers
{
    public class FacebookBannerAdRenderer : ViewRenderer<BannerAdView, Facebook.AudienceNetwork.AdView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Controls.BannerAdView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                SetNativeControl(CreateAdView());
        }

        private AdView CreateAdView()
        {
# if DEBUG
            string placementId = "IMG_16_9_LINK#2068149499897372_2138866369492351";
#else
            string placementId = "206814949867372_2138866369492351";
#endif

            var adView = new AdView(placementId, AdSizes.BannerHeight50, GetVisibleViewController());
            adView.Frame = new CoreGraphics.CGRect(0, 0, 320, 50);
            adView.Delegate = new FeedMeAdDelegate();

            adView.LoadAd();

            return adView;
        }

        private UIViewController GetVisibleViewController()
        {
            var windows = UIApplication.SharedApplication.Windows;
            foreach (var window in windows)
            {
                if (window.RootViewController != null)
                {
                    return window.RootViewController;
                }
            }

            return null;
        }
    }

    public class FeedMeAdDelegate : AdViewDelegate
    {
        public override void AdViewDidFail(AdView adView, NSError error)
        {
        }

        public override void AdViewDidLoad(AdView adView)
        {
        }
    }
}
