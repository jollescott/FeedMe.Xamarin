using Ramsey.Shared.Dto;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecipePage : ContentPage
	{
        RecipeDto recipe;
        public RecipePage (RecipeDto recipe_)
		{
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);

            recipe = recipe_;
            XamlSetup();
		}

        void XamlSetup()
        {
            //Recipe Image
            Image_Recipe.Source = recipe.Image;
            double reselution = Application.Current.MainPage.Width;
            Image_Recipe.WidthRequest = reselution;
            Image_Recipe.HeightRequest = reselution;


            //Spacing
            ContentView_Spacing1.HeightRequest = Constants.padding1;


            //Ingredients
            Frame_IngredientsHead.BackgroundColor = Constants.mainColor1;

            Label_IngridientsHead.Text = "Ingredienser";
            Label_IngridientsHead.TextColor = Constants.textColor3;
            Label_IngridientsHead.FontSize = Constants.fontSize1;

            Stack_Ingridients.Margin = Constants.padding1;


            string dot = "- ";
            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                Stack_Ingridients.Children.Add(new Label()
                {
                    Text = dot + recipe.Ingredients[i],
                    TextColor = Constants.textColor1,
                    FontSize = Constants.fontSize3,
                    Margin = Constants.textListMargin
                });
            }


            //Instructions
            Frame_InstructionsHead.BackgroundColor = Constants.mainColor1;
            Frame_InstructionsHead.Padding = Constants.padding1;

            Label_InstructionsHead.Text = "Tillagning";
            Label_InstructionsHead.TextColor = Constants.textColor3;
            Label_InstructionsHead.FontSize = Constants.fontSize1;

            Stack_Instructions.Margin = Constants.padding1;


            for (int i = 0; i < recipe.Directions.Count; i++)
            {
                Stack_Instructions.Children.Add(new Label()
                {
                    Text = Convert.ToString(i + 1) + ".",
                    TextColor = Constants.textColor2,
                    FontSize = Constants.fontSize2,
                    Margin = Constants.textListMargin
                });

                string text = recipe.Directions[i].TrimStart();
                // Remove start numbers if they exist
                while (char.IsDigit(text[0]) || text[0] == '.')
                {
                    text = text.Substring(1);
                }
                text = text.TrimStart();

                Stack_Instructions.Children.Add(new Label()
                {
                    Text = text,
                    TextColor = Constants.textColor1,
                    FontSize = Constants.fontSize3,
                    Margin = Constants.textListMargin
                });
            }


            //Spacing
            ContentView_Spacing2.HeightRequest = 5 * Constants.padding1;
        }

        //Navigation back button
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            //Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}