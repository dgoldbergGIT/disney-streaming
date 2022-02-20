using System.Collections.Generic;

namespace DisneyStreamingPlus.Model
{
    public class Row
    {
        public List<string> ImageUrls { get; set; }
        public string Caption { get; set; }

        public Row(List<string> imageUrls, string caption)
        {
            ImageUrls = imageUrls;
            Caption = caption;
        }
    }
}