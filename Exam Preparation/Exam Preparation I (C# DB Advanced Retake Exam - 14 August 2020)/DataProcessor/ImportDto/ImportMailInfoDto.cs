namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;
    using SoftJail.Data.Common;

    [JsonObject]
    public class ImportMailInfoDto
    {
        [JsonProperty(nameof(Description))]
        [Required]
        public string Description { get; set; }

        [JsonProperty(nameof(Sender))]
        [Required]
        public string Sender { get; set; }

        [JsonProperty(nameof(Address))]
        [Required]
        [RegularExpression(GlobalConstants.MailAddressRegex)]
        public string Address { get; set; }
    }
}
