namespace SoftJail.DataProcessor.ImportDto
{
    using SoftJail.Data.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Officer")]
    public class ImportOfficersAndPrisonersDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(GlobalConstants.OfficerFullNameMinLength)]
        [MaxLength(GlobalConstants.OfficerFullNameMaxLength)]
        public string FullName { get; set; }

        [XmlElement("Money")]
        [Range(typeof(decimal), GlobalConstants.OfficerSalaryMinValue, GlobalConstants.OfficerSalaryMaxValue)]
        public decimal Salary { get; set; }

        [XmlElement("Position")]
        [Required]
        public string Position { get; set; }

        [XmlElement("Weapon")]
        [Required]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public ImportPrisonerInfoDto[] Prisoners { get; set; }
    }
}
