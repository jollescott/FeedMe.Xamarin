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
        public bool SearchModeIncluded { get; set; }

        bool noIngredientsError;
        public bool NoIngredientsError
        {
            get { return noIngredientsError; }
            set
            {
                noIngredientsError = value;
                MyIngredientsErrorBorderCollor = (value) ? Color.Red : Constants.AppColor.green;
                Label_AddIngredients.TextColor = MyIngredientsErrorBorderCollor;
            }
        }
        public Color MyIngredientsErrorBorderCollor { get; set; } 

        public List<IngredientDtoV2> SearchIngredients { get; set; }
        public ObservableCollection<IngredientListModel> SearchIngredientModels { get; set; } = new ObservableCollection<IngredientListModel>();

        public List<IngredientDtoV2> MyIngredients { get; set; }
        public ObservableCollection<IngredientListModel> MyIngredientModels { get; set; } = new ObservableCollection<IngredientListModel>();

        public List<IngredientDtoV2> ExcludedIngredients { get; set; }
        public ObservableCollection<IngredientListModel> ExcludedIngredientModels { get; set; } = new ObservableCollection<IngredientListModel>();

        void MyIngredientsAdd(IngredientDtoV2 ingredient)
        {
            MyIngredients.Insert(0, ingredient);
            if (!InSearchWindow)
            {
                Sorting.ResizeListView(ListView_myIngredients, MyIngredients.Count);
                MyIngredientModels.Insert(0, new IngredientListModel() { Ingredient = ingredient });
            }
            SaveMyIngredients(MyIngredients);
        }
        void MyIngredientsRemoveAt(int index)
        {
            MyIngredients.RemoveAt(index);
            if (!InSearchWindow)
            {
                MyIngredientModels.RemoveAt(index);
                Sorting.ResizeListView(ListView_myIngredients, MyIngredients.Count);
            }
            SaveMyIngredients(MyIngredients);
        }

        void ExcludedIngredientsAdd(IngredientDtoV2 ingredient)
        {
            ingredient.Role = IngredientRole.Exclude;
            ExcludedIngredients.Insert(0, ingredient);
            if (!InSearchWindow)
            {
                Sorting.ResizeListView(ListView_excludedIngredients, ExcludedIngredients.Count);
                ExcludedIngredientModels.Insert(0, new IngredientListModel() { Ingredient = ingredient });
            }
            SaveExcludedIngredients(ExcludedIngredients);
        }
        void ExcludedIngredientsRemoveAt(int index)
        {
            ExcludedIngredients.RemoveAt(index);
            if (!InSearchWindow)
            {
                ExcludedIngredientModels.RemoveAt(index);
                Sorting.ResizeListView(ListView_excludedIngredients, ExcludedIngredients.Count);
            }
            SaveExcludedIngredients(ExcludedIngredients);
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
            ExcludedIngredients = User.User.SavedExcludedIngredinets;
            foreach (var ingredient in ExcludedIngredients)
            {
                ExcludedIngredientModels.Add(new IngredientListModel() { Ingredient = ingredient, IsAdded = true });
            }

            NoIngredientsError = false;

            XamlSetup();

            BindingContext = this;
        }


        void XamlSetup()
        {
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

            Label_MyIgredients.HeightRequest = Constants.textHeight;

            ListView_myIngredients.BackgroundColor = Constants.AppColor.lightGray;
            ListView_myIngredients.RowHeight = Constants.textHeight;
            ListView_excludedIngredients.BackgroundColor = Constants.AppColor.lightGray;
            ListView_excludedIngredients.RowHeight = Constants.textHeight;
            //ListView_myIngredients.HeightRequest = Constants.textHeight;

            Sorting.ResizeListView(ListView_myIngredients, MyIngredients.Count);
            Sorting.ResizeListView(ListView_excludedIngredients, ExcludedIngredients.Count);
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

        void GenerateExcludedIngredientModels()
        {
            ExcludedIngredientModels.Clear();
            foreach (var ingredient in ExcludedIngredients)
            {
                ExcludedIngredientModels.Add(new IngredientListModel
                {
                    Ingredient = ingredient,
                    IsAdded = true
                });
            }
            Sorting.ResizeListView(ListView_excludedIngredients, ExcludedIngredients.Count);
        }


        // --------------------------------------------- ASYNC SPAGHETTI ---------------------------------------------------


        async void Alert(string title, string message, string cancel)
        {
            await DisplayAlert(title, message, cancel);
        }

        async void GotoMealsListPage()
        {
            var ingredents = new List<IngredientDtoV2>();
            ingredents.AddRange(MyIngredients);
            ingredents.AddRange(ExcludedIngredients);
            await Navigation.PushAsync(new MealsListPage(ingredents) { Title = "Recept" });
        }

        async void SaveMyIngredients(List<IngredientDtoV2> ingredients)
        {
            await Task.Factory.StartNew(() => User.User.SavedIngredinets = ingredients);
        }

        async void SaveExcludedIngredients(List<IngredientDtoV2> ingredients)
        {
            await Task.Factory.StartNew(() => User.User.SavedExcludedIngredinets = ingredients);
        }


        // --------------------------------------------- REQUESTS ---------------------------------------------------

        
        // Recive ingredentDtos from the server and update lists
        async void GET_ingredientDtos(string search)
        {
            bool isLoading = true;
            ActivityIndicator_Ingredients.IsRunning = true;
            await Task.Factory.StartNew(() => SearchIngredients = ingredientsSearching.Search(search, out isLoading));

            SearchIngredientModels.Clear();

            if (SearchModeIncluded)
            {
                foreach (var ingredient in SearchIngredients)
                {
                    SearchIngredientModels.Add(new IngredientListModel()
                    {
                        Ingredient = ingredient,
                        IsAdded = (MyIngredients.Any(p => p.IngredientId == ingredient.IngredientId)) ? true : false
                    });
                }
            }
            else
            {
                foreach (var ingredient in SearchIngredients)
                {
                    SearchIngredientModels.Add(new IngredientListModel()
                    {
                        Ingredient = ingredient,
                        IsAdded = (ExcludedIngredients.Any(p => p.IngredientId == ingredient.IngredientId)) ? true : false,
                        AddedIcon = Constants.ExcludeIngredientCheckIcon,
                        AddedColor = Color.Red
                    });
                }
            }

            if (!isLoading)
                ActivityIndicator_Ingredients.IsRunning = false;

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

            SearchIngredientModels[index] = new IngredientListModel()
            {
                Ingredient = SearchIngredientModels[index].Ingredient,
                IsAdded = !SearchIngredientModels[index].IsAdded,
                AddedColor = SearchIngredientModels[index].AddedColor,
                AddedIcon = SearchIngredientModels[index].AddedIcon
            };

            if (SearchModeIncluded)
            {
                if (MyIngredients.Any(p => p.IngredientId == selectedIngredient.IngredientId))
                    MyIngredientsRemoveAt(MyIngredients.FindIndex(p => p.IngredientId == selectedIngredient.IngredientId));

                else
                    MyIngredientsAdd(selectedIngredient);
            }
            else
            {
                if (ExcludedIngredients.Any(p => p.IngredientId == selectedIngredient.IngredientId))
                    ExcludedIngredientsRemoveAt(ExcludedIngredients.FindIndex(p => p.IngredientId == selectedIngredient.IngredientId));

                else
                    ExcludedIngredientsAdd(selectedIngredient);
            }
        }

        // Searching
        private void SearchBar_Ingredients_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBar_Ingredients.Text == null)
                return;

            string searchWord = SearchBar_Ingredients.Text.ToLower();

            if (searchWord.Length > 0)
                GET_ingredientDtos(searchWord);
        }

        // Clicked in myIngredients list
        private void ListView_myIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MyIngredientsRemoveAt(MyIngredientModels.IndexOf(ListView_myIngredients.SelectedItem as IngredientListModel));
        }

        // Clicked in excludedIngredients list
        private void ListView_excludedIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ExcludedIngredientsRemoveAt(ExcludedIngredientModels.IndexOf(ListView_excludedIngredients.SelectedItem as IngredientListModel));
        }

        // Clicked go to recipes button
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (MyIngredients.Count > 0)
                GotoMealsListPage();
            else
            {
                NoIngredientsError = true;
                Alert("Ingredienser saknas", "Du måste lägga till ingredienser för att kunna söka efter recept", "ok");
            }
        }

        // Open search window included
        private void Button_OpenIngredientsSearch_Clicked(object sender, EventArgs e)
        {
            NoIngredientsError = false;
            SearchBar_Ingredients.Placeholder = "Lägg till ingredienser";
            Grid_IngredientSearchView.IsVisible = true;
            Grid_IngredientSearchView.IsEnabled = true;
            ScrollView_MainView.IsVisible = false;
            ScrollView_MainView.IsEnabled = false;
            InSearchWindow = true;
            SearchModeIncluded = true;
        }

        // Open search window excluded
        private void Button_OpenExcludedIngredientsSearch_Clicked(object sender, EventArgs e)
        {
            SearchBar_Ingredients.Placeholder = "Exkludera ingredienser";
            Grid_IngredientSearchView.IsVisible = true;
            Grid_IngredientSearchView.IsEnabled = true;
            ScrollView_MainView.IsVisible = false;
            ScrollView_MainView.IsEnabled = false;
            InSearchWindow = true;
            SearchModeIncluded = false;
        }

        // Close search window included
        private void Button_CloseIngredientsSearch_Clicked(object sender, EventArgs e)
        {
            ScrollView_MainView.IsVisible = true;
            ScrollView_MainView.IsEnabled = true;
            Grid_IngredientSearchView.IsVisible = false;
            Grid_IngredientSearchView.IsEnabled = false;
            SearchBar_Ingredients.Text = string.Empty;
            InSearchWindow = false;
            if (SearchModeIncluded)
                Task.Factory.StartNew(() => GenerateMyIngredientModels());
            else
                Task.Factory.StartNew(() => GenerateExcludedIngredientModels());

            SearchIngredients.Clear();
            SearchIngredientModels.Clear();
        }

        private void TapGestureRecognizer_Tapped_MyIngredientsHelp(object sender, EventArgs e)
        {
            Alert("Mina Ingredienser", "Klicka på “Lägg till” knappen för att lägga till ingredienser du har i ditt kök.", "ok");
        }

        private void TapGestureRecognizer_Tapped_ExcludedIngredientsHelp(object sender, EventArgs e)
        {
            Alert("Uteslutna Ingredienser", "Här kan du klicka på “Lägg till” knappen för att utesluta ingredienser du inte vill ha i dina recept.", "ok");
        }
    }
}
