namespace CarDealer.DTO
{
    using System;

    using Newtonsoft.Json;

    [JsonObject]
    public class ExportOrderedCustomersDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("BirthDate")]
        public string BirthDate { get; set; }

        [JsonProperty("IsYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
