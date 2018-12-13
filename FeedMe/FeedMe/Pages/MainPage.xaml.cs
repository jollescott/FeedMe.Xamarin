using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Extensions;

namespace FeedMe
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient httpClient = new HttpClient();

        List<IngredientDto> searchIngredients = new List<IngredientDto>();
        List<IngredientDto> myIngredients = new List<IngredientDto>();


        public MainPage()
        {
            InitializeComponent();

            XamlSetup();
        }


        void XamlSetup()
        {
            BackgroundImage = "background.jpg";

            StackLayout_all.Padding = Constants.padding2;
            StackLayout_all.Spacing = Constants.padding2;

            Frame_SearchBar.BackgroundColor = Constants.mainColor1;

            SearchBar_Ingredients.BackgroundColor = Constants.backgroundColor;

            ListView_SearchIngredients.BackgroundColor = Color.White;

            //Label_MyIgredients.TextColor = Constants.textColor1;
            Label_MyIgredients.FontSize = Constants.fontSize2;
            Label_MyIgredients.Margin = Constants.padding3;

            ListView_myIngredients.BackgroundColor = Constants.backgroundColor;
            ListView_myIngredients.RowHeight = Convert.ToInt32(SearchBar_Ingredients.Height);

            Button_FeedMe.TextColor = Constants.textColor3;
            Button_FeedMe.FontSize = Constants.fontSize1;
            Button_FeedMe.BackgroundColor = Constants.mainColor2;
            Button_FeedMe.CornerRadius = Constants.buttonCornerRadius;
        }


        /*private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchWord = SearchBar_Ingredients.Text.ToLower();


            if (searchWord != "")
            {
                //http GET
                /*httpClient.GetAsync(Constants.server_adress + searchWord).ContinueWith(async (t) =>
                {
                    var response = t.Result;

                    var json = await response.Content.ReadAsStringAsync();
                    filterdEatebles = JsonConvert.DeserializeObject<List<IngredientDto>>(json, new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore });

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //Update xaml ListView
                        ListView_Ingredients.ItemsSource = filterdEatebles;
                    });
                });

            }
        }*/


        // Klicked FeedMe
        /*private void Button_Clicked(object sender, EventArgs e)
        {
            Button_FeedMe.IsEnabled = false;
            Button_FeedMe.BackgroundColor = Color.Gray;
            Button_FeedMe.Text = "Loading...";

            //List<string> ingredientsToPost = new List<string>();
            //foreach (IngredientDto ingredient in myIngredients)
            //{
            //    ingredientsToPost.Add(ingredient.Name);
            //}



            PostIngredients(myIngredients);



            //List<RecipeDto> list = new List<RecipeDto> {
            //    new RecipeDto
            //    {
            //        Name = "Köttbullar och potatis",
            //        Image = "food.jpg"
            //    },
            //    new RecipeDto
            //    {
            //        Name = "Mat",
            //        Image = "food.jpg"
            //    },
            //    new RecipeDto
            //    {
            //        Name = "Test test test test test",
            //        Image = "food.jpg"
            //    }
            //};
            //gotoMealsListPage(list);
        }*/


        async void PostIngredients(List<IngredientDto> ingredientDtos)
        {
            //string jsonstring = "[";
            //foreach (IngredientDto ingredient in ingredientDtos)
            //{
            //    jsonstring += "\"" + ingredient.IngredientId + "\", ";
            //}
            //jsonstring = jsonstring.Remove(jsonstring.Length - 2) + "]";

            //                      "[   \"ing\", \"ing\", \"ing\"   ]"


            var json = JsonConvert.SerializeObject(ingredientDtos); //skicka ingredientDto
            //var json = jsonstring;

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage respone = await httpClient.PostAsync(Constants.recipe_suggest, content);

                if (respone.IsSuccessStatusCode)
                {
                    var result = await respone.Content.ReadAsSwedishStringAsync();
                    var recipes = JsonConvert.DeserializeObject<List<RecipeMetaDto>>(result);

                    gotoMealsListPage(recipes);
                }
                else
                {
                    await DisplayAlert("Response error", "Status code " + (int)respone.StatusCode + ": " + respone.StatusCode.ToString(), "ok");
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("An error occurred", e.Message, "ok");
            }

            Button_FeedMe.IsEnabled = true;
            Button_FeedMe.BackgroundColor = Constants.mainColor1;
            Button_FeedMe.Text = "FeedMe";
        }


        // --------------------------------------------- SPAGHETTI ---------------------------------------------------



        private void UpdateMyIngreadientsListView(List<IngredientDto> ingredients)
        {
            List<IngredientDto> itemsorce = new List<IngredientDto>();
            foreach (IngredientDto ingredient in myIngredients)
            {
                itemsorce.Insert(0, ingredient);
            }

            ListView_myIngredients.ItemsSource = itemsorce;
        }

        private List<string> IngredientsToStrings(List<IngredientDto> ingredientDtos)
        {
            List<string> strIngredients = new List<string>();
            foreach (IngredientDto ingredient in ingredientDtos)
            {
                strIngredients.Add(ingredient.IngredientId);
            }
            return strIngredients;
        }

        async void GET_ingredientDtos(string _adress)
        {
            try
            {
                //HttpResponseMessage response = _httpClient.GetAsync(_adress).ConfigureAwait(false).GetAwaiter().GetResult();
                //HttpResponseMessage response = _httpClient.GetAsync(_adress).GetAwaiter().GetResult();
                HttpResponseMessage response = await httpClient.GetAsync(_adress);


                if (response.IsSuccessStatusCode)
                {
                    //await DisplayAlert("success", "succeess", "ok");
                    var result = await response.Content.ReadAsStringAsync();
                    searchIngredients = JsonConvert.DeserializeObject<List<IngredientDto>>(result);
                    ListView_SearchIngredients.ItemsSource = IngredientsToStrings(searchIngredients);
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

        async void POST_recipeMetas(List<IngredientDto> ingredientDtos)
        {
            var json = JsonConvert.SerializeObject(ingredientDtos); //skicka ingredientDto
            //var json = jsonstring;

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage respone = await httpClient.PostAsync(Constants.recipe_suggest, content);

                if (respone.IsSuccessStatusCode)
                {
                    var result = await respone.Content.ReadAsSwedishStringAsync();
                    var recipes = JsonConvert.DeserializeObject<List<RecipeMetaDto>>(result);

                    gotoMealsListPage(recipes);
                }
                else
                {
                    await DisplayAlert("Response error", "Status code " + (int)respone.StatusCode + ": " + respone.StatusCode.ToString(), "ok");
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("An error occurred", e.Message, "ok");
            }

            Button_FeedMe.IsEnabled = true;
            Button_FeedMe.BackgroundColor = Constants.mainColor1;
            Button_FeedMe.Text = "FeedMe";
        }


        async void gotoMealsListPage(List<RecipeMetaDto> recipeDtos)
        {
            await Navigation.PushAsync(new MealsListPage(recipeDtos) { Title = "Bon Appétit" });
        }

        void ResizeListView()
        {
            int height = Convert.ToInt32(myIngredients.Count * SearchBar_Ingredients.Height);
            double adjust = -4 * (myIngredients.Count - 1);
            ListView_myIngredients.HeightRequest = height + adjust;
        }


        // --------------------------------------------- EVENTS ---------------------------------------------------


        /*private void Entry_Ingredients_Completed(object sender, EventArgs e)
        {
            string text = Entry_Ingredients.Text;
            if (text.Length > 1)
                text = char.ToUpper(text[0]) + text.Substring(1);

            myIngredients.Add(new IngredientDto() { IngredientId = text });
            Entry_Ingredients.Text = "";
            UpdateMyIngreadientsListView(myIngredients);
        }*/

        private void ListView_SearchIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            myIngredients.Add(searchIngredients[IngredientsToStrings(searchIngredients).IndexOf(Convert.ToString(e.SelectedItem))]);
            UpdateMyIngreadientsListView(myIngredients);
        }

        private void SearchBar_Ingredients_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchWord = SearchBar_Ingredients.Text.ToLower();
            string adress = "https://ramsey.azurewebsites.net/ingredient/suggest?search=" + searchWord;

            GET_ingredientDtos(adress);
        }

        private void ListView_myIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            myIngredients.Remove(ListView_myIngredients.SelectedItem as IngredientDto);
            UpdateMyIngreadientsListView(myIngredients);
        }

        // Klicked FeedMe
        private void Button_Clicked(object sender, EventArgs e)
        {
            Button_FeedMe.IsEnabled = false;
            Button_FeedMe.BackgroundColor = Color.Gray;
            Button_FeedMe.Text = "Ladddar...";

            POST_recipeMetas(myIngredients);
        }

        private void ListView_myIngredients_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            ResizeListView();
        }
    }
}