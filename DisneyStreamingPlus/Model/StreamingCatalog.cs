using DisneyHomePageApi;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyStreamingPlus.Model
{
    internal class StreamingCatalog
    {
        public static async Task<ObservableCollection<GroupInfoList>> GetImagesGroupedAsync()
        {
            var _connector = await DisneyHomePageHttpConnector.CreateAsync();
            var staticSets = _connector.ParseStaticSets();
            var dynamicSets = await _connector.ParseDynamicSets();

            // TODO: union equality have the correct string comparision? (ordinal I think...)
            var allSets = new Dictionary<string, List<string>>(staticSets.Union(dynamicSets));

            var query = from row in allSets
                        from imageUrl in row.Value
                        group imageUrl by row.Key into g
                        select new { GroupName = g.Key, Items = g };

            var groups = new ObservableCollection<GroupInfoList>();
            foreach (var g in query)
            {
                var groupInfoList = new GroupInfoList();
                groupInfoList.Key = g.GroupName;
                foreach (var item in g.Items)
                {
                    groupInfoList.Add(item);
                }
                groups.Add(groupInfoList);
            }
            return groups;
        }
    }
}