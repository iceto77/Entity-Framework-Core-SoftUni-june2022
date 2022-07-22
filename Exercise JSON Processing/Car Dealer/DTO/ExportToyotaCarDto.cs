namespace CarDealer.DTO
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ExportToyotaCarDto
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Make")] 
        public string Make { get; set; }

        [JsonProperty("Model")] 
        public string Model { get; set; }

        [JsonProperty("TravelledDistance")] 
        public long TravelledDistance { get; set; }
    }
}
