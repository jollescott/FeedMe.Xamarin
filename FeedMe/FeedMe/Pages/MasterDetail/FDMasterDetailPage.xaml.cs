﻿using FeedMe.Interfaces;
using FeedMe.Pages.Popups;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as FDMasterDetailPageMenuItem;
            if (item == null)
                return;

            if (item.Id == 0)
            {
                Detail = new NavigationPage(new LoadingPage());
            }
            switch (item.Id)
            {
                case 0:
                    Detail = new NavigationPage(new MainPage());
                    break;
                case 1:
                    
                    if(DependencyService.Get<IFacebook>().UserId == null)
                        PopupNavigation.Instance.PushAsync(new LoginPage());

                    break;

                case 2:
                    Detail = new NavigationPage(new shoppingListPage());
                    break;
            }

            /*var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;*/
        }
    }
}