using Ramsey.NET.Dto;
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
            //Head
            Frame_Head.BackgroundColor = Constants.navigationBarColor;
            Frame_Head.Padding = new Thickness(Constants.navigationBarPadding, 0, Constants.navigationBarPadding, Constants.navigationBarPadding);

            AbsLayout_Head.HeightRequest = Constants.navigationBarHeight;
            AbsLayout_Head.Padding = new Thickness(0, 0, 0, Constants.padding2);

            Image_Back.WidthRequest = Constants.navigationBarHeight;
            Image_Back.HeightRequest = Constants.navigationBarHeight;

            Label_HeadTitle.Text = "Recept";
            Label_HeadTitle.TextColor = Constants.textColor1;
            Label_HeadTitle.FontSize = Constants.fontSize1;

            //ExtraHead
            Label_HeadTL1.Text = "Betyg:";
            Label_HeadTL1.TextColor = Constants.textColor1;
            Label_HeadTL1.FontSize = Constants.fontSize2;

            Label_HeadTL2.Text = recipe.Rating + "/5";
            Label_HeadTL2.TextColor = Constants.textColor2;
            Label_HeadTL2.FontSize = Constants.fontSize2;

            Label_HeadBL1.Text = "Antal:";
            Label_HeadBL1.TextColor = Constants.textColor1;
            Label_HeadBL1.FontSize = Constants.fontSize2;

            Label_HeadBL2.Text = Convert.ToString("vet inte");
            Label_HeadBL2.TextColor = Constants.textColor2;
            Label_HeadBL2.FontSize = Constants.fontSize2;

            Button_Rate.Text = "Betygsätt";
            Button_Rate.TextColor = Constants.textColor3;
            Button_Rate.FontSize = Constants.fontSize2;
            Button_Rate.BackgroundColor = Constants.mainColor1;

            //Recipe Image
            Image_Recipe.Source = "food.jpg"; //recipe.ImageLink;
            double reselution = Application.Current.MainPage.Width;
            Image_Recipe.WidthRequest = reselution;
            Image_Recipe.HeightRequest = reselution;

            //Recipe Title
            Label_RecipeName.Text = recipe.Name;
            Label_RecipeName.TextColor = Constants.textColor1;
            Label_RecipeName.FontSize = Constants.fontSize1;

            //Ingredients
            Stack_Ingridients.Margin = Constants.padding1;

            Label_IngridientsHead.Text = "Ingidienser";
            Label_IngridientsHead.TextColor = Constants.textColor2;
            Label_IngridientsHead.FontSize = Constants.fontSize1;

            string dot = "● ";
            for (int i = 0; i < recipe.RecipeParts.Count; i++)
            {
                Stack_Ingridients.Children.Add(new Label()
                {
                    Text = dot + recipe.RecipeParts[i].Quantity + " " + recipe.RecipeParts[i].Unit + " ???",
                    TextColor = Constants.textColor1,
                    FontSize = Constants.fontSize2,
                    Margin = Constants.textListMargin
                });
            }


            //Instructions
            Stack_Instructions.Margin = Constants.padding1;

            Label_InstructionsHead.Text = "Så här gör du";
            Label_InstructionsHead.TextColor = Constants.textColor2;
            Label_InstructionsHead.FontSize = Constants.fontSize1;


            for (int i = 0; i < recipe.Directions.Count; i++)
            {
                Stack_Instructions.Children.Add(new Label()
                {
                    Text = Convert.ToString(i + 1) + ".",
                    TextColor = Constants.textColor2,
                    FontSize = Constants.fontSize1,
                    Margin = Constants.textListMargin
                });

                Stack_Instructions.Children.Add(new Label()
                {
                    Text = recipe.Directions[i].Instruction,
                    TextColor = Constants.textColor1,
                    FontSize = Constants.fontSize2,
                    Margin = Constants.textListMargin
                });
            }
        }

        //Rate Button
        private void Button_Rate_Clicked(object sender, EventArgs e)
        {

        }

        //Navigation back button
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            //Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}