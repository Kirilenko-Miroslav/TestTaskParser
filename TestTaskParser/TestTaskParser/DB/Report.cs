namespace TestTaskParser.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Text.Json.Serialization;

    public partial class Report
    {
        [Key]
        public int ReportId { get; set; }
        [JsonPropertyName("dealNumber")]
        public string dealNumber { get; set; }
        [JsonPropertyName("sellerName")]
        public string sellerName { get; set; }
        [JsonPropertyName("sellerInn")]
        public string sellerInn { get; set; }
        [JsonPropertyName("buyerName")]
        public string buyerName { get; set; }
        [JsonPropertyName("buyerInn")]
        public string buyerInn { get; set; }
        [JsonPropertyName("dealDate")]
        [Column(TypeName = "datetime2")]
        public DateTime dealDate { get; set; }
        [JsonPropertyName("woodVolumeBuyer")]
        public float? woodVolumeBuyer { get; set; }
        [JsonPropertyName("woodVolumeSeller")]
        public float? woodVolumeSeller { get; set; }
    }
}
