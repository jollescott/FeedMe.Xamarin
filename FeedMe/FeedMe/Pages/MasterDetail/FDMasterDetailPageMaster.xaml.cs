using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace FeedMe.Pages.MasterDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FDMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public FDMasterDetailPageMaster()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                Icon = "menu.png";
            }

            //Grid_MenuBackground.BackgroundColor = Constants.AppColor.navigationBarColor;

            BindingContext = new FDMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;

            Label_PrivacyPolicy.TextColor = Constants.AppColor.text_link;
        }

        private class FDMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<FDMasterDetailPageMenuItem> MenuItems { get; set; }

            public FDMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<FDMasterDetailPageMenuItem>(new[]
                {
                    new FDMasterDetailPageMenuItem { Id = 0, Title = "Sök med ingredienser", Icon = "md-search"},
                    new FDMasterDetailPageMenuItem { Id = 1, Title = "Sök med receptnamn", Icon = "md-search"},
                    new FDMasterDetailPageMenuItem { Id = 2, Title = "Gillade recept", Icon = "md-favorite-border" },
                    //new FDMasterDetailPageMenuItem { Id = 3, Title = "Inköpslista\n(Kommer snart)", Icon = "md-shopping-basket" }
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                {
                    return;
                }

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        // Klicked PrivacyPolicy link
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://api.feedmeapp.se/privacy"));
        }
    }
}