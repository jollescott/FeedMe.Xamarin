using FeedMe.Interfaces;
using Newtonsoft.Json;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Extensions;
using Ramsey.Shared.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public FavoritesPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var user_id = DependencyService.Get<IFacebook>().UserId;
            var user_json = JsonConvert.SerializeObject(new UserDtoV2
            {
                UserId = user_id
            });

            var content = new StringContent(user_json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(RamseyApi.V2.Favorite.List, content);
            var list_json = await response.Content.ReadAsSwedishStringAsync();

            try
            {
                var favorites = JsonConvert.DeserializeObject<IEnumerator<FavoriteDtoV2>>(list_json);
            }
            catch(Exception ex)
            {

            }
        }
    }
}