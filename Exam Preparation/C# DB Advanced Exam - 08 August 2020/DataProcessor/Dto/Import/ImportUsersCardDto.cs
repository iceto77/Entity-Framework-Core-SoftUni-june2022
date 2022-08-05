namespace VaporStore.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using VaporStore.Data.Common;

    [JsonObject]
    public class ImportUsersCardDto
    {
        [JsonProperty(nameof(Number))]
        [Required]
        [RegularExpression(GlobalConstants.CardNumberRegex)]
        public string Number { get; set; }

        [JsonProperty(nameof(CVC))]
        [Required]
        [RegularExpression(GlobalConstants.CardCvcRegex)]
        public string CVC { get; set; }

        [JsonProperty(nameof(Type))]
        public string Type { get; set; }
    }
}
