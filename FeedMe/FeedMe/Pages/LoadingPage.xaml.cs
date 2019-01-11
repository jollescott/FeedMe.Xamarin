using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using FeedMe.Pages.MasterDetail;
using Ramsey.Shared.Misc;

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

            BackgroundColor = Color.White;
		}

        void XamlSetup()
        {
            BackgroundImage = "background_loading.png";

            //Image_AppLogo.HeightRequest = Image_AppLogo.Width;
            //Image_AppLogo.Source = "logo_app.png";

            //Image_CompanyLogo.HeightRequest = Image_CompanyLogo.Width;
            //Image_CompanyLogo.Source = "logo_company.png";
            //Image_CompanyLogo.Margin = Constants.padding1;
        }


        protected override void OnAppearing()
        {
            Test_connection();

            //Application.Current.MainPage = new NavigationPage(new MainPage());
            //Application.Current.MainPage = new NavigationPage(new MainPage() { Title = "" }) {BarBackgroundColor = Constants.navigationBarColor, BarTextColor = Constants.textColor1 };
        }


        async void Test_connection()
        {
            bool repet = true;
            do
            {
                try
                {
                    string str = RamseyApi.V2.Ingredient.Suggest;
                    HttpResponseMessage response = await httpClient.GetAsync(RamseyApi.V2.Ingredient.Suggest);

                    if (response.IsSuccessStatusCode)
                    {
                        repet = false;

                        Application.Current.MainPage = new FDMasterDetailPage();
                    }
                    else
                    {
                        await DisplayAlert("Can't connect to server", "Status code " + (int)response.StatusCode + ": " + response.StatusCode.ToString(), "try again");
                        //await DisplayAlert("Error", "", "reload");
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