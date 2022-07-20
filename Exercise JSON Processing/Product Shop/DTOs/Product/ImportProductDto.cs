namespace ProductShop.DTOs.Product
{
    using System.ComponentModel.DataAnnotations;

    using ProductShop.Common;

    using Newtonsoft.Json;

    [JsonObject]
    public class ImportProductDto
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(GlobalConstants.ProductNameMinLengtgh)]
        public string Name { get; set; }

        [JsonProperty(nameof(Price))]
        public decimal Price { get; set; }

        [JsonProperty(nameof(SellerId))]
        public int SellerId { get; set; }

        [JsonProperty(nameof(BuyerId))]
        public int? BuyerId { get; set; }

    }
}
