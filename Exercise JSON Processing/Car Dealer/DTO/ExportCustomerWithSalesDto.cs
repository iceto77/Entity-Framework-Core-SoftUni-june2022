namespace CarDealer.DTO
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ExportCustomerWithSalesDto
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("boughtCars")]
        public int BoughtCars { get; set; }

        [JsonProperty("spentMoney")]
        public decimal SpentMoney { get; set; }
    }
}
