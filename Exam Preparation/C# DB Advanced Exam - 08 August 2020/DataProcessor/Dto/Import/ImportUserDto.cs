namespace VaporStore.DataProcessor.Dto.Import
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using VaporStore.Data.Common;

    [JsonObject]
    public class ImportUserDto
    {
        [JsonProperty(nameof(FullName))]
        [Required]
        [RegularExpression(GlobalConstants.UserFullNameRegex)]
        public string FullName { get; set; }

        [JsonProperty(nameof(Username))]
        [Required]
        [MinLength(GlobalConstants.UserUsernameMinLength)]
        [MaxLength(GlobalConstants.UserUsernameMaxLength)]
        public string Username { get; set; }

        [JsonProperty(nameof(Email))]
        [Required]
        public string Email { get; set; }

        [JsonProperty(nameof(Age))]
        [Range(GlobalConstants.UserAgeMinValue, GlobalConstants.UserAgeMaxValue)]
        public int Age { get; set; }

        [JsonProperty(nameof(Cards))]
        public ImportUsersCardDto[] Cards { get; set; }
    }
}
