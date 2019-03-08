using System;
using Facebook.AudienceNetwork;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace FeedMe.iOS.Renderers
{
    public partial class FacebookBannerAdViewController : UIViewController
    {
        public FacebookBannerAdViewController() : base("FacebookBannerAdViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.

            CreateAdView();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }


        private AdView CreateAdView()
        {
# if DEBUG
            string placementId = "IMG_16_9_LINK#2068149499897372_2138868712825450";
# else
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

