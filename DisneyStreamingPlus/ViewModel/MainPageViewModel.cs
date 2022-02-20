using DisneyStreamingPlus.Model;
using System.Collections.Generic;

namespace DisneyStreamingPlus.ViewModel
{
    public class MainPageViewModel
    {
        public MainPageViewModel()
        {
            //todo: is Rows really what we get here?
            Rows = new NotifyTaskCompletion<List<Row>>(StreamingCatalog.GetImageUrlsAsync());
        }

        //TODO: Do we want just rows here with no logic?
        public NotifyTaskCompletion<List<Row>> Rows { get; set; }
    }
}