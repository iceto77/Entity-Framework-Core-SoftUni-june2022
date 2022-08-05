namespace Theatre.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    [JsonObject]
    public class ImportTicketDto
    {
        [JsonProperty(nameof(Price))]
        [Range(typeof(decimal), "1.00", "100.00")]
        public decimal Price { get; set; }

        [JsonProperty(nameof(RowNumber))]
        [Required]
        [Range(typeof(sbyte), "1", "10")]
        public sbyte RowNumber { get; set; }

        [JsonProperty(nameof(PlayId))]
        public int PlayId { get; set; }
    }
}
