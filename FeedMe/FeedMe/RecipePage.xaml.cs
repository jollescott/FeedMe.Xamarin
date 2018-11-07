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
        Meal meal;
        public RecipePage (Meal meal_)
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            meal = meal_;
            XamlSetup();
		}

        void XamlSetup()
        {
            //Head
            Frame_Head.BackgroundColor = Constants.navigationBarColor;
            Frame_Head.Padding = new Thickness(Constants.navigationBarPadding, 0, Constants.navigationBarPadding, Constants.navigationBarPadding);

            AbsLayout_Head.HeightRequest = Constants.navigationBarHeight;
            AbsLayout_Head.Padding = new Thickness(0, 0, 0, Constants.padding);

            Image_Back.WidthRequest = Constants.navigationBarHeight;
            Image_Back.HeightRequest = Constants.navigationBarHeight;

            Label_HeadTitle.Text = "Recept";
            Label_HeadTitle.TextColor = Constants.textColor1;
            Label_HeadTitle.FontSize = Constants.fontSize1;

            //ExtraHead
            Label_HeadTL1.Text = "Betyg:";
            Label_HeadTL1.TextColor = Constants.textColor1;
            Label_HeadTL1.FontSize = Constants.fontSize2;

            Label_HeadTL2.Text = meal.rating + "/5";
            Label_HeadTL2.TextColor = Constants.textColor2;
            Label_HeadTL2.FontSize = Constants.fontSize2;

            Label_HeadBL1.Text = "Antal:";
            Label_HeadBL1.TextColor = Constants.textColor1;
            Label_HeadBL1.FontSize = Constants.fontSize2;

            Label_HeadBL2.Text = Convert.ToString(meal.ratings);
            Label_HeadBL2.TextColor = Constants.textColor2;
            Label_HeadBL2.FontSize = Constants.fontSize2;

            Button_Rate.Text = "Betygsätt";
            Button_Rate.TextColor = Constants.textColor3;
            Button_Rate.FontSize = Constants.fontSize2;
            Button_Rate.BackgroundColor = Constants.mainColor1;

            //Recipe Image
            Image_Recipe.Source = meal.imageLink;
            double reselution = Application.Current.MainPage.Width;
            Image_Recipe.WidthRequest = reselution;
            Image_Recipe.HeightRequest = reselution;

            //Recipe Title
            Label_RecipeName.Text = meal.name;
            Label_RecipeName.TextColor = Constants.textColor1;
            Label_RecipeName.FontSize = Constants.fontSize1;

            //Ingredients
            Stack_Ingridients.Margin = Constants.padding;

            Label_IngridientsHead.Text = "Ingidienser";
            Label_IngridientsHead.TextColor = Constants.textColor2;
            Label_IngridientsHead.FontSize = Constants.fontSize1;

            string dot = "● ";

            Label[] labels = new Label[meal.ingredients.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = new Label();
                labels[i].Text = dot + meal.ingredients[i];
                labels[i].TextColor = Constants.textColor1;
                labels[i].FontSize = Constants.fontSize2;
                labels[i].Margin = Constants.textListMargin;
                Stack_Ingridients.Children.Add(labels[i]);
            }


            //Instructions
            Stack_Instructions.Margin = Constants.padding;

            Label_InstructionsHead.Text = "Så här gör du";
            Label_InstructionsHead.TextColor = Constants.textColor2;
            Label_InstructionsHead.FontSize = Constants.fontSize1;

            Label_Instructions.Text = meal.recipe;
            Label_Instructions.TextColor = Constants.textColor1;
            Label_Instructions.FontSize = Constants.fontSize2;
        }

        //Rate Button
        private void Button_Rate_Clicked(object sender, EventArgs e)
        {

        }

        //Navigation back button
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}