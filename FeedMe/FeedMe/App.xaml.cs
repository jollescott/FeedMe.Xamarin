using FeedMe.Interfaces;
using FeedMe.Pages.MasterDetail;
using System;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FeedMe
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            /*MainPage = new NavigationPage( new MainPage() { Title ="MainPage" })
            {
                BarBackgroundColor = Constants.navigationBarColor,
                BarTextColor = Constants.textColor3
            };*/

            //MainPage = new MainPage();

            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.MaterialModule());

            //MainPage = new LoadingPage();
            MainPage = new FDMasterDetailPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
