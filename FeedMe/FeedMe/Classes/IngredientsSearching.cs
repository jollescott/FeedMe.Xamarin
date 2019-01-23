using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Ramsey.Shared.Misc;
using Newtonsoft.Json;
using Ramsey.Shared.Dto.V2;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace FeedMe.Classes
{
    class IngredientsSearching
    {
        HttpClient httpClient = new HttpClient();

        bool ongoingSearch = false;
        uint currentSearchNumber = 1;
        uint heighestSearchNumber = 0;
        List<IngredientDtoV2> ingredients = new List<IngredientDtoV2>();

        public List<IngredientDtoV2> Search(string searchWord)
        {
            if (ongoingSearch)
            {
                currentSearchNumber++;
            }
            else
            {
                ongoingSearch = true;
                currentSearchNumber = 1;
                heighestSearchNumber = 0;
            }

            ReciveIngredients(searchWord, currentSearchNumber);

            return ingredients;
        }

        void ReciveIngredients(string searchWord, uint searchNumber)
        {
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(RamseyApi.V2.Ingredient.Suggest + "?search=" + searchWord).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode && searchNumber > heighestSearchNumber)
                {
                    var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (searchNumber > heighestSearchNumber)
                    {
                        heighestSearchNumber = searchNumber;
                        ingredients = JsonConvert.DeserializeObject<List<IngredientDtoV2>>(result);
                    }
                }
            }
            catch { }

            if (searchNumber == currentSearchNumber)
            {
                ongoingSearch = false;
            }
        }
    }
}
