namespace VaporStore.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using VaporStore.Data.Common;

    [JsonObject]
    public class ImportGameDto
    {
        [JsonProperty(nameof(Name))]
        [Required]
        public string Name { get; set; }

        [JsonProperty(nameof(Price))]
        [Range(typeof(decimal), GlobalConstants.GamePriceMinValue, GlobalConstants.GamePriceMaxValue)]
        public decimal Price { get; set; }

        [JsonProperty(nameof(ReleaseDate))]
        public string ReleaseDate { get; set; }

        [JsonProperty(nameof(Developer))]
        public string Developer { get; set; }

        [JsonProperty(nameof(Genre))]
        public string Genre { get; set; }

        [JsonProperty(nameof(Tags))]
        [Required]
        public string[] Tags { get; set; }
    }
}
