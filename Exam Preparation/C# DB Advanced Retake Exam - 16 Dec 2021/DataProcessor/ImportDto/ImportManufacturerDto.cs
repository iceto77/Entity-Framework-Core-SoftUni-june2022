namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    
    using Artillery.Data.Common;    

    [XmlType("Manufacturer")]
    public class ImportManufacturerDto
    {
        [XmlElement(nameof(ManufacturerName))]
        [MinLength(GlobalConstants.ManufacturerNameMinLength)]
        [MaxLength(GlobalConstants.ManufacturerNameMaxLength)]
        public string ManufacturerName { get; set; }

        [XmlElement(nameof(Founded))]
        [MinLength(GlobalConstants.ManufacturerFoundedMinLength)]
        [MaxLength(GlobalConstants.ManufacturerFoundedMaxLength)]
        public string Founded { get; set; }
    }
}
