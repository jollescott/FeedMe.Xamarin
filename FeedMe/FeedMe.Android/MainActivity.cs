
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace FeedMe.Droid
{
    //"@mipmap/icon"
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "FeedMe", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // FaceBook ads stuff
            //AdSettings.AddTestDevice("4bc78db7-bd07-4bc2-9d11-1d1b19a5a415");
            //AdSettings.AddTestDevice("c3d7b4ba-e06e-465b-9db3-1008974c7ebb");

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);
            Plugin.Iconize.Iconize.Init(Resource.Id.toolbar, Resource.Id.sliding_tabs);

            AppCenter.Start("eedec111-2462-45bc-9c49-3320f6a175a3", typeof(Analytics), typeof(Crashes));

            Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }
    }
}