﻿namespace ProductShop.DTOs.User
{
    using ProductShop.DTOs.Product;

    using Newtonsoft.Json;

    [JsonObject]
    public class ExportUsersWithSoldProductsDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("soldProducts")]
        public ExportUserSoldProductsDto[] SoldProducts { get; set; }
    }
}
