﻿namespace CarDealer.DTO
{
    using System;

    using Newtonsoft.Json;

    [JsonObject]
    public class ImportCustomerDto
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(BirthDate))]
        public DateTime BirthDate { get; set; }

        [JsonProperty(nameof(IsYoungDriver))]
        public bool IsYoungDriver { get; set; }
    }
}
