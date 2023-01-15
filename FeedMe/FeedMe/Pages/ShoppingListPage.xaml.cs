using System.Collections.ObjectModel;
using FeedMe.Classes;
using Newtonsoft.Json;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Misc;

namespace FeedMe.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ShoppingListPage : ContentPage
{
    private readonly HttpClient httpClient = new();
    private readonly List<IconTestModel> icons_testList = new();
    private readonly List<IngredientDtoV2> shoppingListIngredients = new();
    private readonly bool state = false;


    // ----------------------------------------- EVENTS -----------------------------------------------


    private bool searching;
    private List<IngredientDtoV2> searchIngredients = new();


    public ShoppingListPage()
    {
        InitializeComponent();

        BindingContext = this;

        var savedIngredients = User.User.ShoppingListIngredients;
        if (savedIngredients != null && savedIngredients != "")
            shoppingListIngredients = JsonConvert.DeserializeObject<List<IngredientDtoV2>>(savedIngredients);


        //TestIcons.Add("md-add");
        //TestIcons.Add("md-add");
        //TestIcons.Add("md-add");
        //TestIcons.Add("md-remove");


        XamlSetup();
    }
    //List<IconTestModel> icons_testList2 = new List<IconTestModel>();

    public ObservableCollection<string> TestIcons { get; set; } = new() { "md-add", "md-remove" };

    private void XamlSetup()
    {
        //ListView_ShoppingList.BackgroundColor = Constants.AppColor.lightGray;
        //ListView_ShoppingList.ItemsSource =
        StackLayoutMain.Padding = Constants.Padding2;
        ListViewMyIngredients.RowHeight = Constants.TextHeight;
        UpdateShoppingListListView(shoppingListIngredients);
    }

    // --------------------------------------------- SPAGHETTI ---------------------------------------------


    private void UpdateSearchIngreadientsListView(List<IngredientDtoV2> ingredients)
    {
        try
        {
            var items = new List<ListItem>();
            foreach (var ingredient in ingredients)
                items.Add(new ListItem
                {
                    Name = ingredient.IngredientName, Color
                        //IconSource = "md-remove-shopping-cart",   // FUNKAR INTE aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa :(
                        = Sorting.IngredientExistsInList(ingredient, shoppingListIngredients)
                            ? Colors.Black
                            : Color.FromHex("#00CC66")
                });
            ListViewSearchIngredients.ItemsSource = items;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void UpdateShoppingListListView(List<IngredientDtoV2> ingredients)
    {
        var itemsorce = new List<IngredientDtoV2>();
        foreach (var ingredient in ingredients) itemsorce.Insert(0, ingredient);

        ListViewMyIngredients.ItemsSource = itemsorce;

        Sorting.ResizeListView(ListViewMyIngredients, shoppingListIngredients.Count);
    }


    // --------------------------------------------- REQUESTS ---------------------------------------------------


    private async void GET_ingredientDtos(string search)
    {
        try
        {
            //HttpResponseMessage response = _httpClient.GetAsync(_adress).ConfigureAwait(false).GetAwaiter().GetResult();
            //HttpResponseMessage response = _httpClient.GetAsync(_adress).GetAwaiter().GetResult();
            var response = await httpClient.GetAsync(RamseyApi.V2.Ingredient.Suggest + "?search=" + search);


            if (response.IsSuccessStatusCode)
            {
                //await DisplayAlert("success", "succeess", "ok");
                var result = await response.Content.ReadAsStringAsync();
                searchIngredients =
                    Sorting.SortIngredientsByNameLenght(JsonConvert.DeserializeObject<List<IngredientDtoV2>>(result));
                UpdateSearchIngreadientsListView(searchIngredients);
            }
            else
            {
                await DisplayAlert("Connection error",
                    "Status code " + (int)response.StatusCode + ": " + response.StatusCode, "ok");
            }
        }
        catch (Exception)
        {
            await DisplayAlert("An error occurred", "Server conection failed", "ok");
        }
    }

    private void SearchBar_Ingredients_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchWord = SearchBarIngredients.Text.ToLower();

        if (searchWord.Length > 0)
        {
            if (!searching) searching = true;

            GET_ingredientDtos(searchWord);
        }
        else
        {
            searching = false;
        }
    }

    private void ListView_myIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        shoppingListIngredients.Remove(ListViewMyIngredients.SelectedItem as IngredientDtoV2);
        UpdateShoppingListListView(shoppingListIngredients);

        User.User.ShoppingListIngredients = JsonConvert.SerializeObject(shoppingListIngredients);
    }

    private void Button_AddIngredients_Clicked(object sender, EventArgs e)
    {
        FrameSearch.IsEnabled = true;
        FrameSearch.IsVisible = true;
        ScrollViewMain.IsEnabled = false;
        ScrollViewMain.IsVisible = false;
    }

    private void ListView_SearchIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedIngredient =
            searchIngredients[
                ((List<ListItem>)ListViewSearchIngredients.ItemsSource).IndexOf((ListItem)e.SelectedItem)];

        if (Sorting.IngredientExistsInList(selectedIngredient, shoppingListIngredients))
        {
            foreach (var ingredient in shoppingListIngredients)
                if (ingredient.IngredientId == selectedIngredient.IngredientId)
                {
                    shoppingListIngredients.Remove(ingredient);
                    break;
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
        ScrollViewMain.IsEnabled = true;
        ScrollViewMain.IsVisible = true;
        FrameSearch.IsEnabled = false;
        FrameSearch.IsVisible = false;
        SearchBarIngredients.Text = "";
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
        var sorce = new List<IconTestModel>
        {
            new() { Icon = name },
            new() { Icon = name }
        };
        //list_test.ItemsSource = sorce;
        ListTest.ItemsSource = icons_testList;
    }
}

internal class IconTestModel
{
    public string Icon { get; set; }
}