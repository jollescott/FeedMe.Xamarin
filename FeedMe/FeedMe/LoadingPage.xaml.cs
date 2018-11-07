using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingPage : ContentPage
	{
		public LoadingPage ()
		{
			InitializeComponent ();
            Init();
            
            //img0.Source = ImageSource.FromFile("img_0.jpg");
		}

        void Init()
        {
            AbsLayout.BackgroundColor = Constants.backgroundColor;
        }


        protected async override void OnAppearing()
        {
            await Task.Delay(5000);

            //Application.Current.MainPage = new NavigationPage(new MainPage());
            Application.Current.MainPage = new NavigationPage(new MainPage() { Title = "" }) {BarBackgroundColor = Constants.navigationBarColor, BarTextColor = Constants.mainColor1 };
        }
    }
}