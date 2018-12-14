using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe.Pages.MasterDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FDMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public FDMasterDetailPageMaster()
        {
            InitializeComponent();

            Grid_MenuBackground.BackgroundColor = Constants.AppColor.navigationBarColor;

            BindingContext = new FDMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class FDMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<FDMasterDetailPageMenuItem> MenuItems { get; set; }
            
            public FDMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<FDMasterDetailPageMenuItem>(new[]
                {
                    new FDMasterDetailPageMenuItem { Id = 0, Title = "Sök recept" },
                    new FDMasterDetailPageMenuItem { Id = 1, Title = "Gillade recept\n(Kommer snart)" },
                    new FDMasterDetailPageMenuItem { Id = 2, Title = "Inköpslista\n(Kommer snart)" }
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}