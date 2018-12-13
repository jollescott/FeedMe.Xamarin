using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class shoppingListPage : ContentPage
	{

		public shoppingListPage ()
		{
			InitializeComponent ();

            XamlSetup();
		}

        void XamlSetup()
        {
            ListView_ShoppingList.BackgroundColor = Constants.backgroundColor;
            //ListView_ShoppingList.ItemsSource = 
        }
	}


}