using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FeedMe.Droid
{
    [Application(Label = "FeedMe", Icon = "@mipmap/ic_launcher")]
    [MetaData("com.google.android.gms.ads.APPLICATION_ID", Value = "@string/google_ads_id")]
    class FeedMeApplication : Application
    {
        protected FeedMeApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
    }
}