﻿using System.Text;
using FeedMe.Models;
using FeedMe.Pages;
using Newtonsoft.Json;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Misc;

namespace FeedMe;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MealsListPage : ContentPage
{
    private readonly HttpClient httpClient = new();
    private readonly List<IngredientDtoV2> myIngredients;
    private readonly bool nameSearching;
    private readonly List<RecipeDtoV2> recipes;
    private readonly bool viewFavorites;

    private bool _canOpenNewRecipes = true;
    private int latestId;


    // Search
    private int nextId;
    private List<RecipeMetaModel> recipeMetaModels;
    private List<RecipeMetaDtoV2> recipeMetas;
    private string searchWord;

    public MealsListPage()
    {
        InitializeComponent();
        viewFavorites = true;

        LabelLoading.Text = "Laddar...";
        recipes = User.User.SavedRecipes;
        XamlSetup();

        if (recipes.Count < 1)
        {
            LabelMessage.Text = "Här sparas de recept du har gillat";
            LabelMessage.IsVisible = true;
        }
    }

    public MealsListPage(bool NameSearching)
    {
        InitializeComponent();
        nameSearching = true;
        LabelLoading.IsEnabled = false;
        LabelLoading.IsVisible = false;
        ActivityIndicatorWaitingForServer.IsRunning = false;
        FrameRecipeSearching.IsEnabled = true;
        FrameRecipeSearching.IsVisible = true;
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
        var json = JsonConvert.SerializeObject(myIngredients);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync(RamseyApi.V2.Recipe.Suggest + "?start=" + start, content);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var recivedRecipeMetas = JsonConvert.DeserializeObject<List<RecipeMetaDtoV2>>(result);
                if (recivedRecipeMetas.Count < 25)
                {
                    ButtonViewMoreRecipes.IsEnabled = false;
                    ButtonViewMoreRecipes.IsVisible = false;
                }
                else
                {
                    ButtonViewMoreRecipes.IsEnabled = true;
                    ButtonViewMoreRecipes.IsVisible = true;
                }

                recipeMetas.AddRange(recivedRecipeMetas);
                XamlSetup();
            }
            else
            {
                Alert("Fel", "Kunnde inte ansluta till servern\n\nstatus code: " + (int)response.StatusCode, "ok");
                LabelLoading.IsVisible = false;
                ActivityIndicatorWaitingForServer.IsRunning = false;
            }
        }
        catch
        {
            Alert("Fel", "Kunnde inte ansluta till servern", "ok");
            LabelLoading.IsVisible = false;
            ActivityIndicatorWaitingForServer.IsRunning = false;
        }
    }

    private void XamlSetup()
    {
        LabelLoading.IsEnabled = false;
        LabelLoading.IsVisible = false;

        var frameHeight = (int)(Application.Current.MainPage.Width * 0.8);

        if (viewFavorites)
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
        else
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
                    CoverageMessage = "Du har " + (int)(x.Coverage * 100) + "%  av alla ingredienser",
                    ShowCoverageMessage = !nameSearching,
                    LogoRadius = 40,
                    RecipeId = x.RecipeId
                };
            }).ToList();

        // Add ads
        for (var i = 0; i < recipeMetaModels.Count; i++)
            if (i % 4 == 0)
                recipeMetaModels.Insert(i, new RecipeMetaModel { IsAd = true });


        ListViewRecipes.BackgroundColor = Colors.Transparent;
        ListViewRecipes.ItemsSource = recipeMetaModels;

        ActivityIndicatorWaitingForServer.IsRunning = false;
        ActivityIndicatorWaitingForServerLoadingMoreRecipes.IsRunning = false;
    }

    //Recipe selected
    private void ListView_Recipes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selected = ListViewRecipes.SelectedItem;

        if (selected == null || !_canOpenNewRecipes) return;

        _canOpenNewRecipes = false;

        var selectedItemIndex = recipeMetaModels.IndexOf((RecipeMetaModel)selected);
        ListViewRecipes.SelectedItem = null;

        // Return it was an ad
        for (var i = 0; i < recipeMetaModels.Count; i += 4)
            if (selectedItemIndex == i)
                return;

        var index = selectedItemIndex - (int)(selectedItemIndex / 4f) - 1;

        if (viewFavorites)
            GotoRecipePage(recipes[index]);
        else
            GotoRecipePage(recipeMetas[index]);
    }

    //Next page
    private async void GotoRecipePage(RecipeMetaDtoV2 recipeMeta)
    {
        await Navigation.PushAsync(new RecipePage(recipeMeta) { Title = recipeMeta.Name });

        _canOpenNewRecipes = true;
    }

    //Next page (favorite)
    private async void GotoRecipePage(RecipeDtoV2 recipe)
    {
        await Navigation.PushAsync(new RecipePage(recipe) { Title = recipe.Name });

        _canOpenNewRecipes = true;
    }

    //Navigation back button
    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    //Load more recipes button
    private void Button_ViewMoreRecipes_Clicked(object sender, EventArgs e)
    {
        ActivityIndicatorWaitingForServerLoadingMoreRecipes.IsRunning = true;
        ButtonViewMoreRecipes.IsEnabled = false;
        ButtonViewMoreRecipes.IsVisible = false;
        if (nameSearching)
            ReciveRecipeMetasFromName(recipeMetas.Count);
        else
            ReciveRecipeMetas(recipeMetas.Count);
    }

    private async void ReciveRecipeMetasFromName(int start, int id = -1)
    {
        try
        {
            var json = JsonConvert.SerializeObject(new List<RecipeMetaDto>());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response =
                await httpClient.PostAsync(RamseyApi.V2.Recipe.Text + "?search=" + searchWord + "&start=" + start,
                    content);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var recivedRecipeMetas = JsonConvert.DeserializeObject<List<RecipeMetaDtoV2>>(result);

                // return if this is not the latest search
                if (id != -1 && id != latestId) return;

                if (start == 0)
                {
                    recipeMetas = new List<RecipeMetaDtoV2>();
                    recipeMetaModels = new List<RecipeMetaModel>();
                }

                if (recivedRecipeMetas.Count < 25)
                {
                    ButtonViewMoreRecipes.IsEnabled = false;
                    ButtonViewMoreRecipes.IsVisible = false;
                }
                else
                {
                    ButtonViewMoreRecipes.IsEnabled = true;
                    ButtonViewMoreRecipes.IsVisible = true;
                }

                recipeMetas.AddRange(recivedRecipeMetas);
                XamlSetup();

                if (recipeMetaModels.Count == 0)
                {
                    LabelMessage.Text = "Inga recept hittades";
                    LabelMessage.IsVisible = true;
                    LabelMessage.IsEnabled = true;
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
                LabelLoading.IsVisible = false;
                ActivityIndicatorWaitingForServer.IsRunning = false;
            }
        }
        catch
        {
            Alert("Fel", "Kunnde inte ansluta till servern", "ok");
            LabelLoading.IsVisible = false;
            ActivityIndicatorWaitingForServer.IsRunning = false;
        }
    }

    private void SearchBar_RecipeSearching_TextChanged(object sender, TextChangedEventArgs e)
    {
        var id = nextId;
        latestId = nextId;

        LabelMessage.IsVisible = false;
        LabelMessage.IsEnabled = false;

        LabelLoading.IsEnabled = true;
        LabelLoading.IsVisible = true;
        ActivityIndicatorWaitingForServer.IsRunning = true;

        if (recipeMetaModels != null && recipeMetaModels.Count > 0)
            ListViewRecipes.ScrollTo(((List<RecipeMetaModel>)ListViewRecipes.ItemsSource)[0], ScrollToPosition.Center,
                false);

        searchWord = SearchBarRecipeSearching.Text;

        ReciveRecipeMetasFromName(0, id);
    }
}