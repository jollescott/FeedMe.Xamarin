using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

using Newtonsoft.Json;
using FeedMe.Models;
using System.Linq;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MealsListPage : ContentPage
    {
        List<RecipeMetaModel> recipes;
        HttpClient httpClient = new HttpClient();
        public MealsListPage(List<RecipeMetaDto> recipes_)
        {
            InitializeComponent();

            var models = recipes_.Select(x => new RecipeMetaModel
            {
                Image = x.Image,
                Name = x.Name,
                Owner = x.Owner,
                OwnerLogo = x.OwnerLogo,
                Source = x.Source,
                RecipeID = x.RecipeID
            }).ToList();

            models.Where((x, i) => (i + 1) % 5 == 0).ForEach(x => x.IsAd = true);

            recipes = models;
            XamlSetup();
        }

        void XamlSetup()
        {
            BackgroundColor = Color.LightGray;
            ListView_Recipes.BackgroundColor = Color.Transparent;
            ListView_Recipes.RowHeight = Convert.ToInt32(Application.Current.MainPage.Width * 3 / 5);
            ListView_Recipes.ItemsSource = recipes;
        }

        //Recipe selected
        private void ListView_Recipes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //object selected = ListView_Recipes.SelectedItem;
            //((ListView)sender).SelectedItem = null;

            var selected = ListView_Recipes.SelectedItem;
            if (selected != null)
            {
                int index = recipes.IndexOf(selected);

                GET_recipeDto(Constants.recipe_retrive + recipes[index].RecipeID);
            }
        }

        //Next page
        async void gotoRecipePage(RecipeDto meal)
        {
            await Navigation.PushAsync(new RecipePage(meal) { Title = meal.Name });

            ListView_Recipes.SelectedItem = null;
        }

        //Navigation back button
        async private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void GET_recipeDto(string _adress)
        {
            try
            {


                HttpResponseMessage response = await httpClient.GetAsync(_adress);

                if (response.IsSuccessStatusCode)
                {
                    //await DisplayAlert("success", "succeess", "ok");
                    var result = await response.Content.ReadAsStringAsync();
                    RecipeDto recipe = JsonConvert.DeserializeObject<RecipeDto>(result);

                    gotoRecipePage(recipe);
                }
                else
                {
                    await DisplayAlert("Response error", "Status code " + (int)response.StatusCode + ": " + response.StatusCode.ToString(), "ok");
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("An error occurred", e.Message, "ok");
            }
        }
    }

}