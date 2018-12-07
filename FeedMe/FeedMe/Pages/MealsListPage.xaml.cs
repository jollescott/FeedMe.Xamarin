using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MealsListPage : ContentPage
	{
        List<RecipeDto> recipes;
		public MealsListPage (List<RecipeDto> recipes_)
		{
            InitializeComponent();

            recipes = recipes_;

            XamlSetup();
        }

        List<Cell> itemSorce = new List<Cell>();
        void XamlSetup()
        {
            //Recipes
            ListView_Recipes.RowHeight = Convert.ToInt32(Application.Current.MainPage.Width * 3/5);
            ListView_Recipes.ItemsSource = recipes;
        }

        //Recipe selected
        private void ListView_Recipes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //object selected = ListView_Recipes.SelectedItem;
            //((ListView)sender).SelectedItem = null;

            int index = recipes.IndexOf(ListView_Recipes.SelectedItem);

            gotoRecipePage(recipes[index]);
        }

        //Next page
        async void gotoRecipePage(RecipeDto meal)
        {
            await Navigation.PushAsync(new RecipePage(meal) { Title = meal.Name });
        }

        //Navigation back button
        async private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //Application.Current.MainPage.Navigation.PopAsync();
            await Navigation.PopAsync();
        }
    }



    public class Cell
    {
        public string Name { get; set; }
        public string imgsource { get; set; }
        public Color textColor { get; set; }
        public double TextSize { get; set; }
        public Color backgroundColor { get; set; }
        public int Margin { get; set; }
        public string Star0 { get; set; }
        public string Star1 { get; set; }
        public string Star2 { get; set; }
        public string Star3 { get; set; }
        public string Star4 { get; set; }
        public int StarSize { get; set; }
    }
}