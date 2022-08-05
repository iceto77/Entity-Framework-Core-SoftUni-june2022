namespace Artillery.DataProcessor.ImportDto
{
    using Artillery.Data.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Country")]
    public class ImportCountryDto
    {
        [XmlElement(nameof(CountryName))]
        [MinLength(GlobalConstants.CountryNameMinLength)]
        [MaxLength(GlobalConstants.CountryNameMaxLength)]
        public string CountryName { get; set; }

        [XmlElement(nameof(ArmySize))]
        [Range(GlobalConstants.CountryArmySizeMin, GlobalConstants.CountryArmySizeMax)]
        public int ArmySize { get; set; }
    }
}
