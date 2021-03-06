﻿using FeedMe.Interfaces;
using Plugin.Iconize;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FeedMe.Pages.MasterDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FDMasterDetailPage : MasterDetailPage
    {
        public FDMasterDetailPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelectedAsync;
        }

        private async void ListView_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as FDMasterDetailPageMenuItem;
            if (item == null)
                return;

            switch (item.Id)
            {
                case 0:
                    Detail = new IconNavigationPage(new MainPage()); // search page
                    break;
                case 1:
                    Detail = new NavigationPage(new MealsListPage(true) { Title = "Sök Recept" }); // search with name page
                    break;
                case 2:

                    //bool isAuth = await VerifyFacebookAsync();
                    //if (isAuth)
                    //    Detail = new IconNavigationPage(new FavoritesPage());

                    Detail = new NavigationPage(new MealsListPage() { Title = "Gillade Recept"}); // saved recipes page
                    break;

                //case 3:
                //    Detail = new NavigationPage(new shoppingListPage()); // shopping list page
                //    break;
            }
            /*
            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            */
            IsPresented = false;
            MasterPage.ListView.SelectedItem = null;
        }
    }
}