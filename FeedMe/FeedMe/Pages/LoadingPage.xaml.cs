using FeedMe.Pages.MasterDetail;
using Ramsey.Shared.Misc;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        private readonly HttpClient httpClient = new HttpClient();

        public LoadingPage()
        {
            InitializeComponent();
            XamlSetup();
        }

        private void XamlSetup()
        {
            //BackgroundImage = "background_loading.png";
            //ff_Background.Source = "background_loading.png"; // image does not exist
        }


        protected override void OnAppearing()
        {
            TestConnection();
        }

        private async void TestConnection()
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
                catch (Exception)
                {
                    ActivityIndicatior_WaitingForServer.IsRunning = false;
                    await DisplayAlert("Can't connect to server", "Server conection failed", "try again");
                }

            } while (repet);
        }
    }
}