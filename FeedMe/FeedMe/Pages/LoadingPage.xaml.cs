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
		}

        void XamlSetup()
        {
            //BackgroundImage = "background_loading.png";
            ff_Background.Source = "background_loading.png";
        }


        protected override void OnAppearing()
        {
            TestConnection();
        }


        async void TestConnection()
        {
            bool repet = true;
            do
            {
                ActivityIndicatior_WaitingForServer.IsRunning = true;
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
                        ActivityIndicatior_WaitingForServer.IsRunning = false;
                        await DisplayAlert("Can't connect to server", "Status code " + (int)response.StatusCode + ": " + response.StatusCode.ToString(), "try again");
                        //await DisplayAlert("Error", "", "reload");
                    }
                }
                catch(Exception)
                {
                    ActivityIndicatior_WaitingForServer.IsRunning = false;
                    await DisplayAlert("Can't connect to server", "Server conection failed", "try again");
                }

            } while (repet);
        }
    }
}