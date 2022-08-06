namespace Footballers.DataProcessor.ImportDto
{
    using Footballers.Data.Common;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    [JsonObject]
    public class ImportTeamDto
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(GlobalConstants.TeamNameMinLength)]
        [MaxLength(GlobalConstants.TeamNameMaxLength)]
        [RegularExpression(GlobalConstants.TeamNameMaxRegex)]
        public string Name { get; set; }

        [JsonProperty(nameof(Nationality))]
        [Required]
        [MinLength(GlobalConstants.TeamNationalityMinLength)]
        [MaxLength(GlobalConstants.TeamNationalityMaxLength)]
        public string Nationality { get; set; }

        [JsonProperty(nameof(Trophies))]
        public int Trophies { get; set; }

        [JsonProperty(nameof(Footballers))]
        public int[] Footballers { get; set; }
    }
}
