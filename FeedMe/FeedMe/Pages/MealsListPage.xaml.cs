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
using System.Threading.Tasks;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MealsListPage : ContentPage
	{
        List<RecipeMetaModel> recipeMetaModels;
        List<RecipeMetaDtoV2> recipeMetas;
        List<RecipeDtoV2> recipes;
        List<IngredientDtoV2> myIngredients;
        HttpClient httpClient = new HttpClient();

        bool viewFavorites;

		public MealsListPage()
		{
            InitializeComponent();
            viewFavorites = true;

            Label_Loading.Text = "Laddar...";
            recipes= User.User.SavedRecipes;
            XamlSetup();

            if (recipes.Count < 1)
            {
                Label_Message.Text = "Här sparas de recept du har gillat";
                Label_Message.IsVisible = true;
            }
        }

        public MealsListPage(List<IngredientDtoV2> ingredients)
        {
            InitializeComponent();
            viewFavorites = false;
            myIngredients = ingredients;

            recipeMetas = new List<RecipeMetaDtoV2>();
            myIngredients = ingredients;
            ReciveRecipeMetas(0);
        }

        async void Alert(string title, string message, string cancel)
        {
            await DisplayAlert(title, message, cancel);
        }

        async void ReciveRecipeMetas(int start)
        {
            var json = JsonConvert.SerializeObject(myIngredients);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage respone = await httpClient.PostAsync(RamseyApi.V2.Recipe.Suggest + "?start=" + start.ToString(), content);

                if (respone.IsSuccessStatusCode)
                {
                    var result = respone.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var recivedRecipeMetas = JsonConvert.DeserializeObject<List<RecipeMetaDtoV2>>(result);
                    if (recivedRecipeMetas.Count < 25)
                    {
                        Button_ViewMoreRecipes.IsEnabled = false;
                        Button_ViewMoreRecipes.IsVisible = false;
                    }
                    else
                    {
                        Button_ViewMoreRecipes.IsEnabled = true;
                        Button_ViewMoreRecipes.IsVisible = true;
                    }
                    recipeMetas.AddRange(recivedRecipeMetas);
                    XamlSetup();
                }
                else
                {
                    Alert("Fel", "Kunnde inte ansluta till servern\n\nstatus code: " + (int)respone.StatusCode, "ok");
                    Label_Loading.IsVisible = false;
                    ActivityIndicatior_WaitingForServer.IsRunning = false;
                }
            }
            catch
            {
                Alert("Fel", "Kunnde inte ansluta till servern", "ok");
                Label_Loading.IsVisible = false;
                ActivityIndicatior_WaitingForServer.IsRunning = false;
            }
        }

        void XamlSetup()
        {
            Label_Loading.IsEnabled = false;
            Label_Loading.IsVisible = false;

            if (viewFavorites)
            {
                recipeMetaModels = recipes.Select(x =>
                {
                    return new RecipeMetaModel
                    {
                        Image = x.Image,
                        Source = x.Source,
                        Name = x.Name,
                        Owner = x.Owner,
                        OwnerLogo = x.OwnerLogo,
                        ShowCoverageMessage = false,
                        LogoRadius = 40,
                        RecipeID = x.RecipeID
                    };
                }).ToList();
            }
            else
            {
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
                        ShowCoverageMessage = true,
                        LogoRadius = 40,
                        RecipeID = x.RecipeID
                    };
                }).ToList();
            }

            // Add ads
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
            ActivityIndicatior_WaitingForServer_LoadingMoreRecipes.IsRunning = false;
        }

        bool canOpenNewRecipes = true;
        //Recipe selected
        private void ListView_Recipes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = ListView_Recipes.SelectedItem;

            if (selected == null || !canOpenNewRecipes)
                return;

            canOpenNewRecipes = false;

            int selectedItemIndex = recipeMetaModels.IndexOf(selected);
            ListView_Recipes.SelectedItem = null;

            for (int i = 0; i < recipeMetaModels.Count; i += 4)
            {
                if (selectedItemIndex == i)
                    return;
            }

            int index = selectedItemIndex - (int)(selectedItemIndex / 4f) - 1;

            if (viewFavorites)
                GotoRecipePage(recipes[index]);
            else
                GotoRecipePage(recipeMetas[index]);
        }

        //Next page
        async void GotoRecipePage(RecipeMetaDtoV2 recipeMeta)
        {
            await Navigation.PushAsync(new RecipePage(recipeMeta) { Title = recipeMeta.Name });

            canOpenNewRecipes = true;
        }

        //Next page (favorite)
        async void GotoRecipePage(RecipeDtoV2 recipe)
        {
            await Navigation.PushAsync(new RecipePage(recipe) { Title = recipe.Name });

            canOpenNewRecipes = true;
        }

        //Navigation back button
        async private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        //Load more recipes button
        void Button_ViewMoreRecipes_Clicked(object sender, EventArgs e)
        {
            ActivityIndicatior_WaitingForServer_LoadingMoreRecipes.IsRunning = true;
            Button_ViewMoreRecipes.IsEnabled = false;
            Button_ViewMoreRecipes.IsVisible = false;
            ReciveRecipeMetas(recipeMetas.Count);
        }
    }

}