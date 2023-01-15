using Ramsey.Shared.Dto.V2;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace FeedMe.Classes
{
    internal abstract class Sorting
    {
        public static List<IngredientDtoV2> SortIngredientsByNameLenght(List<IngredientDtoV2> ingredients)
        {
            for (var i = 1; i < ingredients.Count; i++)
            {
                var j = 0;
                while (ingredients[i].IngredientName.Length > ingredients[j].IngredientName.Length)
                {
                    j++;
                }
                var item = ingredients[i];
                ingredients.RemoveAt(i);
                ingredients.Insert(j, item);
            }

            return ingredients;
        }


        public static bool IngredientExistsInList(IngredientDtoV2 ingredient, List<IngredientDtoV2> ingredientList)
        {
            return ingredientList.Any(item => item.IngredientId == ingredient.IngredientId);
        }


        public static bool RecipeMetaExistsInList(RecipeMetaDtoV2 recipe, List<RecipeMetaDtoV2> recipeList)
        {
            return recipeList.Any(r => recipe.RecipeId == r.RecipeId);
        }


        public static void ResizeListView(ListView listView, int numberOfItems)
        {
            if (numberOfItems < 1)
            {
                numberOfItems = 1;
            }

            double height = numberOfItems * Constants.TextHeight;
            var adjust = (numberOfItems * 0.55) + 0.2;
            listView.HeightRequest = height + adjust;
        }
    }
}
