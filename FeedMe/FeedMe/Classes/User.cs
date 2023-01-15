using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Ramsey.Shared.Dto.V2;

namespace FeedMe.User;

internal class User
{
    private static ISettings AppSettings =>
        CrossSettings.Current;

    public static List<IngredientDtoV2> SavedIngredinets
    {
        get
        {
            var json = AppSettings.GetValueOrDefault(nameof(SavedIngredinets), string.Empty);
            if (json == null || json == "")
                return new List<IngredientDtoV2>();
            try
            {
                return JsonConvert.DeserializeObject<List<IngredientDtoV2>>(json);
            }
            catch
            {
                return new List<IngredientDtoV2>();
            }
        }
        set => AppSettings.AddOrUpdateValue(nameof(SavedIngredinets), JsonConvert.SerializeObject(value));
    }

    public static List<IngredientDtoV2> SavedExcludedIngredinets
    {
        get
        {
            var json = AppSettings.GetValueOrDefault(nameof(SavedExcludedIngredinets), string.Empty);
            if (json == null || json == "")
                return new List<IngredientDtoV2>();
            try
            {
                return JsonConvert.DeserializeObject<List<IngredientDtoV2>>(json);
            }
            catch
            {
                return new List<IngredientDtoV2>();
            }
        }
        set => AppSettings.AddOrUpdateValue(nameof(SavedExcludedIngredinets), JsonConvert.SerializeObject(value));
    }

    public static List<RecipeMetaDtoV2> SavedRecipeMetas
    {
        get
        {
            var json = AppSettings.GetValueOrDefault(nameof(SavedRecipeMetas), string.Empty);
            if (json == null || json == "")
                return new List<RecipeMetaDtoV2>();
            try
            {
                return JsonConvert.DeserializeObject<List<RecipeMetaDtoV2>>(json);
            }
            catch
            {
                return new List<RecipeMetaDtoV2>();
            }
        }
        set => AppSettings.AddOrUpdateValue(nameof(SavedRecipeMetas), JsonConvert.SerializeObject(value));
    }

    public static List<RecipeDtoV2> SavedRecipes
    {
        get
        {
            var json = AppSettings.GetValueOrDefault(nameof(SavedRecipes), string.Empty);
            if (json == null || json == "")
                return new List<RecipeDtoV2>();
            try
            {
                return JsonConvert.DeserializeObject<List<RecipeDtoV2>>(json);
            }
            catch
            {
                return new List<RecipeDtoV2>();
            }
        }
        set => AppSettings.AddOrUpdateValue(nameof(SavedRecipes), JsonConvert.SerializeObject(value));
    }

    public static string ShoppingListIngredients
    {
        get => AppSettings.GetValueOrDefault(nameof(ShoppingListIngredients), string.Empty);
        set => AppSettings.AddOrUpdateValue(nameof(ShoppingListIngredients), value);
    }
}