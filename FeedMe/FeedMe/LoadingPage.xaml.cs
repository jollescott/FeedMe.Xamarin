using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingPage : ContentPage
	{
		public LoadingPage ()
		{
			InitializeComponent ();
            XamlSetup();
		}

        void XamlSetup()
        {
            BackgroundImage = "background.jpg";

            Image_AppLogo.HeightRequest = Image_AppLogo.Width;
            Image_AppLogo.Source = "FeedMe_icon.png";

            Image_CompanyLogo.HeightRequest = Image_CompanyLogo.Width;
            Image_CompanyLogo.Source = "ananasSoftware.png";
            Image_CompanyLogo.Margin = Constants.padding1;
        }


        protected async override void OnAppearing()
        {
            await Task.Delay(5000);

            //Application.Current.MainPage = new NavigationPage(new MainPage());
            Application.Current.MainPage = new NavigationPage(new MainPage() { Title = "" }) {BarBackgroundColor = Constants.navigationBarColor, BarTextColor = Constants.textColor1 };
        }
    }
}