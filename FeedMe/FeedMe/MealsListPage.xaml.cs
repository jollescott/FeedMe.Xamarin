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
	public partial class MealsListPage : ContentPage
	{
        Meal[] meals;
		public MealsListPage (Meal[] meals_)
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            meals = meals_;

            List<string> recipeNames = new List<string>();
            foreach (var meal in meals)
            {
                recipeNames.Add(meal.name);
            }

            ListVeiw_Meals.ItemsSource = recipeNames;

            XamlSetup();

            AddRecipes();
		}

        void XamlSetup()
        {
            //Head
            Frame_Head.BackgroundColor = Constants.navigationBarColor;
            Frame_Head.Padding = Constants.navigationBarPadding;

            AbsLayout_Head.HeightRequest = Constants.navigationBarHeight;

            Image_Back.WidthRequest = Constants.navigationBarHeight;
            Image_Back.HeightRequest = Constants.navigationBarHeight;

            Label_HeadTitle.Text = "Recept";
            Label_HeadTitle.TextColor = Constants.textColor1;
            Label_HeadTitle.FontSize = Constants.fontSize1;
        }

        void AddRecipes()
        {
            ListView_Recipes.ItemsSource = new List<Cell>() {

                new Cell()
                {
                    Name = meals[0].name, inget = " ", imgsource = meals[0].imageLink,
                },
            };
        }

        // Go to the RecipePage page
        private async void ListVeiw_Meals_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            string selected = ListVeiw_Meals.SelectedItem.ToString();
            int index;
            for (index = 0; index < meals.Length; index++)
            {
                if (selected == meals[index].name)
                {
                    break;   
                }
            }
            await Navigation.PushAsync(new RecipePage(meals[index]) { Title = "Recept"});
        }

        //Navigation back button
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }



    public class Cell
    {
        public string Name
        {
            get;
            set;
        }
        public string inget
        {
            get;
            set;
        }
        public string imgsource
        {
            get;
            set;
        }
    }
}