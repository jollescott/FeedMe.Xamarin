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
using Ramsey.Shared.Misc;
using Ramsey.Shared.Dto.V2;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using FeedMe.Models;
using System.Linq;

namespace FeedMe
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient httpClient = new HttpClient();
        IngredientsSearching ingredientsSearching = new IngredientsSearching();

        public bool InSearchWindow { get; set; } = false;

        public List<IngredientDtoV2> SearchIngredients { get; set; }
        public ObservableCollection<IngredientListModel> SearchIngredientModels { get; set; } = new ObservableCollection<IngredientListModel>();

        public List<IngredientDtoV2> MyIngredients { get; set; }
        public ObservableCollection<IngredientListModel> MyIngredientModels { get; set; } = new ObservableCollection<IngredientListModel>();

        void MyIngredientsAdd(IngredientDtoV2 ingredient)
        {
            MyIngredients.Insert(0, ingredient);
            if (!InSearchWindow)
            {
                Sorting.ResizeListView(ListView_myIngredients, MyIngredients.Count);
                MyIngredientModels.Insert(0, new IngredientListModel() { Ingredient = ingredient });
            }
            SaveIngredients(MyIngredients);
        }
        void MyIngredientsRemoveAt(int index)
        {
            MyIngredients.RemoveAt(index);
            if (!InSearchWindow)
            {
                MyIngredientModels.RemoveAt(index);
                Sorting.ResizeListView(ListView_myIngredients, MyIngredients.Count);
            }
            SaveIngredients(MyIngredients);
        }

        public MainPage()
        {
            InitializeComponent();

            SearchIngredients = new List<IngredientDtoV2>();
            MyIngredients = User.User.SavedIngredinets;
            foreach (var ingredient in MyIngredients)
            {
                MyIngredientModels.Add(new IngredientListModel() { Ingredient = ingredient, IsAdded = true});
            }

            XamlSetup();

            BindingContext = this;
        }


        void XamlSetup()
        {
            //BackgroundImage = "background.jpg";

            //Search

            //Frame_Search.BackgroundColor = Constants.AppColor.green;

            //StackLayout_Search.Padding = new Thickness(Constants.padding2, 0, Constants.padding2, Constants.padding2);

            SearchBar_Ingredients.BackgroundColor = Color.White;
            SearchBar_Ingredients.HeightRequest = Constants.textHeight;

            ListView_SearchIngredients.RowHeight = Constants.textHeight;


            //Main
            
            StackLayout_main.Padding = Constants.padding2;
            StackLayout_main.Spacing = Constants.padding2;

            Button_FeedMe.TextColor = Constants.AppColor.text_white;
            Button_FeedMe.FontSize = Constants.fontSize1;
            Button_FeedMe.BackgroundColor = Constants.AppColor.green;
            //Button_FeedMe.CornerRadius = Constants.cornerRadius1;

            //Frame_MyIngredients.CornerRadius = Constants.cornerRadius1;

            Label_MyIgredients.FontSize = Constants.fontSize2;
            Label_MyIgredients.HeightRequest = Constants.textHeight;

            ListView_myIngredients.BackgroundColor = Constants.AppColor.lightGray;
            ListView_myIngredients.RowHeight = Constants.textHeight;
            //ListView_myIngredients.HeightRequest = Constants.textHeight;

            Sorting.ResizeListView(ListView_myIngredients, MyIngredients.Count);
        }

        // --------------------------------------------- SPAGHETTI ---------------------------------------------------


        void GenerateMyIngredientModels()
        {
            MyIngredientModels.Clear();
            foreach (var ingredient in MyIngredients)
            {
                MyIngredientModels.Add(new IngredientListModel
                {
                    Ingredient = ingredient,
                    IsAdded = true
                });
            }
            Sorting.ResizeListView(ListView_myIngredients, MyIngredients.Count);
        }


        // --------------------------------------------- ASYNC SPAGHETTI ---------------------------------------------------


        async void Alert(string title, string message, string cancel)
        {
            await DisplayAlert(title, message, cancel);
        }

        async void GotoMealsListPage()
        {
            await Navigation.PushAsync(new MealsListPage() { Title = "Recept" });
        }

        async void SaveIngredients(List<IngredientDtoV2> ingredients)
        {
            await Task.Factory.StartNew(() => User.User.SavedIngredinets = ingredients);
        }


        // --------------------------------------------- REQUESTS ---------------------------------------------------

        
        // Recive ingredentDtos from the server and update lists
        async void GET_ingredientDtos(string search)
        {
            await Task.Factory.StartNew(() => SearchIngredients = ingredientsSearching.Search(search));

            SearchIngredientModels.Clear();
            foreach (var ingredient in SearchIngredients)
            {
                SearchIngredientModels.Add(new IngredientListModel()
                {
                    Ingredient = ingredient,
                    IsAdded = (MyIngredients.Any(p => p.IngredientId == ingredient.IngredientId)) ? true : false
                });
            }
        }


        // --------------------------------------------- EVENTS ---------------------------------------------------


        // Klicked ingredient in search window
        private void ListView_SearchIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ListView_SearchIngredients.SelectedItem == null)
                return;

            int index = ((ObservableCollection<IngredientListModel>)ListView_SearchIngredients.ItemsSource).IndexOf((IngredientListModel)e.SelectedItem);
            ((ListView)sender).SelectedItem = null;
            IngredientDtoV2 selectedIngredient = SearchIngredients[index];

            SearchIngredientModels[index] = new IngredientListModel() { Ingredient = SearchIngredientModels[index].Ingredient, IsAdded = !SearchIngredientModels[index].IsAdded };

            if (MyIngredients.Any(p => p.IngredientId == selectedIngredient.IngredientId))
                MyIngredientsRemoveAt(MyIngredients.FindIndex(p => p.IngredientId == selectedIngredient.IngredientId));

            else
                MyIngredientsAdd(selectedIngredient);
        }

        // Searching
        private void SearchBar_Ingredients_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchWord = SearchBar_Ingredients.Text.ToLower();

            if (searchWord.Length > 0)
                GET_ingredientDtos(searchWord);
        }

        // Klicked in myIngredients list
        private void ListView_myIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MyIngredientsRemoveAt(MyIngredientModels.IndexOf(ListView_myIngredients.SelectedItem as IngredientListModel));
        }

        // Klicked go to recipes button
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (MyIngredients.Count > 0)
                GotoMealsListPage();
            else
                Alert("Ingredienser saknas", "Du måste lägga till ingredienser för att kunna söka efter recept", "ok");
        }

        // Open search window
        private void Button_OpenIngredientsSearch_Clicked(object sender, EventArgs e)
        {
            Grid_IngredientSearchView.IsVisible = true;
            Grid_IngredientSearchView.IsEnabled = true;
            ScrollView_MainView.IsVisible = false;
            ScrollView_MainView.IsEnabled = false;
            InSearchWindow = true;
        }


        // Close search window
        private void Button_CloseIngredientsSearch_Clicked(object sender, EventArgs e)
        {
            ScrollView_MainView.IsVisible = true;
            ScrollView_MainView.IsEnabled = true;
            Grid_IngredientSearchView.IsVisible = false;
            Grid_IngredientSearchView.IsEnabled = false;
            SearchBar_Ingredients.Text = string.Empty;
            InSearchWindow = false;
            Task.Factory.StartNew(() => GenerateMyIngredientModels());
        }
    }
}
