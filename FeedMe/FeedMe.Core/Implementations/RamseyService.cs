using FeedMe.Core.Interfaces;
using Ramsey.Shared.Misc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FeedMe.Core.Services
{
    public class RamseyService : IRamseyService
    {
        private readonly HttpClient _httpClient;

        public RamseyService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var result = await _httpClient.GetAsync(RamseyApi.Base);
                return result.IsSuccessStatusCode;
            }
            catch (HttpRequestException) 
            {
                return false;
            }
        }
    }
}
