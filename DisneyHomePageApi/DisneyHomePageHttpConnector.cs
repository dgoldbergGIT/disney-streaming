using DisneyHomePageApi.Api;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace DisneyHomePageApi
{
    public class DisneyHomePageHttpConnector
    {
        private const string _HomePageBaseAddress = "https://cd-static.bamgrid.com/";
        private const string _HomePageRelativeAddress = "dp-117731241344/home.json";
        private static readonly HttpClient _httpClient;

        static DisneyHomePageHttpConnector()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_HomePageBaseAddress);
        }

        public HomePage GetHomePageAsJson()
        {
            var httpResponseMessageTask = _httpClient.GetAsync(_HomePageRelativeAddress);
            var httpResponseMessage = httpResponseMessageTask.Result;
            var content = httpResponseMessage.Content;
            var stringContentTask = content.ReadAsStringAsync();
            var result = stringContentTask.Result;
            return JsonConvert.DeserializeObject<HomePage>(result);
        }
    }
}