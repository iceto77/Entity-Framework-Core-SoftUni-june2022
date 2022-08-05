namespace Theatre.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    [JsonObject]
    public class ImportProjectionDto
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Name { get; set; }

        [JsonProperty(nameof(NumberOfHalls))]
        [Range(typeof(sbyte), "1", "10")]
        [Required]
        public sbyte NumberOfHalls { get; set; }

        [JsonProperty(nameof(Director))]
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Director { get; set; }

        [JsonProperty(nameof(Tickets))]
        public ImportTicketDto[] Tickets { get; set; }

    }
}
