using FeedMe.Pages.MasterDetail;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Plugin.Iconize.Fonts;

namespace FeedMe
{
    public partial class FormsApp : Application
    {
        public FormsApp()
        {
            InitializeComponent();

            Plugin.Iconize.Iconize.With(new MaterialModule());
            
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
