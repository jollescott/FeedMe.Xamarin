﻿using FeedMe.Pages.MasterDetail;
using System;
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

            //MainPage = new FDMasterDetailPage();

            //MainPage = new MainPage();

            MainPage = new LoadingPage();
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
