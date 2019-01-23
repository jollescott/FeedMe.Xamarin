using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Ramsey.Shared.Misc;
using Ramsey.Shared.Dto.V2;
using Newtonsoft.Json;
using FeedMe.Classes;

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
