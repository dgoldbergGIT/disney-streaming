using Newtonsoft.Json;
using System.Collections.Generic;

namespace DisneyHomePageApi.Api
{
    public class Set
    {
        public List<Item> items;
        public Text text;

        public class Text
        {
            public Title title;

            public class Title
            {
                public Full full;

                public class Full
                {
                    public Set set;

                    public class Set
                    {
                        [JsonProperty("default")]
                        public DefaultContent defaultContent;

                        public class DefaultContent
                        {
                            public string content;
                            public string language;
                        }
                    }
                }
            }
        }
    }
}