using DisneyHomePageApi;
using DisneyHomePageApi.Api;
using System.Collections.Generic;
using System.Linq;

namespace DisneyStreamingPlus.Model
{
    internal class StreamingCatalog
    {
        private DisneyHomePageHttpConnector _connector;
        public HomePage HomePage;

        internal StreamingCatalog()
        {
            _connector = new DisneyHomePageHttpConnector();
            // TODO: Make async
            HomePage = _connector.GetHomePageAsJson();
        }

        public List<string> Row
        {
            get
            {
                //var test = HomePage?.data?.standardCollection?.containers?.Select(i => i?.set?.items?.Select(u => u?.image?.tile?.oneDotSevenEightAspectRatio?.DefaultOuter?.defaultProperty?.url)).ToList();
                var test = HomePage?.data?.standardCollection?.containers?.Select(i => i?.set);
                var test2 = test?.Select(t => t.items).Where(t => t != null);

                var tempList = new List<string>();
                foreach (var listOfItmes in test2)
                {
                    var temp = listOfItmes?.Select(v => v?.image?.tile?.oneDotSevenEightAspectRatio?.DefaultOuter?.defaultProperty?.url);
                    if (temp != null)
                    {
                        tempList.AddRange(temp);
                    }
                }

                tempList.RemoveAll(i => i == null);
                return tempList;
            }
        }
    }
}