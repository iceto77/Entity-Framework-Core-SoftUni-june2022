namespace CarDealer.DTO
{    
    using Newtonsoft.Json;

    [JsonObject]
    public class ImportSupplierDto
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(IsImporter))]
        public bool IsImporter { get; set; }
    }
}
