using FeedMe.Interfaces;
using Newtonsoft.Json;
using Ramsey.Shared.Dto.V2;
using Ramsey.Shared.Misc;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe.Pages.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : PopupPage
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public LoginPage(TaskCompletionSource<bool> tcs)
        {
            InitializeComponent();

            MessagingCenter.Instance.Subscribe<Application, string>(Application.Current, "FacebookLogin_Success", async (Application app, string userid) =>
            {
                var user_id = DependencyService.Get<IFacebook>().UserId;
                var user_json = JsonConvert.SerializeObject(new UserDtoV2
                {
                    UserId = user_id
                });

                var content = new StringContent(user_json, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync(RamseyApi.V2.User.Sync, content);

                tcs.SetResult(true);
                await PopupNavigation.Instance.PopAsync();
            });

            MessagingCenter.Instance.Subscribe(Application.Current, "FacebookLogin_Cancelled", async (Application app) =>
            {
                tcs.SetResult(false);
                await PopupNavigation.Instance.PopAsync();
            });
        }
    }
}