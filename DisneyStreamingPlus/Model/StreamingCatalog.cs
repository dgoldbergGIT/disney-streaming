using DisneyHomePageApi;
using DisneyHomePageApi.Api;
using System;
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
            var listOfContainers = HomePage?.data?.standardCollection?.containers;

            //?.Select(t => t.items);
            // the following shouldn't be necessary now that I'm checking for curatedset?
            //?.Where(t => t != null);

            // TODO: Use group by or a different more advanced way to serialize into JSON. Remember that items[x].type defines the keyword after series
            var listOfRows = new List<Row>();
            foreach (var container in listOfContainers)
            {
                var set = container.set;
                if (set.type == Set.Type.CuratedSet.ToString())
                {
                    var caption = set?.text?.title?.full?.set?.defaultContent?.content;
                    var listOfImageUrls = new List<string>();
                    var listOfItems = set.items;
                    var enumOfImageUrls = listOfItems?.Select(v => v?.image?.tile?.oneDotSevenEightAspectRatio?.DefaultOuter?.DefaultProperty?.Url);
                    if (enumOfImageUrls != null)
                    {
                        listOfImageUrls.AddRange(enumOfImageUrls);
                    }
                    listOfImageUrls.RemoveAll(i => i == null);
                    //TODO: Get real caption
                    listOfRows.Add(new Row(listOfImageUrls, caption));
                }
            }

            return listOfRows;
        }
    }
}