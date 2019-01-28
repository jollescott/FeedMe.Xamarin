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
        List<IngredientDtoV2> myIngredients;
        HttpClient httpClient = new HttpClient();

        bool viewFavorites;

		public MealsListPage()
		{
            InitializeComponent();
            viewFavorites = true;

            Label_Loading.Text = "Laddar...";
            recipeMetas = User.User.SavedRecipeMetas;
            XamlSetup();

            if (recipeMetas.Count < 1)
            {
                Label_Message.Text = "Här sparas de recept om du har gillat";
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
                    Label_Loading.Text = "Sorterar...";

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
                    Alert("Connection error", "Status code " + (int)respone.StatusCode + ": " + respone.StatusCode.ToString(), "ok");
            }
            catch (Exception _e)
            {
                Alert("An error occurred", "Server conection failed", "ok");
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
                    ShowCoverageMessage = (viewFavorites) ? false : true,
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

        bool canViewRecipe = true;
        //Recipe selected
        private void ListView_Recipes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = ListView_Recipes.SelectedItem;

            if (selected == null)
                return;

            canViewRecipe = false;

            int selectedItemIndex = recipeMetaModels.IndexOf(selected);
            ListView_Recipes.SelectedItem = null;

            for (int i = 0; i < recipeMetaModels.Count; i += 4)
            {
                if (selectedItemIndex == i)
                    return;
            }

            int index = selectedItemIndex - (int)(selectedItemIndex / 4f) - 1;

            if(viewFavorites)
                GotoRecipePage(recipeMetaModels[index].Recipe);
            else
                GotoRecipePage(recipeMetas[index]);

        }

        //Next page
        async void GotoRecipePage(RecipeMetaDtoV2 recipeMeta)
        {
            await Navigation.PushAsync(new RecipePage(recipeMeta) { Title = recipeMeta.Name });

            canViewRecipe = true;
        }
        //Next page
        async void GotoRecipePage(RecipeDtoV2 recipe)
        {
            await Navigation.PushAsync(new RecipePage(recipe) { Title = recipe.Name });

            ListView_Recipes.SelectedItem = null;
        }

        //Navigation back button
        async private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        //Load more recipes button
        void Button_ViewMoreRecipes_Clicked(object sender, EventArgs e)
        {
            ReciveRecipeMetas(recipeMetas.Count);
        }
    }

}