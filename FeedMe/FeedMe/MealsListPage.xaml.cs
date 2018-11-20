using Ramsey.NET.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //NavigationPage.SetHasNavigationBar(this, false);

            recipes = recipes_;

            //List<string> recipeNames = new List<string>();
            //foreach (var recipe in recipes)
            //{
            //    recipeNames.Add(recipe.Name);
            //}

            XamlSetup();
		}

        List<Cell> itemSorce = new List<Cell>();
        void XamlSetup()
        {
            //Recipes
            for (int i = 0; i < recipes.Count; i++)
            {

                string[] stars = new string[] {"empty_star.png", "empty_star.png", "empty_star.png", "empty_star.png", "empty_star.png"};
                for (int j = 0; j < 5; j++)
                {
                    if (recipes[i].Rating - j >= 0.66)
                    {
                        stars[j] = "full_star.png";
                    }
                    else if (recipes[i].Rating - j >= 0.33)
                    {
                        stars[j] = "half_star.png";
                    }
                }

                itemSorce.Add(new Cell() {
                    Name = recipes[i].Name,
                    imgsource = "food.jpg", //meals[i].ImageLink,
                    textColor = Constants.textColor1,
                    TextSize = Constants.fontSize2,
                    backgroundColor = (i % 2 == 0) ? Constants.listBackgroundColor1 : Constants.listBackgroundColor2,
                    Margin = Constants.padding3,
                    Star0 = stars[0],
                    Star1 = stars[1],
                    Star2 = stars[2],
                    Star3 = stars[3],
                    Star4 = stars[4],
                    StarSize = 20
                });
            }
            ListView_Recipes.ItemsSource = itemSorce;
        }

        //Recipe selected
        private void ListView_Recipes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            gotoRecipePage(recipes[itemSorce.IndexOf(ListView_Recipes.SelectedItem)]);
        }

        //Next page
        async void gotoRecipePage(RecipeDto meal)
        {
            await Navigation.PushAsync(new RecipePage(meal) { Title = "Recept" });
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