using FeedMe.Droid.Implementations;
using FeedMe.Interfaces;
using Xamarin.Facebook;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidFacebook))]
namespace FeedMe.Droid.Implementations
{
    public class AndroidFacebook : IFacebook
    {
        public string UserId => AccessToken.CurrentAccessToken?.UserId;
    }
}