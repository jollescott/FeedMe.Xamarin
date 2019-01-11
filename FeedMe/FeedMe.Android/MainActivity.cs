using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Droid;
using Xamarin.Facebook;
using FeedMe.Droid.Callbacks;
using Xamarin.Facebook.Login;
using Android.Content;
using Xamarin.Forms;

namespace FeedMe.Droid
{
    //"@mipmap/icon"
    [Activity(Label = "FeedMe", Icon = "@drawable/logo_app", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private ICallbackManager callbackManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
#if DEBUG
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, "ca-app-pub-3940256099942544~3347511713");  // fake ad
            //Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, "ca-app-pub-4571482486671250~7532275431");  // real ad

#else
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, "ca-app-pub-4571482486671250~7532275431");
#endif
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);

            callbackManager = CallbackManagerFactory.Create();
            LoginManager.Instance.RegisterCallback(callbackManager, new FacebookLoginCallback<LoginResult>
            {
                HandleError = error =>
                {
                    MessagingCenter.Instance.Send(Xamarin.Forms.Application.Current, "FacebookLogin_Error", error.InnerException);
                },
                HandleCancel = () =>
                {
                    MessagingCenter.Instance.Send(Xamarin.Forms.Application.Current, "FacebookLogin_Cancelled");
                },
                HandleSuccess = loginResult =>
                {
                    MessagingCenter.Instance.Send(Xamarin.Forms.Application.Current, "FacebookLogin_Success", loginResult.AccessToken);
                }
            });

            Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}