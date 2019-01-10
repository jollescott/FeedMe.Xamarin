using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

using Newtonsoft.Json;
using System.Linq;
using FeedMe.Models;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Misc;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MealsListPage : ContentPage
	{
        List<RecipeMetaModel> recipes;
        List<IngredientDtoV2> myIngredients;
        HttpClient httpClient = new HttpClient();
		public MealsListPage (List<RecipeMetaDtoV2> recipes_, List<IngredientDtoV2> myIngredients)
		{
            InitializeComponent();

            this.myIngredients = myIngredients;

            recipes = recipes_.Select(x =>
            {
                return new RecipeMetaModel
                {
                    Image = x.Image,
                    Source = x.Source,
                    Name = x.Name,
                    Owner = x.Owner,
                    OwnerLogo = x.OwnerLogo,
                    CoverageMessage = "Du har: " + ((int)(x.Coverage * 100)).ToString() + "%  av alla ingredienser",
                    RecipeID = x.RecipeID
                };
            }).ToList();

            for (int i = 0; i < recipes.Count; i++)
            {
                if (i % 4 == 0)
                {
                    recipes.Insert(i, new RecipeMetaModel { IsAd = true});
                }
            }

            XamlSetup();
        }

        void XamlSetup()
        {
            ListView_Recipes.BackgroundColor = Color.Transparent;
            //ListView_Recipes.Margin = new Thickness(0, Constants.padding2 / 2, 0, Constants.padding1);

            //Frame_List.Margin = Constants.padding2; //går inte för den är i view cell
            ListView_Recipes.ItemsSource = recipes;
        }

        //Recipe selected
        private void ListView_Recipes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = ListView_Recipes.SelectedItem;
            if (selected != null)
            {
                int index = recipes.IndexOf(selected);

                GET_recipeDto(recipes[index].RecipeID);
            }

            ((ListView)sender).SelectedItem = null;
        }

        //Next page
        async void gotoRecipePage(RecipeDtoV2 meal)
        {
            await Navigation.PushAsync(new RecipePage(meal, myIngredients) { Title = meal.Name });

            ListView_Recipes.SelectedItem = null;
        }

        //Navigation back button
        async private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void GET_recipeDto(string id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(RamseyApi.V2.Recipe.Retreive + "?id=" + id);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    RecipeDtoV2 recipe = JsonConvert.DeserializeObject<RecipeDtoV2>(result);

                    gotoRecipePage(recipe);
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
    }

}