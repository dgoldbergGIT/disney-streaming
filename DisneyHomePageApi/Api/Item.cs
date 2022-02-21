using Newtonsoft.Json;

namespace DisneyHomePageApi.Api
{
    public class Item
    {
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

                    public Series program;

                    [JsonProperty("default")]
                    public Series DefaultOuter
                    {
                        get { return _series ?? series ?? program; }
                        set { _series = value; }
                    }

                    public class Series
                    {
                        [JsonProperty("default")]
                        public Default DefaultProperty;

                        public class Default
                        {
                            public string MasterWidth;
                            public string MasterHeight;
                            public string Url;
                        }
                    }
                }
            }
        }
    }
}