namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using SoftJail.Data.Common;

    using Newtonsoft.Json;

    [JsonObject]
    public class ImportPrisonerAndMailsDto
    {
        [JsonProperty(nameof(FullName))]
        [Required]
        [MinLength(GlobalConstants.PrisonerFullNameMinLength)]
        [MaxLength(GlobalConstants.PrisonerFullNameMaxLength)]
        public string FullName { get; set; }

        [JsonProperty(nameof(Nickname))]
        [Required]
        [RegularExpression(GlobalConstants.PrisonerNicknameRegex)]
        public string Nickname { get; set; }

        [JsonProperty(nameof(Age))]
        [Range(GlobalConstants.PrisonerAgeMinValue, GlobalConstants.PrisonerAgeMaxValue)]
        public int Age { get; set; }

        [JsonProperty(nameof(IncarcerationDate))]
        [Required]
        public string IncarcerationDate { get; set; }

        [JsonProperty(nameof(ReleaseDate))]
        public string ReleaseDate { get; set; }

        [JsonProperty(nameof(Bail))]
        [Range(typeof(decimal), GlobalConstants.PrisonerBailMinValue, GlobalConstants.PrisonerBailMaxValue)]
        public decimal? Bail { get; set; }

        [JsonProperty(nameof(CellId))]
        public int? CellId { get; set; }

        [JsonProperty(nameof(Mails))]
        public ImportMailInfoDto[] Mails { get; set; }
    }
}
