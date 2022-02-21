using DisneyHomePageApi.Api;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DisneyHomePageApi
{
    public class DisneyHomePageHttpConnector
    {
        private const string _HomePageBaseAddress = "https://cd-static.bamgrid.com/";
        private const string _HomePageRelativeAddress = "dp-117731241344/home.json";
        private const string _RefSetPageRelativeAddressFormatString = "dp-117731241344/sets/{0}.json";
        private static readonly HttpClient _httpClient;

        static DisneyHomePageHttpConnector()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_HomePageBaseAddress);
        }

        public async Task<HomePage> GetHomePageAsJsonAsync()
        {
            var httpResponseMessageTask = _httpClient.GetAsync(_HomePageRelativeAddress);
            var httpResponseMessage = httpResponseMessageTask.Result;
            var content = httpResponseMessage.Content;
            var homePageString = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HomePage>(homePageString);
        }

        public async Task<RefIdPage> GetRefIdPageAsJsonAsync(string refId)
        {
            var httpResponseMessageTask = _httpClient.GetAsync(string.Format(_RefSetPageRelativeAddressFormatString, refId));
            var httpResponseMessage = httpResponseMessageTask.Result;
            var content = httpResponseMessage.Content;
            var refIdPageString = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RefIdPage>(refIdPageString);
        }
    }
}