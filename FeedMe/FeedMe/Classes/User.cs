using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Ramsey.Shared.Dto.V2;
using System;
using System.Collections.Generic;
using System.Text;

namespace FeedMe.User
{
    class User
    {
        private static ISettings AppSettings =>
    CrossSettings.Current;

        public static string SavedIngredinets
        {
            get => AppSettings.GetValueOrDefault(nameof(SavedIngredinets), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(SavedIngredinets), value);
        }

        public static List<RecipeMetaDtoV2> SavedRecipeMetas
        {
            get
            {
                string json = AppSettings.GetValueOrDefault(nameof(SavedRecipeMetas), string.Empty);
                if (json == null || json == "")
                    return new List<RecipeMetaDtoV2>();
                else
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<List<RecipeMetaDtoV2>>(json);
                    }
                    catch
                    {
                        return new List<RecipeMetaDtoV2>();
                    }
                }
            }
            set => AppSettings.AddOrUpdateValue(nameof(SavedRecipeMetas), JsonConvert.SerializeObject(value));
        }

        public static string ShoppingListIngredients
        {
            get => AppSettings.GetValueOrDefault(nameof(ShoppingListIngredients), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ShoppingListIngredients), value);
        }
    }
}
