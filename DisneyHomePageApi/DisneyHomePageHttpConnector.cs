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
        public List<Tuple<string, List<string>>> ParseStaticSetsForRowCaptionAndImageUrls()
        {
            var captionAndUrlTupleList = new List<Tuple<string, List<string>>>();
            if (_ContainerSet != null)
            {
                foreach (dynamic container in _ContainerSet)
                {
                    dynamic set = container.set;
                    if (set != null && set.type == "CuratedSet")
                    {
                        string caption = set.text.title.full.set.@default.content;
                        var listOfUrlStrings = GetListOfUrlStrings(set.items);
                        captionAndUrlTupleList.Add(new Tuple<string, List<string>>(caption, listOfUrlStrings));
                    }
                }
            }
            return captionAndUrlTupleList;
        }

        // NOTE: I'm only separating ParseDynamicSets from ParseStaticSets method as a baby baby step to eventually support dynamic SetRef loading on the UI
        public async Task<List<Tuple<string, List<string>>>> ParseDynamicSetsForRowCaptionAndImageUrls()
        {
            var captionAndUrlTupleList = new List<Tuple<string, List<string>>>();

            if (_ContainerSet != null)
            {
                foreach (dynamic container in _ContainerSet)
                {
                    dynamic set = container.set;
                    if (set != null && set.type == "SetRef")
                    {
                        string caption = set.text.title.full.set.@default.content;
                        var refIdUrlString = GetRefIdUrlString(set.refId);
                        // Download the JSON from the website with the RefId
                        var refSetPageAsString = await GetWebPageAsStringAsync(refIdUrlString);
                        dynamic json = JToken.Parse(refSetPageAsString);
                        dynamic items = json?.SelectToken("$.data.*.items");
                        var listOfUrlStrings = GetListOfUrlStrings(items);
                        captionAndUrlTupleList.Add(new Tuple<string, List<string>>(caption, listOfUrlStrings));
                    }
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
            if (items != null)
            {
                foreach (dynamic item in items)
                {
                    // There are three valid keywords where the * is in the path below, but we really don't care what keyword is there, we just want to keep going to get the Url.
                    // Using ['1.78'] so that we can access the dot as part of the string
                    var url = (string)item?.SelectToken("$.image.tile.['1.78'].*.default.url");
                    listOfUrls.Add(url);
                }
            }
            return listOfUrls;
        }
    }
}