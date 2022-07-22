namespace CarDealer.DTO
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ExportPartInfoDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Price")]
        public string Price { get; set; }
    }
}
