namespace CarDealer.DTO
{
    using Newtonsoft.Json;
    using System;

    [JsonObject]
    public class ExportSalesWithDiscountDto
    {
        [JsonProperty("car")]
        public ExportCarInfoDto Car { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("Discount")]
        public string Discount { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("priceWithDiscount")]
        public string PriceWithDiscount { get; set; }
    }
}
