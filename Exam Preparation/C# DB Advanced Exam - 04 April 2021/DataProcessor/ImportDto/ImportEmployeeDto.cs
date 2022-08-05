namespace TeisterMask.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using TeisterMask.Data.Common;

    [JsonObject]
    public class ImportEmployeeDto
    {
        [JsonProperty(nameof(Username))]
        [Required]
        [MinLength(GlobalConstants.EmployeeUsernameMinLength)]
        [MaxLength(GlobalConstants.EmployeeUsernameMaxLength)]
        [RegularExpression(GlobalConstants.EmployeeUsernameRegex)]
        public string Username { get; set; }

        [JsonProperty(nameof(Email))]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [JsonProperty(nameof(Phone))]
        [Required]
        [MaxLength(GlobalConstants.EmployeePhoneMaxLength)]
        [RegularExpression(GlobalConstants.EmployeePhoneRegex)]
        public string Phone { get; set; }

        [JsonProperty(nameof(Tasks))]
        public int[] Tasks { get; set; }
    }
}
