
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace FeedMe.Droid
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "FeedMe", Icon = "@mipmap/ic_launcher", Theme = "@style/SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(new Intent(this, typeof(MainActivity)));
        }
    }
}