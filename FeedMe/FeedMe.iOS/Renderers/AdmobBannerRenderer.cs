using FeedMe.iOS.Renderers;
using Google.MobileAds;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Frame), typeof(AdmobBannerRenderer))]
namespace FeedMe.iOS.Renderers
{
    public class AdmobBannerRenderer : ViewRenderer<BannerAdView, BannerView>
    {
        private BannerView _adView;
        private string _adUnit;

        public AdmobBannerRenderer()
        {
#if DEBUG
            _adUnit = "ca-app-pub-3940256099942544/2934735716";
#else
            _adUnit = "ca-app-pub-4571482486671250/4328985802";
#endif
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BannerAdView> e)
        {
            if (Control == null)
            {
                CreateBannerView();
                SetNativeControl(_adView);
            }
        }

        private void CreateBannerView()
        {
            _adView = new BannerView(size: AdSizeCons.SmartBannerLandscape) {
                RootViewController = GetVisibleViewController()
            };

            _adView.LoadRequest(GetAdRequest());
        }

        private Request GetAdRequest()
        {
            var request = Request.GetDefaultRequest();
            request.TestDevices = new[] { Request.SimulatorId.ToString() };

            return request;
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
}