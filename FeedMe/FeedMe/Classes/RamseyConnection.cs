using System.Threading.Tasks;

namespace FeedMe.Classes
{
    public static class RamseyConnection
    {
        public static Task SaveFavoriteAsync(string recipeID, string userid)
        {
            return Task.CompletedTask;
        }
    }
}
