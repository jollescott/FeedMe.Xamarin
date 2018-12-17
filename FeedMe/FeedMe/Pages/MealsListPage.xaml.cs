using Ramsey.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

using Newtonsoft.Json;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MealsListPage : ContentPage
	{
        List<RecipeMetaDto> recipes;
        HttpClient httpClient = new HttpClient();
		public MealsListPage (List<RecipeMetaDto> recipes_)
		{
            InitializeComponent();

            recipes = recipes_;

            XamlSetup();
        }

        void XamlSetup()
        {
            BackgroundColor = Color.LightGray;
            ListView_Recipes.BackgroundColor = Color.Transparent;
            ListView_Recipes.RowHeight = Convert.ToInt32(Application.Current.MainPage.Width * 3/5);
            ListView_Recipes.Margin = new Thickness(0, Constants.padding2 / 2, 0, Constants.padding1);
            //Frame_List.Margin = Constants.padding2; //c# hittar inte xml namn?

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
                    var result = await response.Content.ReadAsStringAsync();
                    RecipeDto recipe = JsonConvert.DeserializeObject<RecipeDto>(result);

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