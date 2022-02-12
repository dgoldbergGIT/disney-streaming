using Newtonsoft.Json;

namespace DisneyHomePageApi.Api
{
    public class Item
    {
        public string contentId;
        public Image image;

        public class Image
        {
            public Tile tile;

            public class Tile
            {
                [JsonProperty("1.78")]
                public OneDotSevenEightAspectRatio oneDotSevenEightAspectRatio;

                public class OneDotSevenEightAspectRatio
                {
                    private Series _series;

                    public Series series;

                    [JsonProperty("default")]
                    public Series DefaultOuter
                    {
                        get { return _series ?? series; }
                        set { _series = value; }
                    }

                    public class Series
                    {
                        [JsonProperty("default")]
                        public Default defaultProperty;

                        public class Default
                        {
                            public string masterId;
                            public string masterWidth;
                            public string masterHeight;
                            public string url;
                        }
                    }
                }
            }
        }
    }
}