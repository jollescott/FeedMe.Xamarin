using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Droid;

namespace FeedMe.Droid
{
    //"@mipmap/icon"
    [Activity(Label = "FeedMe", Icon = "@drawable/logo_app", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
#if DEBUG
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, "ca-app-pub-3940256099942544~3347511713");
            //Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, "ca-app-pub-4571482486671250~7532275431");

#else
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, "ca-app-pub-4571482486671250~7532275431");
#endif
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }
    }
}