namespace ProductShop.DTOs.Category
{
    using System.ComponentModel.DataAnnotations;

    using ProductShop.Common;

    using Newtonsoft.Json;
    public class ImportCategoriesDto
    {
        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }
    }
}
