namespace CarDealer.DTO
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    [JsonObject]
    public class ExportCarAndPartsDto
    {
        [JsonProperty("car")]
        public ExportCarInfoDto Car { get; set; } 

        [JsonProperty("parts")]
        public ICollection<ExportPartInfoDto> Parts { get; set; }

    }
}
