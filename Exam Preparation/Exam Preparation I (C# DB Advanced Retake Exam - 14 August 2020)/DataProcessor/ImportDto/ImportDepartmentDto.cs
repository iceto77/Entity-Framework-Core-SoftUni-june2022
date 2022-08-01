namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using SoftJail.Data.Common;

    using Newtonsoft.Json;

    [JsonObject]
    public class ImportDepartmentDto
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(GlobalConstants.DepartmentNameMinLength)]
        [MaxLength(GlobalConstants.DepartmentNameMaxLength)]
        public string Name { get; set; }

        [JsonProperty(nameof(Cells))]
        public ImportCellInfoDto[] Cells { get; set; }
    }
}
