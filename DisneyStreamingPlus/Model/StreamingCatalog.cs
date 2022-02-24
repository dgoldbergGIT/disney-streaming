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
            var staticSets = _connector.ParseStaticSets();

            var listOfRows = new List<Row>();
            foreach (var set in staticSets)
            {
                listOfRows.Add(new Row(set.Value, set.Key));
            }

            var dynamicSets = await _connector.ParseDynamicSets();
            foreach (var set in dynamicSets)
            {
                listOfRows.Add(new Row(set.Value, set.Key));
            }

            return listOfRows;
        }
    }
}