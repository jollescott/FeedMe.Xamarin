using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FeedMe.Controls;
using FeedMe.iOS.Renderers;
using Foundation;
using Google.MobileAds;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AdmobView), typeof(AdmobRenderer))]
namespace FeedMe.iOS.Renderers
{
    public class AdmobRenderer : ViewRenderer<AdmobView, Google.MobileAds.BannerView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<AdmobView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                SetNativeControl(CreateAdView());
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(AdmobView.AdUnit))
                Control.AdUnitID = Element.AdUnit;
        }

        private BannerView CreateAdView()
        {
            var adView = new BannerView
            {
                AdSize = AdSizeCons.LargeBanner,
                AdUnitID = Element.AdUnit,
                RootViewController = ViewController
            };
            
            adView.LoadRequest(GetAdRequest());
                        
            return adView;
        }

        private Request GetAdRequest()
        {
            var request = Request.GetDefaultRequest();
            request.TestDevices = new[] {Request.SimulatorId.ToString()};

            return request;
        }
    }
}