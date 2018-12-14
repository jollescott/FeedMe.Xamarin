using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Net.Http;
using FeedMe.Pages.MasterDetail;


namespace FeedMe
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingPage : ContentPage
	{

        HttpClient httpClient = new HttpClient();

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


        protected override void OnAppearing()
        {
            test_connection(Constants.ingredient_search);

            //Application.Current.MainPage = new NavigationPage(new MainPage());
            //Application.Current.MainPage = new NavigationPage(new MainPage() { Title = "" }) {BarBackgroundColor = Constants.navigationBarColor, BarTextColor = Constants.textColor1 };
        }


        async void test_connection(string _adress)
        {
            bool repet = true;
            do
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(_adress);

                    if (response.IsSuccessStatusCode)
                    {
                        repet = false;

                        //await DisplayAlert("success", "succeess", "ok");
                        //Application.Current.MainPage = new NavigationPage(new MainPage() { Title = "" }) { BarBackgroundColor = Constants.navigationBarColor, BarTextColor = Constants.textColor1 };
                        Application.Current.MainPage = new FDMasterDetailPage();
                    }
                    else
                    {
                        await DisplayAlert("Can't connect to server", "Status code " + (int)response.StatusCode + ": " + response.StatusCode.ToString(), "try again");
                    }
                }
                catch(Exception)
                {
                    await DisplayAlert("Can't connect to server", "Server conection failed", "try again");
                }

            } while (repet);
        }
    }
}