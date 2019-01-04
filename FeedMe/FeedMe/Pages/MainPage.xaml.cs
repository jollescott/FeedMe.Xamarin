using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using Ramsey.Shared.Dto;
using Ramsey.Shared.Extensions;
using FeedMe.Classes;

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
            //BackgroundImage = "background.jpg";

            //Search

            Frame_Search.BackgroundColor = Constants.AppColor.green;

            StackLayout_Search.Padding = new Thickness(Constants.padding2, 0, Constants.padding2, Constants.padding2);

            SearchBar_Ingredients.BackgroundColor = Color.White;
            SearchBar_Ingredients.HeightRequest = Constants.textHeight;

            ListView_SearchIngredients.BackgroundColor = Constants.AppColor.lightGray;
            ListView_SearchIngredients.RowHeight = Constants.textHeight;


            //Main
            
            StackLayout_main.Padding = Constants.padding2;
            StackLayout_main.Spacing = Constants.padding2;

            Button_FeedMe.TextColor = Constants.AppColor.text_white;
            Button_FeedMe.FontSize = Constants.fontSize1;
            Button_FeedMe.BackgroundColor = Constants.AppColor.green;
            Button_FeedMe.CornerRadius = Constants.cornerRadius1;

            Frame_MyIngredients.CornerRadius = Constants.cornerRadius1;

            Label_MyIgredients.FontSize = Constants.fontSize2;
            Label_MyIgredients.HeightRequest = Constants.textHeight;

            ListView_myIngredients.BackgroundColor = Constants.AppColor.lightGray;
            ListView_myIngredients.RowHeight = Constants.textHeight;
            ListView_myIngredients.HeightRequest = Constants.textHeight;

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

            ResizeListView(ListView_myIngredients, myIngredients.Count);
        }

        private void UpdateSearchIngreadientsListView(List<IngredientDto> ingredients)
        {
            List<ListItem> items = new List<ListItem>();
            foreach (IngredientDto ingredient in ingredients)
            {
                items.Add(new ListItem {
                    Name = ingredient.IngredientId,
                    IconSource = (ExistsIn(ingredient, myIngredients)) ? "icon_remove.png" : "icon_add.png"
            });
            }
            ListView_SearchIngredients.ItemsSource = items;
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

        void ResizeListView(ListView listView, int length)
        {
            if (length < 1)
            {
                length = 1;
            }
            double height = length * Constants.textHeight;
            double adjust = 1 + (length - 1) * 0.36;
            ListView_myIngredients.HeightRequest = height + adjust;
        }

        void HideSearchView()
        {
            ListView_SearchIngredients.IsVisible = false;
            ListView_SearchIngredients.IsEnabled = false;
        }

        void ShowSearchView()
        {
            ListView_SearchIngredients.HeightRequest = StackLayout_all.Height - SearchBar_Ingredients.Height - Constants.padding2;
            ListView_SearchIngredients.MinimumHeightRequest = StackLayout_all.Height - SearchBar_Ingredients.Height - Constants.padding2;
            ListView_SearchIngredients.IsVisible = true;
            ListView_SearchIngredients.IsEnabled = true;
        }

        List<IngredientDto> SortIngredientsByLenght(List<IngredientDto> ingredients)
        {
            for (int i = 1; i < ingredients.Count; i++)
            {
                int j = 0;
                while (ingredients[i].IngredientId.Length > ingredients[j].IngredientId.Length)
                {
                    j++;
                }
                IngredientDto item = ingredients[i];
                ingredients.RemoveAt(i);
                ingredients.Insert(j, item);
            }

            return ingredients;
        }

        bool ExistsIn(IngredientDto searchItem, List<IngredientDto> list)
        {
            foreach (IngredientDto item in list)
            {
                if (item.IngredientId == searchItem.IngredientId)
                {
                    return true;
                }
            }
            return false;
        }


        // --------------------------------------------- ASYNC SPAGHETTI ---------------------------------------------------


        async void Alert(string title, string message, string cancel)
        {
            await DisplayAlert(title, message, cancel);
        }

        async void gotoMealsListPage(List<RecipeMetaDto> recipeDtos)
        {
            await Navigation.PushAsync(new MealsListPage(recipeDtos, myIngredients) { Title = "Bon Appétit" });
        }


        // --------------------------------------------- REQUESTS ---------------------------------------------------


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
                    searchIngredients = SortIngredientsByLenght(JsonConvert.DeserializeObject<List<IngredientDto>>(result));
                    UpdateSearchIngreadientsListView(searchIngredients);
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

        async void POST_recipeMetas(List<IngredientDto> ingredientDtos)
        {
            var json = JsonConvert.SerializeObject(ingredientDtos); //skicka ingredientDto

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage respone = await httpClient.PostAsync(Constants.recipe_suggest, content);

                if (respone.IsSuccessStatusCode)
                {
                    var result = await respone.Content.ReadAsStringAsync(); //ReadAsSwedishStringAsync();
                    var recipes = JsonConvert.DeserializeObject<List<RecipeMetaDto>>(result);

                    gotoMealsListPage(recipes);
                }
                else
                {
                    await DisplayAlert("Response error", "Status code " + (int)respone.StatusCode + ": " + respone.StatusCode.ToString(), "ok");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("An error occurred", "Server conection failed", "ok");
            }

            Button_FeedMe.IsEnabled = true;
            Button_FeedMe.BackgroundColor = Constants.AppColor.green;
            Button_FeedMe.Text = "FeedMe";
        }


        // --------------------------------------------- EVENTS ---------------------------------------------------


        private void ListView_SearchIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            IngredientDto selectedIngredient = searchIngredients[((List<ListItem>)ListView_SearchIngredients.ItemsSource).IndexOf((ListItem)e.SelectedItem)];
            if (ExistsIn(selectedIngredient, myIngredients))
            {
                foreach (IngredientDto ingredient in myIngredients)
                {
                    if (ingredient.IngredientId == selectedIngredient.IngredientId)
                    {
                        myIngredients.Remove(ingredient);
                        break;
                    }
                }
            }
            else
            {
                myIngredients.Add(selectedIngredient);
            }
            UpdateSearchIngreadientsListView(searchIngredients);
            UpdateMyIngreadientsListView(myIngredients);
        }

        bool searching = false;
        private void SearchBar_Ingredients_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchWord = SearchBar_Ingredients.Text.ToLower();

            if (searchWord.Length > 0)
            {
                if (!searching)
                {
                    searching = true;
                    ShowSearchView();
                }

                string adress = Constants.ingredient_search + searchWord;
                GET_ingredientDtos(adress);
            }
            else{
                searching = false;
                HideSearchView();
            }
        }

        private void ListView_myIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            myIngredients.Remove(ListView_myIngredients.SelectedItem as IngredientDto);
            UpdateMyIngreadientsListView(myIngredients);
        }

        // Klicked FeedMe
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (myIngredients.Count > 0)
            {
                Button_FeedMe.IsEnabled = false;
                Button_FeedMe.BackgroundColor = Color.Gray;
                Button_FeedMe.Text = "Laddar...";

                POST_recipeMetas(myIngredients);
            }
            else
            {
                Alert("Ingredienser saknas", "Du måste lägga till ingredienser för att kunna söka efter recept", "ok");
            }
        }
    }
}
