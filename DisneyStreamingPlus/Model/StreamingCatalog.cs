using DisneyHomePageApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyStreamingPlus.Model
{
    internal class StreamingCatalog
    {
        public static async Task<List<Row>> GetImageUrlsAsync()
        {
            var _connector = new DisneyHomePageHttpConnector();
            var HomePage = await _connector.GetHomePageAsJsonAsync();

            //TODO: This should be a separate Row property - but how to do with async?

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
            //TODO: Get real caption
            var row = new Row(tempList, "Hello World Row");
            //TODO: Fish out rows
            var rowList = new List<Row>(1);
            rowList.Add(row);
            return rowList;
        }
    }
}