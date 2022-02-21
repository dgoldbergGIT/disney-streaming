using Newtonsoft.Json;
using System.Collections.Generic;

namespace DisneyHomePageApi.Api
{
    public class RefIdPage
    {
        public Data data;

        public class Data
        {
            private CuratedSetClass _curatedSet;

            public CuratedSetClass trendingSet;

            public CuratedSetClass personalizedCuratedSet;

            public CuratedSetClass CuratedSet
            {
                get { return _curatedSet ?? trendingSet ?? personalizedCuratedSet; }
                set { _curatedSet = value; }
            }

            public class CuratedSetClass
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
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}