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
using Microsoft.AppCenter.Analytics;

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

        string searchWord;

        bool viewFavorites = false;
        bool nameSearching = false;

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

        async void Alert(string title, string message, string cancel)
        {
            await DisplayAlert(title, message, cancel);
        }

        //async void ReciveRecipeTags()
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await httpClient.PostAsync(RamseyApi.V2.Tags.Suggest, null);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = await response.Content.ReadAsStringAsync();
        //            var recivedTags = JsonConvert.DeserializeObject<List<TagDto>>(result);
        //            Console.WriteLine("jao");
        //        }
        //        else
        //        {
        //            Alert("Fel", "Kunnde inte ansluta till servern\n\nstatus code: " + (int)response.StatusCode, "ok");
        //            Label_Loading.IsVisible = false;
        //            ActivityIndicatior_WaitingForServer.IsRunning = false;
        //        }
        //    }
        //    catch
        //    {
        //        Alert("Fel", "Kunnde inte ansluta till servern", "ok");
        //        Label_Loading.IsVisible = false;
        //        ActivityIndicatior_WaitingForServer.IsRunning = false;
        //    }
        //}


        async void ReciveRecipeMetas(int start)
        {
            var json = JsonConvert.SerializeObject(myIngredients);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(RamseyApi.V2.Recipe.Suggest + "?start=" + start.ToString(), content);
                Analytics.TrackEvent("reciveRecipeMetasResponse", new Dictionary<string, string> { { "reciveRecipeMetasResponseStatusCode", response.StatusCode.ToString() } });

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
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

        void XamlSetup()
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
                        FrameHeight = frameHeight,
                        CoverageMessage = "Du har " + ((int)(x.Coverage * 100)).ToString() + "%  av alla ingredienser",
                        ShowCoverageMessage = !nameSearching,
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

            // Return it was an ad
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
            if (nameSearching)
                ReciveRecipeMetasFromName(recipeMetas.Count);
            else
                ReciveRecipeMetas(recipeMetas.Count);
        }


        async void ReciveRecipeMetasFromName(int start, int id=-1)
        {

            try
            {
                var json = JsonConvert.SerializeObject(new List<RecipeMetaDto>());
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(RamseyApi.V2.Recipe.Text + "?search=" + searchWord + "&start=" + start, content);



                Analytics.TrackEvent("reciveRecipeMetasResponseFromNameSearch", new Dictionary<string, string> { { "reciveRecipeMetasResponseStatusCode", response.StatusCode.ToString() } });

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var recivedRecipeMetas = JsonConvert.DeserializeObject<List<RecipeMetaDtoV2>>(result);

                    // return if this is not the latest search
                    if (id != -1 && id != latestId)
                        return;

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
        int nextId = 0;
        int latestId = 0;
        private void SearchBar_RecipeSearching_TextChanged(object sender, TextChangedEventArgs e)
        {
            var id = nextId;
            latestId = nextId;

            Label_Message.IsVisible = false;
            Label_Message.IsEnabled = false;

            Label_Loading.IsEnabled = true;
            Label_Loading.IsVisible = true;
            ActivityIndicatior_WaitingForServer.IsRunning = true;

            if (recipeMetaModels != null && recipeMetaModels.Count > 0)
                ListView_Recipes.ScrollTo(((List<RecipeMetaModel>)ListView_Recipes.ItemsSource)[0], ScrollToPosition.Center, false);

            searchWord = SearchBar_RecipeSearching.Text;
            
            ReciveRecipeMetasFromName(0, id);
        }
    }

}