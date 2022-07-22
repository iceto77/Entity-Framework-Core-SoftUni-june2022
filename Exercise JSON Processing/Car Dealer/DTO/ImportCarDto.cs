namespace CarDealer.DTO
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    [JsonObject]
    public class ImportCarDto
    {
        [JsonProperty(nameof(Make))]
        public string Make { get; set; }

        [JsonProperty(nameof(Model))]
        public string Model { get; set; }

        [JsonProperty(nameof(TravelledDistance))]
        public long TravelledDistance { get; set; }

        [JsonProperty(nameof(PartsId))]
        public IEnumerable<int> PartsId { get; set; }

    }
}
