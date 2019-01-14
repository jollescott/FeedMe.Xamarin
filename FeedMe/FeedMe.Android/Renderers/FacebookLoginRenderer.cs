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
using FeedMe.Controls;
using FeedMe.Droid.Renderers;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginRenderer))]
namespace FeedMe.Droid.Renderers
{
    public class FacebookLoginRenderer : ViewRenderer<FacebookLoginButton, LoginButton>
    {
        public FacebookLoginRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<FacebookLoginButton> e)
        {
            base.OnElementChanged(e);
            if(Control == null)
            {
                SetNativeControl(new LoginButton(Context));
            }
        }
    }
}