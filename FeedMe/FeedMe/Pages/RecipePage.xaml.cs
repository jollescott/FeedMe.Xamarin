using System.Windows.Input;
using FeedMe.Classes;
using Newtonsoft.Json;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Enums;
using Ramsey.Shared.Misc;

namespace FeedMe.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipePage
    {
        private readonly HttpClient httpClient = new();
        private readonly RecipeMetaDtoV2 recipeMeta;
        private RecipeDtoV2 recipe;
        private readonly List<IngredientDtoV2> myIngredients;
        private readonly List<Label> ingredentPortionLabels = new List<Label>();
        private readonly bool fromSavedRecipes;
        private bool loadedRecipe = false;

        public bool IsFavorite { get; set; }

        private int portions = 2;
        public int Poritons
        {
            get => portions;
            set
            {
                if (value <= 10 && value >= 1)
                {
                    portions = value;
                }
            }
        }

        private ICommand _favoriteCommand;
        public ICommand FavoriteCommand => _favoriteCommand = _favoriteCommand ?? new Command(RunFavoriteCommand);

        private void RunFavoriteCommand(object obj)
        {
            if (!loadedRecipe)
            {
                return;
            }

            IsFavorite = !IsFavorite;
            OnPropertyChanged(nameof(IsFavorite));

            try
            {
                //Save or unsave recipe
                List<RecipeDtoV2> savedRecipes = User.User.SavedRecipes;
                // save
                if (IsFavorite && !savedRecipes.Any(p => p.Name == recipe.Name))
                {
                    savedRecipes.Insert(0, recipe);
                    User.User.SavedRecipes = savedRecipes;
                }
                // unsave
                else if (!IsFavorite && savedRecipes.Any(p => p.Name == recipe.Name))
                {
                    int toRemoveIndex = -1;
                    for (int i = 0; i < savedRecipes.Count; i++)
                    {
                        if (recipe.RecipeId == savedRecipes[i].RecipeId)
                        {
                            toRemoveIndex = i;
                            break;
                        }
                    }
                    if (toRemoveIndex != -1)
                    {
                        savedRecipes.RemoveAt(toRemoveIndex);
                        User.User.SavedRecipes = savedRecipes;
                    }
                }
            }
            catch (Exception) { }
        }

        public RecipePage(RecipeMetaDtoV2 recipeMeta)
        {
            InitializeComponent();
            BindingContext = this;

            fromSavedRecipes = false;
            this.recipeMeta = recipeMeta;
            myIngredients = User.User.SavedIngredinets;
            XamlSetup1();
            GET_recipeDto(recipeMeta.RecipeId);

            Task.Factory.StartNew(() => UpdateFavorite());
        }
        public RecipePage(RecipeDtoV2 recipe)
        {
            InitializeComponent();
            BindingContext = this;

            fromSavedRecipes = true;
            this.recipe = recipe;
            myIngredients = User.User.SavedIngredinets;
            XamlSetup1();
            XamlSetup2();

            Task.Factory.StartNew(() => UpdateFavorite());
        }

        private void UpdateFavorite()
        {
            IsFavorite = User.User.SavedRecipes.Any(p => p.Name == (fromSavedRecipes ? recipe.Name : recipeMeta.Name));
            OnPropertyChanged(nameof(IsFavorite));
        }

        private async void GET_recipeDto(string id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(RamseyApi.V2.Recipe.Retreive + "?id=" + id);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    recipe = JsonConvert.DeserializeObject<RecipeDtoV2>(result);
                    XamlSetup2();
                    loadedRecipe = true;
                }
                else
                {
                    await DisplayAlert("Fel", "Kunnde inte ansluta till servern\n\nstatus code: " + (int)response.StatusCode, "ok");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Fel", "Kunnde inte ansluta till servern", "ok");
            }
        }

        private void XamlSetup1()
        {
            //Recipe Image
            Grid_Images.HeightRequest = Application.Current.MainPage.Width;
            ImageRecipe.Source = fromSavedRecipes ? recipe.Image : recipeMeta.Image;
            //Image_OwnerLogo.Source = recipeMeta.OwnerLogo;

            //Owner Info
            ImageOwnerLogo.Source = fromSavedRecipes ? recipe.OwnerLogo : recipeMeta.OwnerLogo;
            LabelRecipeName.Text = fromSavedRecipes ? recipe.Name : recipeMeta.Name;
            LabelOwnerLink.Text = fromSavedRecipes ? recipe.Source : recipeMeta.Source;
            LabelOwnerLink.TextColor = Constants.AppColor.TextLink;
            //IconFavorite.FontSize = Constants.FontSize1double;
            //IconButton_AddPortionCount.FontSize = Constants.FontSize1double;
            //IconButton_RemovePortionCount.FontSize = Constants.FontSize1double;

            //Ingredients head
            FrameIngredientsHead.BackgroundColor = Constants.AppColor.Green;
            LabelIngridientsHead.Text = "Ingredienser";
            LabelIngridientsHead.TextColor = Constants.AppColor.TextWhite;
            LabelIngridientsHead.FontSize = Constants.fontSize1;

            //Instructions head
            FrameInstructionsHead.BackgroundColor = Constants.AppColor.Green;
            LabelInstructionsHead.Text = "Tillagning";
            LabelInstructionsHead.TextColor = Constants.AppColor.TextWhite;
            LabelInstructionsHead.FontSize = Constants.fontSize1;
        }

        private void XamlSetup2()
        {
            //Ingredients
            for (int i = 0; i < recipe.RecipeParts.Count() + 1; i++)
            {
                GridIngredients.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            //Grid_Ingredients.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            //Grid_Ingredients.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            GridIngredients.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            GridIngredients.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            for (int i = 0; i < recipe.RecipeParts.Count(); i++)
            {
                // quantity and unit
                ingredentPortionLabels.Add(new Label()
                {
                    TextColor = Constants.AppColor.TextGray,
                    FontSize = Constants.fontSize2,
                    Margin = Constants.TextListMargin,
                    HorizontalTextAlignment = TextAlignment.End
                });
                /*
                GridIngredients.Children.Add(ingredentPortionLabels[i], 0, i);

                // ingredient names
                GridIngredients.Children.Add(new Label()
                {
                    Text = recipe.RecipeParts.ToList()[i].IngredientName.Trim(),
                    TextColor = Constants.AppColor.TextBlack,
                    FontSize = Constants.fontSize2,
                    Margin = Constants.TextListMargin
                }, 1, i);
                
                */

                // has ingredient icons
                foreach (IngredientDtoV2 myIngredient in myIngredients)
                {

                    if (recipe.RecipeParts.ToList()[i].IngredientName.Trim() == myIngredient.IngredientName.Trim())
                    {
                        /*
                        GridIngredients.Children.Add(new IconLabel
                        {
                            Text = "md-check",
                            FontSize = Constants.fontSize1,
                            TextColor = Constants.AppColor.Green,
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Center
                        }, 1, i);
                        */
                    }
                }

                // separation lines
                /*
                GridIngredients.Children.Add(new BoxView()
                {
                    BackgroundColor = Colors.LightGray,
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                }, 0, 2, i, i + 1);
                */
            }

            UpdateIngredientPortions(portions);  // set ingredient portions text


            //Instructions
            bool startsWithNum = false;
            if (recipe.Owner == RecipeProvider.Hemmets)
            {
                startsWithNum = true;
            }
            bool showNumbers = true;
            if (recipe.Directions.Count() <= 1)
            {
                showNumbers = false;
            }


            int n = 1;
            for (int i = 0; i < recipe.Directions.Count(); i++)
            {
                string text = recipe.Directions.ToList()[i].TrimStart();

                if (!startsWithNum || char.IsDigit(text[0]))
                {
                    //Add number
                    if (showNumbers)
                    {
                        StackInstructions.Children.Add(new Label()
                        {
                            Text = Convert.ToString(n) + ".",
                            TextColor = Constants.AppColor.Green,
                            FontSize = Constants.fontSize2,
                            Margin = Constants.TextListMargin
                        });
                        n++;
                    }

                    // Remove start numbers (if they exist)
                    while (char.IsDigit(text[0]) || text[0] == '.')
                    {
                        text = text.Substring(1);
                    }
                    text = text.TrimStart();

                    //Add instruction
                    StackInstructions.Children.Add(new Label()
                    {
                        Text = text,
                        TextColor = Constants.AppColor.TextBlack,
                        FontSize = Constants.fontSize2,
                        Margin = Constants.TextListMargin
                    });
                }
                else
                {
                    n = 1; //reset step count

                    //Add sub heading
                    StackInstructions.Children.Add(new Label()
                    {
                        Text = recipe.Directions.ToList()[i].TrimStart(),
                        TextColor = Constants.AppColor.TextGreen,
                        FontSize = Constants.fontSize2,
                        Margin = new Thickness(Constants.TextListMargin, Constants.Padding3, Constants.TextListMargin, Constants.TextListMargin),
                        HorizontalTextAlignment = TextAlignment.Center
                    });
                }
            }

            loadedRecipe = true;
            ActivityIndicatorLoadingRecipeIngredients.IsRunning = false;
            ActivityIndicatorLoadingRecipeInstructions.IsRunning = false;
        }

        private void UpdateIngredientPortions(int portions)
        {
            double portionMuliplier = 0.5;
            LabelPortions.Text = (portions == 1) ? portions.ToString() + " portion" : portions.ToString() + " portioner";
            for (int i = 0; i < ingredentPortionLabels.Count; i++)
            {
                ingredentPortionLabels[i].Text = (recipe.RecipeParts.ToList()[i].Quantity != 0 && recipe.RecipeParts.ToList()[i].Unit != "odefinierad") ? (recipe.RecipeParts.ToList()[i].Quantity * portions * portionMuliplier).ToString().Trim() + " " + recipe.RecipeParts.ToList()[i].Unit.Trim() : "";
            }
        }

        //Navigation back button
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Source link tapped
        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            // open source in browser
            await Browser.OpenAsync(new Uri(fromSavedRecipes ? recipe.Source : recipeMeta.Source));
        }

        private void IconButton_AddPortionCount_Clicked(object sender, EventArgs e)
        {
            Poritons += 1;
            UpdateIngredientPortions(portions);
        }

        private void IconButton_RemovePortionCount_Clicked(object sender, EventArgs e)
        {
            Poritons -= 1;
            UpdateIngredientPortions(portions);
        }
    }
}