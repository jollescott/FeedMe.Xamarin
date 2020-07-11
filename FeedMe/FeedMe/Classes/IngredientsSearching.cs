using Newtonsoft.Json;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Misc;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace FeedMe.Classes
{
    internal class IngredientsSearching
    {
        private readonly HttpClient httpClient = new HttpClient();
        private bool _ongoingSearch = false;
        private uint currentSearchNumber = 1;
        private uint heighestSearchNumber = 0;
        private List<IngredientDtoV2> ingredients = new List<IngredientDtoV2>();

        public List<IngredientDtoV2> Search(string searchWord, out bool ongoingSearch)
        {
            //activityIndicator.IsRunning = true;
            if (_ongoingSearch)
            {
                currentSearchNumber++;
            }
            else
            {
                _ongoingSearch = true;
                currentSearchNumber = 1;
                heighestSearchNumber = 0;
            }

            ReciveIngredients(searchWord, currentSearchNumber);

            ongoingSearch = _ongoingSearch;
            return ingredients;
        }

        private void ReciveIngredients(string searchWord, uint searchNumber)
        {
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(RamseyApi.V2.Ingredient.Suggest + "?search=" + searchWord).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode && searchNumber > heighestSearchNumber)
                {
                    string result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (searchNumber > heighestSearchNumber)
                    {
                        heighestSearchNumber = searchNumber;
                        ingredients = JsonConvert.DeserializeObject<List<IngredientDtoV2>>(result);
                    }
                }
            }
            catch (Exception)
            {

            }

            if (searchNumber == currentSearchNumber)
            {
                _ongoingSearch = false;
            }
        }
    }
}
