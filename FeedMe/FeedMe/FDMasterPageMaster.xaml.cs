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

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FDMasterPageMaster : ContentPage
    {
        public ListView ListView;

        public FDMasterPageMaster()
        {
            InitializeComponent();

            BindingContext = new FDMasterPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class FDMasterPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<FDMasterPageMenuItem> MenuItems { get; set; }
            
            public FDMasterPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<FDMasterPageMenuItem>(new[]
                {
                    new FDMasterPageMenuItem { Id = 0, Title = "Sök Recept" },
                    new FDMasterPageMenuItem { Id = 1, Title = "Mina ingridienser" },
                    new FDMasterPageMenuItem { Id = 2, Title = "Mitt Konto" },
                    new FDMasterPageMenuItem { Id = 3, Title = "Page 4" },
                    new FDMasterPageMenuItem { Id = 4, Title = "Page 5" },
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