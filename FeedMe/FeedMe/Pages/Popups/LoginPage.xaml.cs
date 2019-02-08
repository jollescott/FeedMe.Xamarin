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
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FeedMe.Pages.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : PopupPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly TaskCompletionSource<bool> _tcs;

        private ICommand _closeCommand;
        public ICommand CloseCommand => _closeCommand = _closeCommand ?? new Command(RunCloseCommandAsync);

        private async void RunCloseCommandAsync(object obj)
        {
            await PopupNavigation.Instance.PopAsync().ContinueWith((task) => { _tcs.TrySetResult(false); });
        }

        public LoginPage(TaskCompletionSource<bool> tcs)
        {
            InitializeComponent();
            _tcs = tcs;

            MessagingCenter.Instance.Subscribe<Application, string>(Application.Current, "FacebookLogin_Success", async (Application app, string userid) =>
            {
                var user_id = DependencyService.Get<IFacebook>().UserId;
                var user_json = JsonConvert.SerializeObject(new UserDtoV2
                {
                    UserId = user_id
                });

                var content = new StringContent(user_json, Encoding.UTF8, "application/json");
                //await _httpClient.PostAsync(RamseyApi.V2.User.Sync, content);

                await PopupNavigation.Instance.PopAsync().ContinueWith((task) => { _tcs.TrySetResult(true); });
            });

            MessagingCenter.Instance.Subscribe(Application.Current, "FacebookLogin_Cancelled", async (Application app) =>
            {
                await PopupNavigation.Instance.PopAsync().ContinueWith((task) => { _tcs.TrySetResult(false); }); ;
            });

            BindingContext = this;
        }

        public static async Task<bool> AssureFacebookAsync()
        {
            var id = DependencyService.Get<IFacebook>().UserId;

            if (id == null)
            {
                var tcs = new TaskCompletionSource<bool>();
                await PopupNavigation.Instance.PushAsync(new LoginPage(tcs));

                return await tcs.Task;
            }
            else
            {
                return true;
            }
        }
    }
}