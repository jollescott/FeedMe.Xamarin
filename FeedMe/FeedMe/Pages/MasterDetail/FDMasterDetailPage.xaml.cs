using Plugin.Iconize;

namespace FeedMe.Pages.MasterDetail;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class FDMasterDetailPage : FlyoutPage
{
    public FDMasterDetailPage()
    {
        InitializeComponent();
        MasterPage.ListView.ItemSelected += ListView_ItemSelected;
    }

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (!(e.SelectedItem is FDMasterDetailPageMenuItem item)) return;

        switch (item.Id)
        {
            case 0:
                Detail = new IconNavigationPage(new MainPage()); // search page
                break;
            case 1:
                Detail = new NavigationPage(new MealsListPage(true) { Title = "Sök Recept" }); // search with name page
                break;
            case 2:
                Detail = new NavigationPage(new MealsListPage { Title = "Gillade Recept" }); // saved recipes page
                break;
        }

        IsPresented = false;
        MasterPage.ListView.SelectedItem = null;
    }
}