using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe.Pages.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : PopupPage
    {
        public LoginPage()
        {
            InitializeComponent();

            MessagingCenter.Instance.Subscribe(Application.Current, "FacebookLogin_Success", (Application app) =>
            {
                PopupNavigation.Instance.PopAsync();
            });
        }
    }
}