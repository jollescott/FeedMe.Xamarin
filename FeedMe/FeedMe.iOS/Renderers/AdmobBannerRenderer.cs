using System;
using FeedMe.Controls;
using FeedMe.iOS.Renderers;
using Google.MobileAds;
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
            _adUnit = "";
#endif
            _adUnit = "";
#else
        }

    protected override void OnElementChanged(ElementChangedEventArgs<BannerAdView> e)
        {
            if (Control == null)
            {
                CreateBannerView();
                SetNativeControl(_adView);
            }
        }

        private BannerView CreateBannerView()
        {
            _adView = new BannerView(size: AdSizeCons.SmartBannerLandscape);

            
            return _adView;
        }
    }
}