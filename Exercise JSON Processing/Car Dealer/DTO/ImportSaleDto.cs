namespace CarDealer.DTO
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ImportSaleDto
    {
        [JsonProperty(nameof(CarId))]
        public int CarId { get; set; }
        
        [JsonProperty(nameof(CustomerId))]
        public int CustomerId { get; set; }

        [JsonProperty(nameof(Discount))]
        public decimal Discount { get; set; }
    }
}
