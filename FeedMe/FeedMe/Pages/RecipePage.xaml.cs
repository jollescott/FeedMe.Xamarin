using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecipePage : ContentPage
	{
        RecipeDtoV2 recipe;
        List<IngredientDtoV2> myIngredients;
        public RecipePage (RecipeDtoV2 recipe, List<IngredientDtoV2> myIngredients)
		{
            InitializeComponent();

            this.recipe = recipe;
            this.myIngredients = myIngredients;
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


            //Ingredients head
            Frame_IngredientsHead.BackgroundColor = Constants.AppColor.green;
            Frame_IngredientsHead.Margin = new Thickness(0, Constants.padding1, 0, 0);

            Label_IngridientsHead.Text = "Ingredienser";
            Label_IngridientsHead.TextColor = Constants.AppColor.text_white;
            Label_IngridientsHead.FontSize = Constants.fontSize1;

            //Ingredients
            Grid_Ingredients.Margin = Constants.padding2;

            for (int i = 0; i < recipe.RecipeParts.Count(); i++)
            {
                Grid_Ingredients.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            Grid_Ingredients.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            Grid_Ingredients.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });

            for (int i = 0; i < recipe.RecipeParts.Count(); i++)
            {
                Grid_Ingredients.Children.Add(new Label()
                {
                    Text = recipe.RecipeParts.ToList()[i].IngredientID.Trim(),
                    TextColor = Constants.AppColor.text_black,
                    FontSize = Constants.fontSize3,
                    Margin = Constants.textListMargin
                },
                0, i);
                Grid_Ingredients.Children.Add(new Label()
                {
                    Text = recipe.RecipeParts.ToList()[i].Quantity.ToString().Trim() + " " + recipe.RecipeParts.ToList()[i].Unit.Trim(),
                    TextColor = Constants.AppColor.text_black,
                    FontSize = Constants.fontSize3,
                    Margin = Constants.textListMargin
                },
                1, i);
            }


            //Instructions head
            Frame_InstructionsHead.BackgroundColor = Constants.AppColor.green;
            Frame_InstructionsHead.Padding = Constants.padding1;

            Label_InstructionsHead.Text = "Tillagning";
            Label_InstructionsHead.TextColor = Constants.AppColor.text_white;
            Label_InstructionsHead.FontSize = Constants.fontSize1;

            //Instructions
            bool startsWithNum = false;
            if (recipe.Owner == RecipeProvider.Hemmets)
            {
                startsWithNum = true;
            }
            bool showNumbers = true;
            if (recipe.Directions.Count() <= 1)
            {
                showNumbers = false;
            }


            Stack_Instructions.Margin = new Thickness( Constants.padding2, Constants.padding2, Constants.padding2, 3 * Constants.padding2 );

            int n = 1;
            for (int i = 0; i < recipe.Directions.Count(); i++)
            {
                string text = recipe.Directions.ToList()[i].TrimStart();

                if (!startsWithNum || char.IsDigit(text[0]))
                {
                    //Add number
                    if (showNumbers)
                    {
                        Stack_Instructions.Children.Add(new Label()
                        {
                            Text = Convert.ToString(n) + ".",
                            TextColor = Constants.AppColor.green,
                            FontSize = Constants.fontSize2,
                            Margin = Constants.textListMargin
                        });
                        n++;
                    }

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
                        Text = recipe.Directions.ToList()[i].TrimStart(),
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