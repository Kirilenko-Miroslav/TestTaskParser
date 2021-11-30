using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TestTaskParser.DB;

namespace TestTaskParser
{          //дополнительные классы для десериализации JSON строки
        public class SearchReportWoodDeal
        {
            [JsonPropertyName("content")]
            public Report[] Content { get; set; }
        }
        public class Data
        {
            [JsonPropertyName("searchReportWoodDeal")]
            public SearchReportWoodDeal SearchReportWoodDeal { get; set; }
        }
        public class Basa
        {
            [JsonPropertyName("data")]
            public Data Data { get; set; }
        }
}
