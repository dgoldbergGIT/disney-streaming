using DisneyStreamingPlus.Model;
using System.Collections.Generic;

namespace DisneyStreamingPlus.ViewModel
{
    public class MainPageViewModel
    {
        public MainPageViewModel()
        {
            Rows = new NotifyTaskCompletion<List<Row>>(StreamingCatalog.GetListOfRowsAsync());
        }

        public NotifyTaskCompletion<List<Row>> Rows { get; set; }
    }
}