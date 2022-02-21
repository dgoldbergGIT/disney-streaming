using DisneyHomePageApi.Api;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DisneyHomePageApi
{
    public class DisneyHomePageHttpConnector
    {
        //TODO: Put in config file
        private const string _HomePageBaseAddress = "https://cd-static.bamgrid.com/dp-117731241344/";

        private const string _HomePageRelativeAddress = "home.json";
        private const string _RefSetPageRelativeAddressFormatString = "sets/{0}.json";
        private static readonly HttpClient _httpClient;

        static DisneyHomePageHttpConnector()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_HomePageBaseAddress);
        }

        public async Task<HomePage> GetHomePageAsJsonAsync()
        {
            return await GetJsonAsync<HomePage>(_HomePageRelativeAddress);
        }

        public async Task<RefIdPage> GetRefIdPageAsJsonAsync(string refId)
        {
            return await GetJsonAsync<RefIdPage>(string.Format(_RefSetPageRelativeAddressFormatString, refId));
        }

        private async Task<TResult> GetJsonAsync<TResult>(string relativeAddress)
        {
            var httpResponseMessageTask = _httpClient.GetAsync(relativeAddress);
            var httpResponseMessage = httpResponseMessageTask.Result;
            var content = httpResponseMessage.Content;
            var refIdPageString = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(refIdPageString);
        }
    }
}