using DisneyStreamingPlus.Model;
using System.Collections.Generic;

namespace DisneyStreamingPlus.ViewModel
{
    internal class MainPageViewModel
    {
        private StreamingCatalog _streamingCatalog;

        public MainPageViewModel()
        {
            //todo: move to async
            _streamingCatalog = new StreamingCatalog();
            Caption = "Hello World";
        }

        public string Caption { get; set; }

        public List<Row> Rows
        {
            get
            {
                var row = new Row(Images, "Hello World Row");
                var rowList = new List<Row>(1);
                rowList.Add(row);
                return rowList;
            }
        }

        public List<string> Images
        {
            get
            {
                return _streamingCatalog.Row;
            }
        }
    }
}