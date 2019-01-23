using Ramsey.Shared.Dto.V2;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FeedMe.Classes
{
    class Sorting
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


        public static bool IngredientExistsInList(IngredientDtoV2 ingredient, List<IngredientDtoV2> list)
        {
            foreach (IngredientDtoV2 item in list)
            {
                if (item.IngredientId == ingredient.IngredientId)
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
            double adjust = 1 + (numberOfItems - 1) * 0.36;
            listView.HeightRequest = height + adjust;
        }
    }
}
