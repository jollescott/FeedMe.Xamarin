using FeedMe.Models;
using Newtonsoft.Json;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MealsListPage : ContentPage
    {
        private List<RecipeMetaModel> recipeMetaModels;
        private List<RecipeMetaDtoV2> recipeMetas;
        private readonly List<RecipeDtoV2> recipes;
        private readonly List<IngredientDtoV2> myIngredients;
        private readonly HttpClient httpClient = new HttpClient();
        private string searchWord;
        private readonly bool viewFavorites = false;
        private readonly bool nameSearching = false;

        public MealsListPage()
        {
            InitializeComponent();
            viewFavorites = true;

            Label_Loading.Text = "Laddar...";
            recipes = User.User.SavedRecipes;
            XamlSetup();

            if (recipes.Count < 1)
            {
                Label_Message.Text = "Här sparas de recept du har gillat";
                Label_Message.IsVisible = true;
            }
        }

        public MealsListPage(bool NameSearching)
        {
            InitializeComponent();
            nameSearching = true;
            Label_Loading.IsEnabled = false;
            Label_Loading.IsVisible = false;
            ActivityIndicatior_WaitingForServer.IsRunning = false;
            Frame_RecipeSearching.IsEnabled = true;
            Frame_RecipeSearching.IsVisible = true;
        }

        public MealsListPage(List<IngredientDtoV2> ingredients)
        {
            InitializeComponent();
            myIngredients = ingredients;

            recipeMetas = new List<RecipeMetaDtoV2>();
            myIngredients = ingredients;
            ReciveRecipeMetas(0);
        }

        private async void Alert(string title, string message, string cancel)
        {
            await DisplayAlert(title, message, cancel);
        }

        private async void ReciveRecipeMetas(int start)
        {
            string json = JsonConvert.SerializeObject(myIngredients);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(RamseyApi.V2.Recipe.Suggest + "?start=" + start.ToString(), content);

                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    List<RecipeMetaDtoV2> recivedRecipeMetas = JsonConvert.DeserializeObject<List<RecipeMetaDtoV2>>(result);
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
                    Alert("Fel", "Kunnde inte ansluta till servern\n\nstatus code: " + (int)response.StatusCode, "ok");
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

        private void XamlSetup()
        {
            Label_Loading.IsEnabled = false;
            Label_Loading.IsVisible = false;

            int frameHeight = (int)(Application.Current.MainPage.Width * 0.8);

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
                        FrameHeight = frameHeight,
                        ShowCoverageMessage = false,
                        LogoRadius = 40,
                        RecipeId = x.RecipeId
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
                        FrameHeight = frameHeight,
                        CoverageMessage = "Du har " + ((int)(x.Coverage * 100)).ToString() + "%  av alla ingredienser",
                        ShowCoverageMessage = !nameSearching,
                        LogoRadius = 40,
                        RecipeId = x.RecipeId
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

        private bool canOpenNewRecipes = true;
        //Recipe selected
        private void ListView_Recipes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            object selected = ListView_Recipes.SelectedItem;

            if (selected == null || !canOpenNewRecipes)
            {
                return;
            }

            canOpenNewRecipes = false;

            int selectedItemIndex = recipeMetaModels.IndexOf(selected);
            ListView_Recipes.SelectedItem = null;

            // Return it was an ad
            for (int i = 0; i < recipeMetaModels.Count; i += 4)
            {
                if (selectedItemIndex == i)
                {
                    return;
                }
            }

            int index = selectedItemIndex - (int)(selectedItemIndex / 4f) - 1;

            if (viewFavorites)
            {
                GotoRecipePage(recipes[index]);
            }
            else
            {
                GotoRecipePage(recipeMetas[index]);
            }
        }

        //Next page
        private async void GotoRecipePage(RecipeMetaDtoV2 recipeMeta)
        {
            await Navigation.PushAsync(new RecipePage(recipeMeta) { Title = recipeMeta.Name });

            canOpenNewRecipes = true;
        }

        //Next page (favorite)
        private async void GotoRecipePage(RecipeDtoV2 recipe)
        {
            await Navigation.PushAsync(new RecipePage(recipe) { Title = recipe.Name });

            canOpenNewRecipes = true;
        }

        //Navigation back button
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        //Load more recipes button
        private void Button_ViewMoreRecipes_Clicked(object sender, EventArgs e)
        {
            ActivityIndicatior_WaitingForServer_LoadingMoreRecipes.IsRunning = true;
            Button_ViewMoreRecipes.IsEnabled = false;
            Button_ViewMoreRecipes.IsVisible = false;
            if (nameSearching)
            {
                ReciveRecipeMetasFromName(recipeMetas.Count);
            }
            else
            {
                ReciveRecipeMetas(recipeMetas.Count);
            }
        }

        private async void ReciveRecipeMetasFromName(int start, int id = -1)
        {

            try
            {
                string json = JsonConvert.SerializeObject(new List<RecipeMetaDto>());
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(RamseyApi.V2.Recipe.Text + "?search=" + searchWord + "&start=" + start, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    List<RecipeMetaDtoV2> recivedRecipeMetas = JsonConvert.DeserializeObject<List<RecipeMetaDtoV2>>(result);

                    // return if this is not the latest search
                    if (id != -1 && id != latestId)
                    {
                        return;
                    }

                    if (start == 0)
                    {
                        recipeMetas = new List<RecipeMetaDtoV2>();
                        recipeMetaModels = new List<RecipeMetaModel>();
                    }

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

                    if (recipeMetaModels.Count == 0)
                    {
                        Label_Message.Text = "Inga recept hittades";
                        Label_Message.IsVisible = true;
                        Label_Message.IsEnabled = true;
                    }


                    if (id == latestId)
                    {
                        latestId = 0;
                        nextId = 0;
                    }
                    else
                    {
                        nextId++;
                    }
                }
                else
                {
                    Alert("Fel", "Kunnde inte ansluta till servern\n\nstatus code: " + (int)response.StatusCode, "ok");
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


        // Search
        private int nextId = 0;
        private int latestId = 0;
        private void SearchBar_RecipeSearching_TextChanged(object sender, TextChangedEventArgs e)
        {
            int id = nextId;
            latestId = nextId;

            Label_Message.IsVisible = false;
            Label_Message.IsEnabled = false;

            Label_Loading.IsEnabled = true;
            Label_Loading.IsVisible = true;
            ActivityIndicatior_WaitingForServer.IsRunning = true;

            if (recipeMetaModels != null && recipeMetaModels.Count > 0)
            {
                ListView_Recipes.ScrollTo(((List<RecipeMetaModel>)ListView_Recipes.ItemsSource)[0], ScrollToPosition.Center, false);
            }

            searchWord = SearchBar_RecipeSearching.Text;

            ReciveRecipeMetasFromName(0, id);
        }
    }

}