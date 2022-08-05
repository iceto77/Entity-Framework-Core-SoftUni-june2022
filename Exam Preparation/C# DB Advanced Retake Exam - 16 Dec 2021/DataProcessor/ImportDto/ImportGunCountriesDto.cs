namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    [JsonObject]
    public class ImportGunCountriesDto
    {
        [JsonProperty(nameof(Id))]
        public int Id { get; set; }
    }
}
