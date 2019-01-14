using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook.LoginKit;
using FeedMe.Controls;
using FeedMe.iOS.Renderers;
using Foundation;
using Microsoft.AppCenter.Crashes;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginRenderer))]
namespace FeedMe.iOS.Renderers
{
    public class FacebookLoginRenderer : ViewRenderer<FacebookLoginButton, LoginButton>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<FacebookLoginButton> e)
        {
            base.OnElementChanged(e);

            if(Control == null)
                SetNativeControl(new LoginButton());

            if(e.NewElement != null)
                Control.Completed += Control_Completed;

            if (e.OldElement != null)
                Control.Completed -= Control_Completed;
        }

        private void Control_Completed(object sender, LoginButtonCompletedEventArgs e)
        {
            if(e.Result.Token != null)
            {
                MessagingCenter.Instance.Send(Xamarin.Forms.Application.Current, "FacebookLogin_Success", e.Result.Token.UserId);
            }
            else
            {
                MessagingCenter.Instance.Send(Xamarin.Forms.Application.Current, "FacebookLogin_Cancelled");
            }
        }
    }
}