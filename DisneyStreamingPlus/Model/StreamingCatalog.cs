using DisneyHomePageApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisneyStreamingPlus.Model
{
    internal class StreamingCatalog
    {
        public static async Task<List<Row>> GetListOfRowsAsync()
        {
            var _connector = await DisneyHomePageHttpConnector.CreateAsync();
            var staticSets = _connector.ParseStaticSetsForRowCaptionsAndRowImageUrls();

            var listOfRows = new List<Row>();
            foreach (var set in staticSets)
            {
                listOfRows.Add(new Row(set.Item2, set.Item1));
            }
            var dynamicSets = await _connector.ParseDynamicSetsForRowCaptionsAndRowImageUrls();
            foreach (var set in dynamicSets)
            {
                listOfRows.Add(new Row(set.Item2, set.Item1));
            }

            return listOfRows;
        }
    }
}