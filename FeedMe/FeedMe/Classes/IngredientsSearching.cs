using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Ramsey.Shared.Misc;
using Newtonsoft.Json;
using Ramsey.Shared.Dto.V2;
using Xamarin.Forms;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;

namespace FeedMe.Classes
{
    class IngredientsSearching
    {
        HttpClient httpClient = new HttpClient();

        bool _ongoingSearch = false;
        uint currentSearchNumber = 1;
        uint heighestSearchNumber = 0;
        List<IngredientDtoV2> ingredients = new List<IngredientDtoV2>();

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

        void ReciveIngredients(string searchWord, uint searchNumber)
        {
            try
            {
                Analytics.TrackEvent("search", new Dictionary<string, string> { { "search", searchWord } });

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
            catch (Exception ex) { Crashes.TrackError(ex, new Dictionary<string, string> { { "search", searchWord } }); }

            if (searchNumber == currentSearchNumber)
            {
                _ongoingSearch = false;
            }
        }
    }
}
