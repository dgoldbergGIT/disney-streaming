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
            var homePage = await _connector.GetHomePageAsJsonAsync();

            //TODO: This should be a separate Row property - but how to do with async?

            //var test = HomePage?.data?.standardCollection?.containers?.Select(i => i?.set?.items?.Select(u => u?.image?.tile?.oneDotSevenEightAspectRatio?.DefaultOuter?.defaultProperty?.url)).ToList();
            var listOfContainers = homePage?.data?.standardCollection?.containers;

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
                    var enumerationOfImageUrls = listOfItems?.Select(v => v?.image?.tile?.oneDotSevenEightAspectRatio?.DefaultOuter?.DefaultProperty?.Url);
                    if (enumerationOfImageUrls != null)
                    {
                        listOfImageUrls.AddRange(enumerationOfImageUrls);
                    }
                    listOfImageUrls.RemoveAll(i => i == null);
                    listOfRows.Add(new Row(listOfImageUrls, caption));
                }
                else if (set.type == Set.Type.SetRef.ToString())
                {
                    var refSetPage = await _connector.GetRefIdPageAsJsonAsync(set.refId);
                    var refIdSet = refSetPage.data.CuratedSet;
                    var caption = refIdSet?.text?.title?.full?.set?.defaultContent?.content;
                    var listOfImageUrls = new List<string>();
                    var listOfItems = refIdSet.items;
                    var enumerationOfImageUrls = listOfItems?.Select(v => v?.image?.tile?.oneDotSevenEightAspectRatio?.DefaultOuter?.DefaultProperty?.Url);
                    if (enumerationOfImageUrls != null)
                    {
                        listOfImageUrls.AddRange(enumerationOfImageUrls);
                    }
                    listOfImageUrls.RemoveAll(i => i == null);
                    listOfRows.Add(new Row(listOfImageUrls, caption));
                }
            }

            return listOfRows;
        }

        //private static Row GetRowFromSet ()
    }
}