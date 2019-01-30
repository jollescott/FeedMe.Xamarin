using Ramsey.Shared.Dto;
using Ramsey.Shared.Dto.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using Ramsey.Shared.Misc;
using System.Windows.Input;
using FeedMe.Interfaces;
using FeedMe.Classes;
using Ramsey.Shared.Enums;
using System.Threading.Tasks;
using Plugin.Iconize;

namespace FeedMe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecipePage : ContentPage
	{
        HttpClient httpClient = new HttpClient();
        RecipeMetaDtoV2 recipeMeta;
        RecipeDtoV2 recipe;
        List<IngredientDtoV2> myIngredients;
        List<Label> ingredentPortionLabels = new List<Label>();

        bool loadedRecipe = false;

        public bool IsFavorite { get; set; }

        private int portions = 2;
        public int Poritons {
            get { return portions; }
            set
            {
                if (value <= 10 && value >= 1)
                    portions = value;                    
            }
        }

        private ICommand _favoriteCommand;
        public ICommand FavoriteCommand => _favoriteCommand = _favoriteCommand ?? new Command(RunFavoriteCommand);

        private void RunFavoriteCommand(object obj)
        {
            //var userid = DependencyService.Get<IFacebook>().UserId;
            //await RamseyConnection.SaveFavoriteAsync(recipeMeta.RecipeID, userid);

            IsFavorite = !IsFavorite;

            OnPropertyChanged(nameof(IsFavorite));


            // Save or unsave recipe

            var savedRecipeMetas = User.User.SavedRecipeMetas;
            // save
            if (IsFavorite && !Sorting.RecipeMetaExistsInList(recipeMeta, savedRecipeMetas))
            {
                savedRecipeMetas.Insert(0, recipeMeta);
                User.User.SavedRecipeMetas = savedRecipeMetas;
            }
            // unsave
            else if (!IsFavorite && Sorting.RecipeMetaExistsInList(recipeMeta, savedRecipeMetas))
            {
                int toRemoveIndex = -1;
                for (int i = 0; i < savedRecipeMetas.Count; i++)
                {
                    if (recipeMeta.RecipeID == savedRecipeMetas[i].RecipeID)
                    {
                        toRemoveIndex = i;
                        break;
                    }
                }
                if (toRemoveIndex != -1)
                {
                    savedRecipeMetas.RemoveAt(toRemoveIndex);
                    User.User.SavedRecipeMetas = savedRecipeMetas;
                }
            }
        }

        public RecipePage (RecipeMetaDtoV2 recipeMeta)
		{
            InitializeComponent();
            BindingContext = this;

            this.recipeMeta = recipeMeta;
            myIngredients = User.User.SavedIngredinets;
            XamlSetup1();
            GET_recipeDto(recipeMeta.RecipeID);
            Task.Factory.StartNew(() => UpdateFavorite());
		}

        void UpdateFavorite()
        {
            IsFavorite = Sorting.RecipeMetaExistsInList(recipeMeta, User.User.SavedRecipeMetas);
            OnPropertyChanged(nameof(IsFavorite));
        }

        async void GET_recipeDto(string id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(RamseyApi.V2.Recipe.Retreive + "?id=" + id);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    recipe = JsonConvert.DeserializeObject<RecipeDtoV2>(result);
                    XamlSetup2();
                }
                else
                {
                    await DisplayAlert("Fel", "Kunnde inte ansluta till servern\n\nstatus code: " + (int)response.StatusCode, "ok");
                }
            }
            catch (Exception _e)
            {
                await DisplayAlert("Fel", "Kunnde inte ansluta till servern", "ok");
            }
        }

        void XamlSetup1()
        {
            //Recipe Image
            Grid_Images.HeightRequest = Application.Current.MainPage.Width;
            Image_Recipe.Source = recipeMeta.Image;
            //Image_OwnerLogo.Source = recipeMeta.OwnerLogo;

            //Owner Info
            Image_OwnerLogo.Source = recipeMeta.OwnerLogo;
            Label_RecipeName.Text = recipeMeta.Name;
            Label_OwnerLink.Text = recipeMeta.Source;
            Label_OwnerLink.TextColor = Constants.AppColor.text_link;
            Icon_Favorite.FontSize = Constants.fontSize1double;
            IconButton_AddPortionCount.FontSize = Constants.fontSize1double;
            IconButton_RemovePortionCount.FontSize = Constants.fontSize1double;

            //Ingredients head
            Frame_IngredientsHead.BackgroundColor = Constants.AppColor.green;
            Label_IngridientsHead.Text = "Ingredienser";
            Label_IngridientsHead.TextColor = Constants.AppColor.text_white;
            Label_IngridientsHead.FontSize = Constants.fontSize1;

            //Instructions head
            Frame_InstructionsHead.BackgroundColor = Constants.AppColor.green;
            Label_InstructionsHead.Text = "Tillagning";
            Label_InstructionsHead.TextColor = Constants.AppColor.text_white;
            Label_InstructionsHead.FontSize = Constants.fontSize1;
        }
        
        void XamlSetup2()
        {
            //Ingredients
            for (int i = 0; i < recipe.RecipeParts.Count() + 1; i++)
            {
                Grid_Ingredients.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            //Grid_Ingredients.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            //Grid_Ingredients.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
             Grid_Ingredients.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            Grid_Ingredients.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            for (int i = 0; i < recipeMeta.RecipeParts.Count(); i++)
            {
                // quantity and unit
                ingredentPortionLabels.Add(new Label()
                {
                    TextColor = Constants.AppColor.text_gray,
                    FontSize = Constants.fontSize2,
                    Margin = Constants.textListMargin,
                    HorizontalTextAlignment = TextAlignment.End
                });
                Grid_Ingredients.Children.Add(ingredentPortionLabels[i], 0, i);

                // ingredient names
                Grid_Ingredients.Children.Add(new Label()
                {
                    Text = recipe.RecipeParts.ToList()[i].IngredientName.Trim(),
                    TextColor = Constants.AppColor.text_black,
                    FontSize = Constants.fontSize2,
                    Margin = Constants.textListMargin
                }, 1, i);

                // has ingredient icons
                foreach (var myIngredient in myIngredients)
                {

                    if (recipe.RecipeParts.ToList()[i].IngredientName.Trim() == myIngredient.IngredientName.Trim())
                    {
                        //Grid_Ingredients.Children.Add(new Image()
                        //{
                        //    Source = "icon_check.png",
                        //    HorizontalOptions = LayoutOptions.End,
                        //    VerticalOptions = LayoutOptions.Center,
                        //    Aspect = Aspect.AspectFit,
                        //    HeightRequest = 15,
                        //    Margin = new Thickness(0, 0, 5, 0)
                        //}, 1, i);

                        Grid_Ingredients.Children.Add(new IconLabel
                        {
                            Text = "md-check",
                            FontSize = Constants.fontSize1,
                            TextColor = Constants.AppColor.green,
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Center
                        }, 1, i);
                    }
                }

                // separation lines
                Grid_Ingredients.Children.Add(new BoxView()
                {
                    BackgroundColor = Color.LightGray,
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                }, 0, 2, i, i + 1);
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
                        Stack_Instructions.Children.Add(new Label()
                        {
                            Text = Convert.ToString(n) + ".",
                            TextColor = Constants.AppColor.green,
                            FontSize = Constants.fontSize2,
                            Margin = Constants.textListMargin
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
                    Stack_Instructions.Children.Add(new Label()
                    {
                        Text = text,
                        TextColor = Constants.AppColor.text_black,
                        FontSize = Constants.fontSize2,
                        Margin = Constants.textListMargin
                    });
                }
                else
                {
                    n = 1; //reset step count

                    //Add sub heading
                    Stack_Instructions.Children.Add(new Label()
                    {
                        Text = recipe.Directions.ToList()[i].TrimStart(),
                        TextColor = Constants.AppColor.text_green,
                        FontSize = Constants.fontSize2,
                        Margin = new Thickness(Constants.textListMargin, Constants.padding3, Constants.textListMargin, Constants.textListMargin),
                        HorizontalTextAlignment = TextAlignment.Center
                    });
                }
            }

            loadedRecipe = true;
            ActivityIndicatior_LoadingRecipeIngredients.IsRunning = false;
            ActivityIndicatior_LoadingRecipeInstructions.IsRunning = false;
        }

        void UpdateIngredientPortions(int portions)
        {
            double portionMuliplier = 0.5;
            Label_Portions.Text = (portions == 1) ? portions.ToString() + " portion" : portions.ToString() + " portioner";
            for (int i = 0; i < ingredentPortionLabels.Count; i++)
            {
                ingredentPortionLabels[i].Text = (recipe.RecipeParts.ToList()[i].Quantity != 0) ? (recipe.RecipeParts.ToList()[i].Quantity * portions * portionMuliplier).ToString().Trim() + " " + recipe.RecipeParts.ToList()[i].Unit.Trim() : "";
            }
        }

        //Navigation back button
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Source link tapped
        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            // open source in browser
            Device.OpenUri(new Uri(recipeMeta.Source));
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