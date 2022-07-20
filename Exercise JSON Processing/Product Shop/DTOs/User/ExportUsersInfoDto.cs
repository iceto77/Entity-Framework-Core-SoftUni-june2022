namespace ProductShop.DTOs.User
{
    using Newtonsoft.Json;
    using System.Linq;

    [JsonObject]
    public class ExportUsersInfoDto
    {
        [JsonProperty("usersCount")]

        //вариянт с automapping, който не работи в judge
        //public int UsersCount { get; set; }
        public int UsersCount
            => this.Users.Any() ? this.Users.Length : 0;

        [JsonProperty("users")]
        public ExportUsersWithFullProductInfoDto[] Users { get; set; }

    }
}
