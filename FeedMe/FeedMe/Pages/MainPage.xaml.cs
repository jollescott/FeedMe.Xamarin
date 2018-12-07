using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using Ramsey.Shared.Dto;

namespace FeedMe
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient httpClient = new HttpClient();

        List<IngredientDto> myIngredients = new List<IngredientDto>();
        List<IngredientDto> baseIngredients = new List<IngredientDto>();


        public MainPage()
        {
            InitializeComponent();

            XamlSetup();
        }


        void XamlSetup()
        {
            BackgroundImage = "background.jpg";

            StackLayout_all.Margin = Constants.padding2;
            StackLayout_all.Spacing = Constants.padding2;

            Entry_Ingredients.BackgroundColor = Constants.backgroundColor;

            ListView_myIngredients.BackgroundColor = Constants.backgroundColor;

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
        private void Button_Clicked(object sender, EventArgs e)
        {
            Button_FeedMe.IsEnabled = false;
            Button_FeedMe.BackgroundColor = Color.Gray;
            Button_FeedMe.Text = "Loading...";

            //List<string> ingredientsToPost = new List<string>();
            //foreach (IngredientDto ingredient in myIngredients)
            //{
            //    ingredientsToPost.Add(ingredient.Name);
            //}



            //PostIngredients(myIngredients);



            List<RecipeDto> list = new List<RecipeDto> {
                new RecipeDto
                {
                    Name = "Köttbullar och potatis",
                    Image = "food.jpg"
                },
                new RecipeDto
                {
                    Name = "Mat",
                    Image = "food.jpg"
                },
                new RecipeDto
                {
                    Name = "Test test test test test",
                    Image = "food.jpg"
                }
            };
            gotoMealsListPage(list);
        }


        async void gotoMealsListPage(List<RecipeDto> recipeDtos)
        {
            await Navigation.PushAsync(new MealsListPage(recipeDtos) { Title = "Bon Appétit" });
        }


        async void PostIngredients(List<IngredientDto> ingredientDtos)
        {
            //List<string> stringList = new List<string> { "tomat", "pasta", "gurka", "kyckling"};

            string jsonstring = "[";
            foreach (IngredientDto ingredient in ingredientDtos)
            {
                jsonstring += "\"" + ingredient.Name + "\", ";
            }
            jsonstring = jsonstring.Remove(jsonstring.Length - 2) + "]";
            //                      "[   \"ing\", \"ing\", \"ing\"   ]"


            //var json = JsonConvert.SerializeObject(ingredientDtos); //skicka ingredientDto
            //var json = "[\"test\", \"test\"]";//JsonConvert.SerializeObject(stringList);

            var json = jsonstring;

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage respone = await httpClient.PostAsync(Constants.server_adress_recipe, content);

                if (respone.IsSuccessStatusCode)
                {
                    var result = await respone.Content.ReadAsStringAsync();
                    var recipes = JsonConvert.DeserializeObject<List<RecipeDto>>(result);

                    gotoMealsListPage(recipes);
                }
                else
                {
                    await DisplayAlert("An error occurred", "Status code " + (int)respone.StatusCode + ": " + respone.StatusCode.ToString(), "ok");
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


        private void ListView_myIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            myIngredients.Remove(ListView_myIngredients.SelectedItem as IngredientDto);
            UpdateMyIngreadientsListView(myIngredients);
        }


        private void Entry_Ingredients_Completed(object sender, EventArgs e)
        {
            string text = Entry_Ingredients.Text;
            if (text.Length > 1)
                text = char.ToUpper(text[0]) + text.Substring(1);

            myIngredients.Add(new IngredientDto() {Name = text});
            Entry_Ingredients.Text = "";
            UpdateMyIngreadientsListView(myIngredients);
        }


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
                strIngredients.Add(ingredient.Name);
            }
            return strIngredients;
        }

    }
}
