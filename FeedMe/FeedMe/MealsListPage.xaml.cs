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
            meals = meals_;

            List<string> recipeNames = new List<string>();
            foreach (var meal in meals)
            {
                recipeNames.Add(meal.name);
            }

            ListVeiw_Meals.ItemsSource = recipeNames;
		}


        // Go to the RecipePage page
        private async void ListVeiw_Meals_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new RecipePage(meals[0]) { Title = "Recept"});
        }
    }
}