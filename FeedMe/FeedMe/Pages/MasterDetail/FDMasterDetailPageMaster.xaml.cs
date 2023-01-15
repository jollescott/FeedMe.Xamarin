using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FeedMe.Classes;

namespace FeedMe.Pages.MasterDetail;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class FDMasterDetailPageMaster : ContentPage
{
    public ListView ListView;

    public FDMasterDetailPageMaster()
    {
        InitializeComponent();

        if (Device.RuntimePlatform == Device.iOS)
        {
            //Icon = "menu.png";
        }

        //Grid_MenuBackground.BackgroundColor = Constants.AppColor.navigationBarColor;

        BindingContext = new FDMasterDetailPageMasterViewModel();
        ListView = MenuItemsListView;

        Label_PrivacyPolicy.TextColor = Constants.AppColor.TextLink;
    }

    // Klicked PrivacyPolicy link
    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await Browser.OpenAsync(new Uri("https://api.feedmeapp.se/privacy"));
    }

    private class FDMasterDetailPageMasterViewModel : INotifyPropertyChanged
    {
        public FDMasterDetailPageMasterViewModel()
        {
            MenuItems = new ObservableCollection<FDMasterDetailPageMenuItem>(new[]
            {
                new FDMasterDetailPageMenuItem { Id = 0, Title = "Sök med ingredienser", Icon = "md-search" },
                new FDMasterDetailPageMenuItem { Id = 1, Title = "Sök med receptnamn", Icon = "md-search" },
                new FDMasterDetailPageMenuItem { Id = 2, Title = "Gillade recept", Icon = "md-favorite-border" }
                //new FDMasterDetailPageMenuItem { Id = 3, Title = "Inköpslista\n(Kommer snart)", Icon = "md-shopping-basket" }
            });
        }

        public ObservableCollection<FDMasterDetailPageMenuItem> MenuItems { get; }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null) return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}