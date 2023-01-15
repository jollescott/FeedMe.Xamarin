using System.Collections.ObjectModel;
using FeedMe.Classes;
using FeedMe.Models;
using Ramsey.Shared.Dto.V2;

namespace FeedMe;

public partial class MainPage : ContentPage
{
    private readonly HttpClient httpClient = new();
    private readonly IngredientsSearching ingredientsSearching = new();

    private bool noIngredientsError;

    public MainPage()
    {
        InitializeComponent();

        SearchIngredients = new List<IngredientDtoV2>();
        MyIngredients = User.User.SavedIngredinets;
        foreach (var ingredient in MyIngredients)
            MyIngredientModels.Add(new IngredientListModel { Ingredient = ingredient, IsAdded = true });
        ExcludedIngredients = User.User.SavedExcludedIngredinets;
        foreach (var ingredient in ExcludedIngredients)
            ExcludedIngredientModels.Add(new IngredientListModel { Ingredient = ingredient, IsAdded = true });

        NoIngredientsError = false;

        XamlSetup();

        BindingContext = this;
    }

    public bool InSearchWindow { get; set; }
    public bool SearchModeIncluded { get; set; }

    public bool NoIngredientsError
    {
        get => noIngredientsError;
        set
        {
            noIngredientsError = value;
            MyIngredientsErrorBorderCollor = value ? Colors.Red : Constants.AppColor.Green;
            LabelAddIngredients.TextColor = MyIngredientsErrorBorderCollor;
        }
    }

    public Color MyIngredientsErrorBorderCollor { get; set; }

    public List<IngredientDtoV2> SearchIngredients { get; set; }
    public ObservableCollection<IngredientListModel> SearchIngredientModels { get; set; } = new();

    public List<IngredientDtoV2> MyIngredients { get; set; }
    public ObservableCollection<IngredientListModel> MyIngredientModels { get; set; } = new();

    public List<IngredientDtoV2> ExcludedIngredients { get; set; }
    public ObservableCollection<IngredientListModel> ExcludedIngredientModels { get; set; } = new();

    private void MyIngredientsAdd(IngredientDtoV2 ingredient)
    {
        MyIngredients.Insert(0, ingredient);
        if (!InSearchWindow)
        {
            Sorting.ResizeListView(ListViewMyIngredients, MyIngredients.Count);
            MyIngredientModels.Insert(0, new IngredientListModel { Ingredient = ingredient });
        }

        SaveMyIngredients(MyIngredients);
    }

    private void MyIngredientsRemoveAt(int index)
    {
        MyIngredients.RemoveAt(index);
        if (!InSearchWindow)
        {
            MyIngredientModels.RemoveAt(index);
            Sorting.ResizeListView(ListViewMyIngredients, MyIngredients.Count);
        }

        SaveMyIngredients(MyIngredients);
    }

    private void ExcludedIngredientsAdd(IngredientDtoV2 ingredient)
    {
        ingredient.Role = IngredientRole.Exclude;
        ExcludedIngredients.Insert(0, ingredient);
        if (!InSearchWindow)
        {
            Sorting.ResizeListView(ListViewExcludedIngredients, ExcludedIngredients.Count);
            ExcludedIngredientModels.Insert(0, new IngredientListModel { Ingredient = ingredient });
        }

        SaveExcludedIngredients(ExcludedIngredients);
    }

    private void ExcludedIngredientsRemoveAt(int index)
    {
        ExcludedIngredients.RemoveAt(index);
        if (!InSearchWindow)
        {
            ExcludedIngredientModels.RemoveAt(index);
            Sorting.ResizeListView(ListViewExcludedIngredients, ExcludedIngredients.Count);
        }

        SaveExcludedIngredients(ExcludedIngredients);
    }

    private void XamlSetup()
    {
        SearchBarIngredients.BackgroundColor = Colors.White;
        SearchBarIngredients.HeightRequest = Constants.TextHeight;

        ListViewSearchIngredients.RowHeight = Constants.TextHeight;


        //Main

        StackLayoutMain.Padding = Constants.Padding2;
        StackLayoutMain.Spacing = Constants.Padding2;

        ButtonFeedMe.TextColor = Constants.AppColor.TextWhite;
        ButtonFeedMe.FontSize = Constants.fontSize1;
        ButtonFeedMe.BackgroundColor = Constants.AppColor.Green;
        //Button_FeedMe.CornerRadius = Constants.cornerRadius1;

        //Frame_MyIngredients.CornerRadius = Constants.cornerRadius1;

        LabelMyIngredients.HeightRequest = Constants.TextHeight;

        ListViewMyIngredients.BackgroundColor = Constants.AppColor.LightGray;
        ListViewMyIngredients.RowHeight = Constants.TextHeight;
        ListViewExcludedIngredients.BackgroundColor = Constants.AppColor.LightGray;
        ListViewExcludedIngredients.RowHeight = Constants.TextHeight;
        //ListView_myIngredients.HeightRequest = Constants.textHeight;

        Sorting.ResizeListView(ListViewMyIngredients, MyIngredients.Count);
        Sorting.ResizeListView(ListViewExcludedIngredients, ExcludedIngredients.Count);
    }

    // --------------------------------------------- SPAGHETTI ---------------------------------------------------


    private void GenerateMyIngredientModels()
    {
        MyIngredientModels.Clear();
        foreach (var ingredient in MyIngredients)
            MyIngredientModels.Add(new IngredientListModel
            {
                Ingredient = ingredient,
                IsAdded = true
            });
        Sorting.ResizeListView(ListViewMyIngredients, MyIngredients.Count);
    }

    private void GenerateExcludedIngredientModels()
    {
        ExcludedIngredientModels.Clear();
        foreach (var ingredient in ExcludedIngredients)
            ExcludedIngredientModels.Add(new IngredientListModel
            {
                Ingredient = ingredient,
                IsAdded = true
            });
        Sorting.ResizeListView(ListViewExcludedIngredients, ExcludedIngredients.Count);
    }


    // --------------------------------------------- ASYNC SPAGHETTI ---------------------------------------------------


    private async void Alert(string title, string message, string cancel)
    {
        await DisplayAlert(title, message, cancel);
    }

    private async void GotoMealsListPage()
    {
        var ingredents = new List<IngredientDtoV2>();
        ingredents.AddRange(MyIngredients);
        ingredents.AddRange(ExcludedIngredients);
        await Navigation.PushAsync(new MealsListPage(ingredents) { Title = "Recept" });
    }

    private async void SaveMyIngredients(List<IngredientDtoV2> ingredients)
    {
        await Task.Factory.StartNew(() => User.User.SavedIngredinets = ingredients);
    }

    private async void SaveExcludedIngredients(List<IngredientDtoV2> ingredients)
    {
        await Task.Factory.StartNew(() => User.User.SavedExcludedIngredinets = ingredients);
    }


    // --------------------------------------------- REQUESTS ---------------------------------------------------


    // Recive ingredentDtos from the server and update lists
    private async void GET_ingredientDtos(string search)
    {
        var isLoading = true;
        ActivityIndicatorIngredients.IsRunning = true;
        await Task.Factory.StartNew(() => SearchIngredients = ingredientsSearching.Search(search, out isLoading));

        SearchIngredientModels.Clear();

        if (SearchModeIncluded)
            foreach (var ingredient in SearchIngredients)
                SearchIngredientModels.Add(new IngredientListModel
                {
                    Ingredient = ingredient,
                    IsAdded = MyIngredients.Any(p => p.IngredientId == ingredient.IngredientId) ? true : false
                });
        else
            foreach (var ingredient in SearchIngredients)
                SearchIngredientModels.Add(new IngredientListModel
                {
                    Ingredient = ingredient,
                    IsAdded = ExcludedIngredients.Any(p => p.IngredientId == ingredient.IngredientId) ? true : false,
                    AddedIcon = Constants.ExcludeIngredientCheckIcon,
                    AddedColor = Colors.Red
                });

        if (!isLoading) ActivityIndicatorIngredients.IsRunning = false;
    }


    // --------------------------------------------- EVENTS ---------------------------------------------------


    // Klicked ingredient in search window
    private void ListView_SearchIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (ListViewSearchIngredients.SelectedItem == null) return;

        var index =
            ((ObservableCollection<IngredientListModel>)ListViewSearchIngredients.ItemsSource).IndexOf(
                (IngredientListModel)e.SelectedItem);
        ((ListView)sender).SelectedItem = null;
        var selectedIngredient = SearchIngredients[index];

        SearchIngredientModels[index] = new IngredientListModel
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
                ExcludedIngredientsRemoveAt(
                    ExcludedIngredients.FindIndex(p => p.IngredientId == selectedIngredient.IngredientId));
            else
                ExcludedIngredientsAdd(selectedIngredient);
        }
    }

    // Searching
    private void SearchBar_Ingredients_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (SearchBarIngredients.Text == null) return;

        var searchWord = SearchBarIngredients.Text.ToLower();

        if (searchWord.Length > 0) GET_ingredientDtos(searchWord);
    }

    // Clicked in myIngredients list
    private void ListView_myIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        MyIngredientsRemoveAt(MyIngredientModels.IndexOf(ListViewMyIngredients.SelectedItem as IngredientListModel));
    }

    // Clicked in excludedIngredients list
    private void ListView_excludedIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        ExcludedIngredientsRemoveAt(
            ExcludedIngredientModels.IndexOf(ListViewExcludedIngredients.SelectedItem as IngredientListModel));
    }

    // Clicked go to recipes button
    private void Button_Clicked(object sender, EventArgs e)
    {
        if (MyIngredients.Count > 0)
        {
            GotoMealsListPage();
        }
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
        SearchBarIngredients.Placeholder = "Lägg till ingredienser";
        GridIngredientSearchView.IsVisible = true;
        GridIngredientSearchView.IsEnabled = true;
        ScrollViewMainView.IsVisible = false;
        ScrollViewMainView.IsEnabled = false;
        InSearchWindow = true;
        SearchModeIncluded = true;
    }

    // Open search window excluded
    private void Button_OpenExcludedIngredientsSearch_Clicked(object sender, EventArgs e)
    {
        SearchBarIngredients.Placeholder = "Exkludera ingredienser";
        GridIngredientSearchView.IsVisible = true;
        GridIngredientSearchView.IsEnabled = true;
        ScrollViewMainView.IsVisible = false;
        ScrollViewMainView.IsEnabled = false;
        InSearchWindow = true;
        SearchModeIncluded = false;
    }

    // Close search window included
    private void Button_CloseIngredientsSearch_Clicked(object sender, EventArgs e)
    {
        ScrollViewMainView.IsVisible = true;
        ScrollViewMainView.IsEnabled = true;
        GridIngredientSearchView.IsVisible = false;
        GridIngredientSearchView.IsEnabled = false;
        SearchBarIngredients.Text = string.Empty;
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
        Alert("Mina Ingredienser", "Klicka på “Lägg till” knappen för att lägga till ingredienser du har i ditt kök.",
            "ok");
    }

    private void TapGestureRecognizer_Tapped_ExcludedIngredientsHelp(object sender, EventArgs e)
    {
        Alert("Uteslutna Ingredienser",
            "Här kan du klicka på “Lägg till” knappen för att utesluta ingredienser du inte vill ha i dina recept.",
            "ok");
    }
}