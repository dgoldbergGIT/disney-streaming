using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DisneyHomePageApi
{
    public class DisneyHomePageHttpConnector
    {
        //TODO: Put in config file
        private const string _HomePageBaseAddress = @"https://cd-static.bamgrid.com/dp-117731241344/";

        private const string _HomePageRelativeAddress = "home.json";
        private const string _RefSetPageRelativeAddressFormatString = "sets/{0}.json";
        private dynamic _ContainerSet;
        private readonly HttpClient _httpClient;

        private DisneyHomePageHttpConnector()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_HomePageBaseAddress);
        }

        public static Task<DisneyHomePageHttpConnector> CreateAsync()
        {
            var ret = new DisneyHomePageHttpConnector();
            return ret.InitializeAsync();
        }

        // TODO: For readability, this should probably be a class rather than a Tuple
        public List<Tuple<string, List<string>>> ParseStaticSetsForRowCaptionsAndRowImageUrls()
        {
            var captionAndUrlTupleList = new List<Tuple<string, List<string>>>();
            foreach (dynamic container in _ContainerSet)
            {
                dynamic set = container.set;
                if (set.type == "CuratedSet")
                {
                    string caption = set.text.title.full.set.@default.content;
                    var listOfUrlStrings = GetListOfUrlStrings(set.items);
                    captionAndUrlTupleList.Add(new Tuple<string, List<string>>(caption, listOfUrlStrings));
                }
            }
            return captionAndUrlTupleList;
        }

        // TODO: I'm only separating ParseDynamicSets from ParseStaticSets method as a baby baby step to eventually support dynamic SetRef loading on the UI
        public async Task<List<Tuple<string, List<string>>>> ParseDynamicSetsForRowCaptionsAndRowImageUrls()
        {
            var captionAndUrlTupleList = new List<Tuple<string, List<string>>>();

            foreach (dynamic container in _ContainerSet)
            {
                dynamic set = container.set;
                if (set.type == "SetRef")
                {
                    string caption = set.text.title.full.set.@default.content;
                    var refIdUrlString = GetRefIdUrlString(set.refId);
                    var refSetPageAsString = await GetWebPageAsStringAsync(refIdUrlString);
                    dynamic json = JToken.Parse(refSetPageAsString);
                    dynamic items = json?.SelectToken("$.data.*.items");
                    var listOfUrlStrings = GetListOfUrlStrings(items);
                    captionAndUrlTupleList.Add(new Tuple<string, List<string>>(caption, listOfUrlStrings));
                }
            }

            return captionAndUrlTupleList;
        }

        private async Task<DisneyHomePageHttpConnector> InitializeAsync()
        {
            _ContainerSet = await GetHomePageAsContainerSetAsync(_HomePageRelativeAddress);
            return this;
        }

        private async Task<string> GetWebPageAsStringAsync(string relativeAddress)
        {
            var httpResponseMessageTask = _httpClient.GetAsync(relativeAddress);
            var httpResponseMessage = httpResponseMessageTask.Result;
            var content = httpResponseMessage.Content;
            var pageAsString = await content.ReadAsStringAsync();
            return pageAsString;
        }

        private async Task<dynamic> GetHomePageAsContainerSetAsync(string relativeAddress)
        {
            var pageAsString = await GetWebPageAsStringAsync(relativeAddress);
            dynamic json = JToken.Parse(pageAsString);
            dynamic containers = json.data.StandardCollection.containers;
            return containers;
        }

        private string GetRefIdUrlString(dynamic refId)
        {
            var refIdString = (string)refId;
            return string.Format(_RefSetPageRelativeAddressFormatString, refIdString);
        }

        private List<string> GetListOfUrlStrings(dynamic items)
        {
            var listOfUrls = new List<string>();
            foreach (dynamic item in items)
            {
                var url = (string)item?.SelectToken("$.image.tile.['1.78'].*.default.url");
                listOfUrls.Add(url);
            }
            return listOfUrls;
        }
    }
}