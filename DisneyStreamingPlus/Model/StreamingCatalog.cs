using DisneyHomePageApi;
using DisneyHomePageApi.Api;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyStreamingPlus.Model
{
    internal class StreamingCatalog
    {
        public static async Task<List<Row>> GetListOfRowsAsync()
        {
            var _connector = new DisneyHomePageHttpConnector();
            var homePage = await _connector.GetHomePageAsJsonAsync();

            var listOfContainers = homePage?.data?.standardCollection?.containers;

            // TODO: Use group by or a different more advanced way to serialize into JSON. Remember that items[x].type defines the keyword after series
            var listOfRows = new List<Row>();
            foreach (var container in listOfContainers)
            {
                var set = container.set;
                if (set == null)
                    continue;
                if (set.type == Set.Type.CuratedSet.ToString())
                {
                    var listOfItems = set.items;
                    var imageUrls = ListOfItemsToImageUrlStrings(listOfItems);
                    if (imageUrls == null)
                        continue;
                    var caption = set.text?.title?.full?.set?.defaultContent?.content;
                    listOfRows.Add(new Row(imageUrls.ToList(), caption));
                }
                else if (set.type == Set.Type.SetRef.ToString())
                {
                    var refSetPage = await _connector.GetRefIdPageAsJsonAsync(set.refId);
                    var refIdSet = refSetPage.data.CuratedSet;

                    var listOfItems = refIdSet.items;
                    var imageUrls = ListOfItemsToImageUrlStrings(listOfItems);
                    if (imageUrls == null)
                        continue;
                    var caption = refIdSet.text?.title?.full?.set?.defaultContent?.content;
                    listOfRows.Add(new Row(imageUrls.ToList(), caption));
                }
            }

            return listOfRows;
        }

        //TODO: Use interfaces on Sets so I can do more in this method
        private static IEnumerable<string> ListOfItemsToImageUrlStrings(List<Item> listOfItems)
        {
            return listOfItems
                ?.Select(v => v?.image?.tile?.oneDotSevenEightAspectRatio?.DefaultOuter?.DefaultProperty?.Url)
                ?.Where(v => v != null);
        }
    }
}