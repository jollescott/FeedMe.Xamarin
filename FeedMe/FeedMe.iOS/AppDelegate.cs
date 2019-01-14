using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;	
using Facebook.CoreKit;
using Facebook.LoginKit;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace FeedMe.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Plugin.Iconize.Iconize.Init();

#if DEBUG
            Google.MobileAds.MobileAds.Configure("ca-app-pub-3940256099942544~3347511713");
#else
            Google.MobileAds.MobileAds.Configure("ca-app-pub-4571482486671250~7532275431");
#endif

            Profile.EnableUpdatesOnAccessTokenChange(true);
            Settings.AppId = "605355546285789";
            Settings.DisplayName = "FeedMe";

            AppCenter.Start("3b7d6ef2-eee4-46d3-a897-b7876624251b", typeof(Analytics), typeof(Crashes));

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());


            return base.FinishedLaunching(app, options);
        }
    }
}
