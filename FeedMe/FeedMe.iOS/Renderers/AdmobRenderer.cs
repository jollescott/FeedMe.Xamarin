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

[assembly: ExportRenderer(typeof(AdView), typeof(AdmobRenderer))]
namespace FeedMe.iOS.Renderers
{
    public class AdmobRenderer : ViewRenderer<AdView, Google.MobileAds.BannerView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<AdView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                SetNativeControl(CreateAdView());
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        private BannerView CreateAdView()
        {
#if DEBUG
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "ca-app-pub-4571482486671250/4328985802";
#endif

            var adView = new BannerView
            {
                AdSize = AdSizeCons.LargeBanner,
                AdUnitID = adUnitId,
                RootViewController = GetVisibleViewController()
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