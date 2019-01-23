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
using System.Text;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MealsListPage : ContentPage
	{
        List<RecipeMetaModel> recipeMetaModels;
        List<RecipeMetaDtoV2> recipeMetas;
        List<IngredientDtoV2> myIngredients;
        HttpClient httpClient = new HttpClient();
		public MealsListPage (List<IngredientDtoV2> myIngredients)
		{
            InitializeComponent();
            this.myIngredients = myIngredients;
            POST_recipeMetas(myIngredients);
        }

        // Get the recipeMetas with POST request
        async void POST_recipeMetas(List<IngredientDtoV2> ingredientDtos)
        {
            var json = JsonConvert.SerializeObject(ingredientDtos); //skicka ingredientDto

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage respone = await httpClient.PostAsync(RamseyApi.V2.Recipe.Suggest, content);

                if (respone.IsSuccessStatusCode)
                {
                    var result = await respone.Content.ReadAsStringAsync();

                    Label_Loading.Text = "Sorterar...";

                    recipeMetas = JsonConvert.DeserializeObject<List<RecipeMetaDtoV2>>(result).OrderByDescending(o => o.Coverage).ToList();

                    XamlSetup();
                }
                else
                {
                    await DisplayAlert("Connection error", "Status code " + (int)respone.StatusCode + ": " + respone.StatusCode.ToString(), "ok");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("An error occurred", "Server conection failed", "ok");
            }
        }

        void XamlSetup()
        {
            Label_Loading.IsEnabled = false;
            Label_Loading.IsVisible = false;

            recipeMetaModels = recipeMetas.Select(x =>
            {
                return new RecipeMetaModel
                {
                    Image = x.Image,
                    Source = x.Source,
                    Name = x.Name,
                    Owner = x.Owner,
                    OwnerLogo = x.OwnerLogo,
                    CoverageMessage = "Du har " + ((int)(x.Coverage * 100)).ToString() + "%  av alla ingredienser",
                    LogoRadius = 40,
                    RecipeID = x.RecipeID
                };
            }).ToList();

            for (int i = 0; i < recipeMetaModels.Count; i++)
            {
                if (i % 4 == 0)
                {
                    recipeMetaModels.Insert(i, new RecipeMetaModel { IsAd = true });
                }
            }


            ListView_Recipes.BackgroundColor = Color.Transparent;
            ListView_Recipes.ItemsSource = recipeMetaModels;

            ActivityIndicatior_WaitingForServer.IsRunning = false;
        }

        //Recipe selected
        private void ListView_Recipes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = ListView_Recipes.SelectedItem;
            if (selected != null)
            {
                int selectedItemIndex = recipeMetaModels.IndexOf(selected);
                int index = selectedItemIndex - (int)(selectedItemIndex / 4f) - 1;

                GotoRecipePage(recipeMetas[index]);
            }

            ((ListView)sender).SelectedItem = null;
        }

        //Next page
        async void GotoRecipePage(RecipeMetaDtoV2 recipeMeta)
        {
            await Navigation.PushAsync(new RecipePage(recipeMeta, myIngredients) { Title = recipeMeta.Name });

            ListView_Recipes.SelectedItem = null;
        }

        //Navigation back button
        async private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }

}