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

            recipe = recipe_;
            XamlSetup();
		}

        void XamlSetup()
        {
            //Recipe Image
            Grid_Images.HeightRequest = Application.Current.MainPage.Width;
            Image_Recipe.Source = recipe.Image;
            Image_OwnerLogo.Source = recipe.OwnerLogo;


            //Owner Info
            StackLayout_OwnerInfo.BackgroundColor = Color.LightGray;
            StackLayout_OwnerInfo.Padding = new Thickness(Constants.padding1,Constants.padding3,Constants.padding1,Constants.padding3);
            Label_OwnerInfoHead.FontSize = Constants.fontSize4;
            Label_OwnerLink.FontSize = Constants.fontSize4;
            Label_OwnerLink.Text = recipe.Source;
            Label_OwnerLink.TextColor = Constants.AppColor.text_link;


            //Ingredients
            Frame_IngredientsHead.BackgroundColor = Constants.AppColor.green;
            Frame_IngredientsHead.Margin = new Thickness(0, Constants.padding1, 0, 0);

            Label_IngridientsHead.Text = "Ingredienser";
            Label_IngridientsHead.TextColor = Constants.AppColor.text_white;
            Label_IngridientsHead.FontSize = Constants.fontSize1;

            Stack_Ingridients.Margin = Constants.padding1;

            string dot = "- ";
            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                Stack_Ingridients.Children.Add(new Label()
                {
                    Text = dot + recipe.Ingredients[i],
                    TextColor = Constants.AppColor.text_black,
                    FontSize = Constants.fontSize3,
                    Margin = Constants.textListMargin
                });
            }


            //Instructions
            Frame_InstructionsHead.BackgroundColor = Constants.AppColor.green;
            Frame_InstructionsHead.Padding = Constants.padding1;

            Label_InstructionsHead.Text = "Tillagning";
            Label_InstructionsHead.TextColor = Constants.AppColor.text_white;
            Label_InstructionsHead.FontSize = Constants.fontSize1;

            Stack_Instructions.Margin = new Thickness( Constants.padding2, Constants.padding2, Constants.padding2, 3 * Constants.padding2 );

            int n = 1;
            for (int i = 0; i < recipe.Directions.Count; i++)
            {
                string text = recipe.Directions[i].TrimStart();

                if (char.IsDigit(text[0]))
                {
                    //Add number
                    Stack_Instructions.Children.Add(new Label()
                    {
                        Text = Convert.ToString(n) + ".",
                        TextColor = Constants.AppColor.green,
                        FontSize = Constants.fontSize2,
                        Margin = Constants.textListMargin
                    });
                    n++;

                    // Remove start numbers (if they exist)
                    while (char.IsDigit(text[0]) || text[0] == '.')
                    {
                        text = text.Substring(1);
                    }
                    text = text.TrimStart();

                    //Add instruction
                    Stack_Instructions.Children.Add(new Label()
                    {
                        Text = text,
                        TextColor = Constants.AppColor.text_black,
                        FontSize = Constants.fontSize3,
                        Margin = Constants.textListMargin
                    });
                }
                else
                {
                    n = 1; //reset step count

                    //Add sub heading
                    Stack_Instructions.Children.Add(new Label()
                    {
                        Text = recipe.Directions[i].TrimStart(),
                        TextColor = Constants.AppColor.text_green,
                        FontSize = Constants.fontSize2,
                        Margin = new Thickness(Constants.textListMargin, Constants.padding3, Constants.textListMargin, Constants.textListMargin),
                        HorizontalTextAlignment = TextAlignment.Center
                    });
                }
            }
        }

        //Navigation back button
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            //Application.Current.MainPage.Navigation.PopAsync();
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(recipe.Source));
        }
    }
}