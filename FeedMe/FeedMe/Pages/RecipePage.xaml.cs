﻿using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using Ramsey.Shared.Misc;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecipePage : ContentPage
	{
        HttpClient httpClient = new HttpClient();
        RecipeMetaDtoV2 recipeMeta;
        RecipeDtoV2 recipe;
        List<IngredientDtoV2> myIngredients;
        List<Label> ingredentPortionLabels = new List<Label>();
        public RecipePage (RecipeMetaDtoV2 recipeMeta, List<IngredientDtoV2> myIngredients)
		{
            InitializeComponent();

            this.recipeMeta = recipeMeta;
            this.myIngredients = myIngredients;
            XamlSetup1();
            GET_recipeDto(recipeMeta.RecipeID);
		}

        async void GET_recipeDto(string id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(RamseyApi.V2.Recipe.Retreive + "?id=" + id);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    recipe = JsonConvert.DeserializeObject<RecipeDtoV2>(result);
                    XamlSetup2();
                }
                else
                {
                    await DisplayAlert("Response error", "Status code " + (int)response.StatusCode + ": " + response.StatusCode.ToString(), "ok");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("An error occurred", "Server conection failed", "ok");
            }
        }

        void XamlSetup1()
        {
            //Recipe Image
            Grid_Images.HeightRequest = Application.Current.MainPage.Width;
            Image_Recipe.Source = recipeMeta.Image;
            Image_OwnerLogo.Source = recipeMeta.OwnerLogo;


            //Owner Info
            StackLayout_OwnerInfo.BackgroundColor = Color.LightGray;
            StackLayout_OwnerInfo.Padding = new Thickness(Constants.padding1,Constants.padding3,Constants.padding1,Constants.padding3);
            Label_OwnerInfoHead.FontSize = Constants.fontSize4;
            Label_OwnerLink.FontSize = Constants.fontSize4;
            Label_OwnerLink.Text = recipeMeta.Source;
            Label_OwnerLink.TextColor = Constants.AppColor.text_link;


            //Ingredients head
            Frame_IngredientsHead.BackgroundColor = Constants.AppColor.green;
            Frame_IngredientsHead.Margin = new Thickness(0, Constants.padding1, 0, 0);

            Label_IngridientsHead.Text = "Ingredienser";
            Label_IngridientsHead.TextColor = Constants.AppColor.text_white;
            Label_IngridientsHead.FontSize = Constants.fontSize1;

            //Portions
            Grid_Portions.Margin = Constants.padding2;
            Label_Portions.FontSize = Constants.fontSize3;

            //Ingredients
            Grid_Ingredients.Margin = Constants.padding2;

            for (int i = 0; i < recipeMeta.RecipeParts.Count() + 1; i++)
            {
                Grid_Ingredients.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            Grid_Ingredients.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Grid_Ingredients.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });

            for (int i = 0; i < recipeMeta.RecipeParts.Count(); i++)
            {
                // quantity and unit
                ingredentPortionLabels.Add(new Label()
                {
                    //Text = (recipeMeta.RecipeParts.ToList()[i].Quantity != 0) ? recipeMeta.RecipeParts.ToList()[i].Quantity.ToString().Trim() + " " + recipeMeta.RecipeParts.ToList()[i].Unit.Trim() : "",
                    TextColor = Constants.AppColor.text_gray,
                    FontSize = Constants.fontSize3,
                    Margin = Constants.textListMargin,
                    HorizontalTextAlignment = TextAlignment.End
                });
                Grid_Ingredients.Children.Add(ingredentPortionLabels[i], 0, i);

                // ingredient names
                Grid_Ingredients.Children.Add(new Label()
                {
                    Text = recipeMeta.RecipeParts.ToList()[i].IngredientID.Trim(),
                    TextColor = Constants.AppColor.text_black,
                    FontSize = Constants.fontSize3,
                    Margin = Constants.textListMargin
                }, 1, i);

                // has ingredient icons
                foreach (var myIngredient in myIngredients)
                {
                    if (recipeMeta.RecipeParts.ToList()[i].IngredientID == myIngredient.IngredientId)
                    {
                        Grid_Ingredients.Children.Add(new Image()
                        {
                            Source = "icon_check.png",
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Center,
                            Aspect = Aspect.AspectFit,
                            HeightRequest = 15,
                            Margin = new Thickness(0, 0, 5, 0)
                        }, 1, i);
                    }
                }

                // separation lines
                Grid_Ingredients.Children.Add(new BoxView()
                {
                    BackgroundColor = Constants.AppColor.gray,
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                }, 0, 2, i, i + 1);
            }

            UpdateIngredientPortions((int)Stepper_Portions.Value);


            //Instructions head
            Frame_InstructionsHead.BackgroundColor = Constants.AppColor.green;
            Frame_InstructionsHead.Padding = Constants.padding1;

            Label_InstructionsHead.Text = "Tillagning";
            Label_InstructionsHead.TextColor = Constants.AppColor.text_white;
            Label_InstructionsHead.FontSize = Constants.fontSize1;
        }
        
        void XamlSetup2()
        {
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

        void UpdateIngredientPortions(int portions)
        {
            double portionMuliplier = 0.5;
            Label_Portions.Text = (portions == 1) ? portions.ToString() + " portion" : portions.ToString() + " portioner";
            for (int i = 0; i < ingredentPortionLabels.Count; i++)
            {
                ingredentPortionLabels[i].Text = (recipeMeta.RecipeParts.ToList()[i].Quantity != 0) ? (recipeMeta.RecipeParts.ToList()[i].Quantity * portions * portionMuliplier).ToString().Trim() + " " + recipeMeta.RecipeParts.ToList()[i].Unit.Trim() : "";
            }
        }

        //Navigation back button
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }


        // Source link
        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            // open source in browser
            Device.OpenUri(new Uri(recipeMeta.Source));
        }

        private void Stepper_Portions_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            UpdateIngredientPortions((int)Stepper_Portions.Value);
        }
    }
}