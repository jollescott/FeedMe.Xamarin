using FeedMe.Classes;
using Newtonsoft.Json;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class shoppingListPage : ContentPage
    {
        private readonly HttpClient httpClient = new HttpClient();
        private List<IngredientDtoV2> searchIngredients = new List<IngredientDtoV2>();
        private readonly List<IngredientDtoV2> shoppingListIngredients = new List<IngredientDtoV2>();
        private readonly bool state = false;
        private readonly List<IconTestModel> icons_testList = new List<IconTestModel>();
        //List<IconTestModel> icons_testList2 = new List<IconTestModel>();

        public Color TestColor { get; } = Color.Red;
        public ObservableCollection<string> TestIcons { get; set; } = new ObservableCollection<string> { "md-add", "md-remove" };


        public shoppingListPage()
        {
            InitializeComponent();

            BindingContext = this;

            string savedIngredients = User.User.ShoppingListIngredients;
            if (savedIngredients != null && savedIngredients != "")
            {
                shoppingListIngredients = JsonConvert.DeserializeObject<List<IngredientDtoV2>>(savedIngredients);
            }


            //TestIcons.Add("md-add");
            //TestIcons.Add("md-add");
            //TestIcons.Add("md-add");
            //TestIcons.Add("md-remove");


            XamlSetup();
        }

        private void XamlSetup()
        {
            //ListView_ShoppingList.BackgroundColor = Constants.AppColor.lightGray;
            //ListView_ShoppingList.ItemsSource =
            StackLayout_main.Padding = Constants.padding2;
            ListView_myIngredients.RowHeight = Constants.textHeight;
            UpdateShoppingListListView(shoppingListIngredients);
        }

        // --------------------------------------------- SPAGHETTI ---------------------------------------------


        private void UpdateSearchIngreadientsListView(List<IngredientDtoV2> ingredients)
        {
            try
            {
                List<ListItem> items = new List<ListItem>();
                foreach (IngredientDtoV2 ingredient in ingredients)
                {
                    items.Add(new ListItem
                    {
                        Name = ingredient.IngredientName,
                        //IconSource = "md-remove-shopping-cart",   // FUNKAR INTE aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa :(
                        Color = (Sorting.IngredientExistsInList(ingredient, shoppingListIngredients)) ? Color.Black : Color.FromHex("#00CC66")
                    });

                }
                ListView_SearchIngredients.ItemsSource = items;
            }
            catch (Exception _e)
            {
                Console.WriteLine(_e);
            }
        }

        private void UpdateShoppingListListView(List<IngredientDtoV2> ingredients)
        {
            List<IngredientDtoV2> itemsorce = new List<IngredientDtoV2>();
            foreach (IngredientDtoV2 ingredient in ingredients)
            {
                itemsorce.Insert(0, ingredient);
            }

            ListView_myIngredients.ItemsSource = itemsorce;

            Sorting.ResizeListView(ListView_myIngredients, shoppingListIngredients.Count);
        }


        // --------------------------------------------- REQUESTS ---------------------------------------------------


        private async void GET_ingredientDtos(string search)
        {
            try
            {
                //HttpResponseMessage response = _httpClient.GetAsync(_adress).ConfigureAwait(false).GetAwaiter().GetResult();
                //HttpResponseMessage response = _httpClient.GetAsync(_adress).GetAwaiter().GetResult();
                HttpResponseMessage response = await httpClient.GetAsync(RamseyApi.V2.Ingredient.Suggest + "?search=" + search);


                if (response.IsSuccessStatusCode)
                {
                    //await DisplayAlert("success", "succeess", "ok");
                    string result = await response.Content.ReadAsStringAsync();
                    searchIngredients = Sorting.SortIngredientsByNameLenght(JsonConvert.DeserializeObject<List<IngredientDtoV2>>(result));
                    UpdateSearchIngreadientsListView(searchIngredients);
                }
                else
                {
                    await DisplayAlert("Connection error", "Status code " + (int)response.StatusCode + ": " + response.StatusCode.ToString(), "ok");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("An error occurred", "Server conection failed", "ok");
            }
        }


        // ----------------------------------------- EVENTS -----------------------------------------------


        private bool searching = false;
        private void SearchBar_Ingredients_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchWord = SearchBar_Ingredients.Text.ToLower();

            if (searchWord.Length > 0)
            {
                if (!searching)
                {
                    searching = true;
                }

                GET_ingredientDtos(searchWord);
            }
            else
            {
                searching = false;
            }
        }

        private void ListView_myIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            shoppingListIngredients.Remove(ListView_myIngredients.SelectedItem as IngredientDtoV2);
            UpdateShoppingListListView(shoppingListIngredients);

            User.User.ShoppingListIngredients = JsonConvert.SerializeObject(shoppingListIngredients);
        }

        private void Button_AddIngredients_Clicked(object sender, EventArgs e)
        {
            Frame_Search.IsEnabled = true;
            Frame_Search.IsVisible = true;
            ScrollView_main.IsEnabled = false;
            ScrollView_main.IsVisible = false;
        }

        private void ListView_SearchIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            IngredientDtoV2 selectedIngredient = searchIngredients[((List<ListItem>)ListView_SearchIngredients.ItemsSource).IndexOf((ListItem)e.SelectedItem)];

            if (Sorting.IngredientExistsInList(selectedIngredient, shoppingListIngredients))
            {
                foreach (IngredientDtoV2 ingredient in shoppingListIngredients)
                {
                    if (ingredient.IngredientId == selectedIngredient.IngredientId)
                    {
                        shoppingListIngredients.Remove(ingredient);
                        break;
                    }
                }
            }
            else
            {
                shoppingListIngredients.Add(selectedIngredient);
            }
            UpdateSearchIngreadientsListView(searchIngredients);

            User.User.ShoppingListIngredients = JsonConvert.SerializeObject(shoppingListIngredients);
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            UpdateShoppingListListView(shoppingListIngredients);
            ScrollView_main.IsEnabled = true;
            ScrollView_main.IsVisible = true;
            Frame_Search.IsEnabled = false;
            Frame_Search.IsVisible = false;
            SearchBar_Ingredients.Text = "";
        }

        private void button_test_Clicked(object sender, EventArgs e)
        {
            //if (state)
            //{
            //    state = !state;
            //    UpdateIcons(1, "md-add");
            //}
            //else
            //{
            //    state = !state;
            //    UpdateIcons(1, "md-remove");
            //}
            //var icons_testList2 = new List<IconTestModel>();
            //icons_testList2.Add(new IconTestModel { Icon = "md-remove" });
            //icons_testList2.Add(new IconTestModel { Icon = "md-remove" });
            //icons_testList2.Add(new IconTestModel { Icon = "md-remove" });
            //icons_testList2.Add(new IconTestModel { Icon = "md-add" });
            //list_test.ItemsSource = icons_testList2;
            //int a = 0;
            //int b = 0;

            TestIcons[0] = "md-remove";
        }

        private void UpdateIcons(int index, string name)
        {
            List<IconTestModel> sorce = new List<IconTestModel>
            {
                new IconTestModel { Icon = name },
                new IconTestModel { Icon = name }
            };
            //list_test.ItemsSource = sorce;
            list_test.ItemsSource = icons_testList;
        }
    }

    internal class IconTestModel
    {
        public string Icon { get; set; }
    }


}