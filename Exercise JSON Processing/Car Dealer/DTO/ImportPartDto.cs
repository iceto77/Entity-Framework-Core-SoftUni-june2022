namespace CarDealer.DTO
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ImportPartDto
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(Price))]
        public decimal Price { get; set; }

        [JsonProperty(nameof(Quantity))]
        public int Quantity { get; set; }

        [JsonProperty(nameof(SupplierId))]
        public int SupplierId { get; set; }
    }
}
