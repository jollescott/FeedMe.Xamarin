using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Ramsey.NET.Dto;

namespace FeedMe
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient httpClient = new HttpClient();

        List<string> eatebles = new List<string> {
            "tomato soup",
            "tomato",
            "tomato sace",
            "tomato puree",
            "Felix Tomato Ketchup",
            "Heinz Tomato Ketchup",
            "Heinz Organic Tomato Ketchup",
            "Sir Kensington's Classic Ketchup",
            "First Field Jersey Ketchup",
            "Catskill Provisions All Natural Ketchup",
            "Victoria Amory Champagne Ketchup",
            "Whataburger Fancy Ketchup",
            "Muir Glen Organic Tomato Ketchup",
            "Sosu Srirachachup",
            "Stonewall Kitchen Country Ketchup",
            "True Made Ketchup",
            "Hunt's 100% Natural Tomato Ketchup",
            "Sainsbury’s tomato ketchup",
            "Tesco tomato ketchup",
            "Co-op Loved by Us tomato ketchup",
            "Waitrose Stokes real tomato ketchup",
            "Morrisons squeezy tomato ketchup,",
            "Aldi Bramwells tomato ketchup",
            "Lidl Kania tomato ketchup",
            "Asda Chosen by You tomato ketchup",
            "French's Tomato Ketchup"};
        List<IngredientDto> selectedEatebles = new List<IngredientDto>();

        //the selectedEatebles List won't work without this. I don't know why
        private List<string> ConvertToNonCrashgingMagicalList(List<string> badList)
        {
            List<string> goodList = new List<string>();

            //Makes an copy of the old list that is magical and won't crash the program
            for (int i = 0; i < badList.Count(); i++)
            {
                goodList.Add(badList[i]);
            }

            return goodList; //return the magic list
        }

        public MainPage()
        {
            InitializeComponent();

            //ListView_SelectedIngredients.ItemsSource = selectedEatebles; //update listview for selected items

            XamlSetup();

            //Add test spagetti to eatebles
            for (int i = 0; i < 1000; i++)
            {
                eatebles.Add("spagetti" + i);
            }

        }

        void XamlSetup()
        {
            BackgroundImage = "background.jpg";

            Stack_Head.BackgroundColor = Constants.backgroundColor;

            ListView_Ingredients.BackgroundColor = Constants.backgroundColor;

            ListView_SeletedIngredients2.BackgroundColor = Constants.backgroundColor;

            Label_myFridge.TextColor = Constants.textColor1;
            Label_myFridge.FontSize = Constants.fontSize1;

            Button_FeedMe.Margin = Constants.padding1;
            Button_FeedMe.TextColor = Constants.textColor3;
            Button_FeedMe.FontSize = Constants.fontSize1;
            Button_FeedMe.BackgroundColor = Constants.mainColor2;
            Button_FeedMe.CornerRadius = Constants.buttonCornerRadius;
        }


        //Sorts list of strings, shortes to longest
        private List<string> SortByLength(List<string> list)
        {
            List<string> sortedList = new List<string>();

            int length = list.Count();

            //loop through unsorted list
            for (int i = 0; i < length; i++)
            {
                int indexShortest = 0; //the index for the shortest string in the list

                //find shortes
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].Length < list[indexShortest].Length)
                    {
                        indexShortest = j;
                    }
                }

                sortedList.Add(list[indexShortest]);  //add the shortes string to the new list
                list.RemoveAt(indexShortest);         //and remove it from the old one
            }

            return sortedList;
        }

        //Find string in list. Returns list index
        private int FindIn(string str, List<string> list)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i] == str)
                {
                    return i;
                }
            }
            return -1;
        }


        List<IngredientDto> filterdEatebles = new List<IngredientDto>();
        //Updates the ListView when user types in "SearchBar_eatebles"
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchWord = SearchBar_Ingredients.Text.ToLower();


            if (searchWord != "")
            {
                ListView_Ingredients.IsVisible = true;
                //Put all eatebles that contains the searchWord in a new List (filterdEatebles)
                /*for (int i = 0; i < eatebles.Count; i++)
                {
                    if (eatebles[i].ToLower().Contains(searchWord))
                    {
                        filterdEatebles.Add(eatebles[i]);
                    }
                }*/

                httpClient.GetAsync(Constants.server_adress + searchWord).ContinueWith(async (t) =>
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

            } else
            {
                ListView_Ingredients.IsVisible = false;
            }
        }

        //When new ingrident is selected from ingridients listview
        private void ListView_Ingredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            IngredientDto selected = e.SelectedItem as IngredientDto;
            selectedEatebles.Add(selected);
            //ListView_SelectedIngredients.ItemsSource = selectedEatebles;

            var names = selectedEatebles.Select(x => x.Name);

            ListView_SeletedIngredients2.ItemsSource = names;
        }

        //When new ingridient is selected from selected ingridients2 
        private void ListView_SelectedIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            IngredientDto selected = ListView_Ingredients.SelectedItem as IngredientDto;
            selectedEatebles.Remove(selected);
            //ListView_SelectedIngredients.ItemsSource = selectedEatebles;
        }

        //When new ingridient is selected from selected ingridients2
        private void ListView_SeletedIngredients2_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            string str = ListView_SeletedIngredients2.SelectedItem.ToString();
            for (int i = 0; i < selectedEatebles.Count; i++)
            {
                if (str == selectedEatebles[i].Name)
                {
                    selectedEatebles.Remove(selectedEatebles[i]);
                    ListView_SeletedIngredients2.ItemsSource = selectedEatebles.Select(x => x.Name);
                    break;
                }
            }
        }

        // Klicked FeedMe
        private void Button_Clicked(object sender, EventArgs e)
        {
            //string[] str = { "spagetti", "sås" };
            //string[] str1 = { "mjöl", "socker", "mjölk", "ägg", "choklad" };
            //Meal[] meals = { new Meal("kakor", str1, "gå bara till ica"), new Meal("spagett", str, "koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka vvvkoka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka vkoka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka koka") };
            //await Navigation.PushAsync(new MealsListPage(meals) { Title = "Bon Appétit" });

            /*IngredientDto[] ingredientsToPost = new IngredientDto[selectedEatebles.Count];
            for (int i = 0; i < ingredientsToPost.Length; i++)
            {
                ingredientsToPost[i] = filterdEatebles[i];
            }

            PostIngredients(ingredientsToPost);*/

            PostIngredients(selectedEatebles);


            //List<RecipeDto> recipes = new List<RecipeDto>();
            //RecipeDto m1 = new RecipeDto();
            //m1.Name = "köttbullarochpotatis";
            //recipes.Add(m1);
            //RecipeDto m2 = new RecipeDto();
            //m2.Name = "tårta";
            //recipes.Add(m2);
            //RecipeDto m3 = new RecipeDto();
            //m3.Name = "fisk och ris";
            //recipes.Add(m3);
            //gotoMealsListPage(recipes);
        }

        async void gotoMealsListPage(List<RecipeDto> recipeDto)
        {
            Meal[] meals = new Meal[recipeDto.Count];
            string[] testStr = { "ingridiens1", "ingridens2", "ingridiens3" };
            for (int i = 0; i < meals.Length; i++)
            {
                meals[i] = new Meal(recipeDto[i].Name, testStr);
            }

            await Navigation.PushAsync(new MealsListPage(recipeDto) { Title = "Bon Appétit" });
        }

        async void PostIngredients(List<IngredientDto> ingredientDto)
        {
            var json = JsonConvert.SerializeObject(ingredientDto);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage respone = await httpClient.PostAsync(Constants.server_adress_recipe, content);

            if (respone.IsSuccessStatusCode)
            {
                var result = await respone.Content.ReadAsStringAsync();
                var recipes = JsonConvert.DeserializeObject<List<RecipeDto>>(result);
                
                gotoMealsListPage(recipes);
            }
        }

        bool imgbool = true;
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            imgbool = !imgbool;
            if (imgbool)
            {
                Image_Page.Source = "myFood_icon.png";
                if (SearchBar_Ingredients.Text != "")
                {
                    ListView_Ingredients.IsVisible = true;
                }
                SearchBar_Ingredients.IsVisible = true;
                ListView_SeletedIngredients2.IsVisible = false;
                Label_myFridge.IsVisible = false;
            }
            else
            {
                Image_Page.Source = "search_icon.png";
                ListView_SeletedIngredients2.IsVisible = true;
                Label_myFridge.IsVisible = true;
                ListView_Ingredients.IsVisible = false;
                SearchBar_Ingredients.IsVisible = false;
            }
        }
    }

}
