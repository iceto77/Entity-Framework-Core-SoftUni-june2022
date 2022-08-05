namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    
    using Artillery.Data.Common;

    [XmlType("Shell")]
    public class ImportShellDto
    {
        [XmlElement(nameof(ShellWeight))]
        [Range(GlobalConstants.ShelShellWeightMin, GlobalConstants.ShelShellWeightMax)]
        public string ShellWeight { get; set; }

        [XmlElement(nameof(Caliber))]
        [Required]
        [MinLength(GlobalConstants.ShellCaliberMinLength)]
        [MaxLength(GlobalConstants.ShellCaliberMaxLength)]
        public string Caliber { get; set; }
    }
}
