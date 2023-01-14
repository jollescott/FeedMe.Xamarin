using Ramsey.Shared.Dto.V2;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace FeedMe.Classes
{
    internal class Sorting
    {
        public static List<IngredientDtoV2> SortIngredientsByNameLenght(List<IngredientDtoV2> ingredients)
        {
            for (int i = 1; i < ingredients.Count; i++)
            {
                int j = 0;
                while (ingredients[i].IngredientName.Length > ingredients[j].IngredientName.Length)
                {
                    j++;
                }
                IngredientDtoV2 item = ingredients[i];
                ingredients.RemoveAt(i);
                ingredients.Insert(j, item);
            }

            return ingredients;
        }


        public static bool IngredientExistsInList(IngredientDtoV2 ingredient, List<IngredientDtoV2> ingredientList)
        {
            foreach (IngredientDtoV2 item in ingredientList)
            {
                if (item.IngredientId == ingredient.IngredientId)
                {
                    return true;
                }
            }
            return false;
        }


        public static bool RecipeMetaExistsInList(RecipeMetaDtoV2 recipe, List<RecipeMetaDtoV2> recipeList)
        {
            foreach (RecipeMetaDtoV2 r in recipeList)
            {
                if (recipe.RecipeId == r.RecipeId)
                {
                    return true;
                }
            }
            return false;
        }


        public static void ResizeListView(ListView listView, int numberOfItems)
        {
            if (numberOfItems < 1)
            {
                numberOfItems = 1;
            }

            double height = numberOfItems * Constants.textHeight;
            double adjust = (numberOfItems * 0.55) + 0.2;
            listView.HeightRequest = height + adjust;
        }
    }
}
