namespace TeisterMask.DataProcessor.ImportDto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using TeisterMask.Data.Common;

    [XmlType("Project")]
    public class ImportProjectDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(GlobalConstants.ProjectNameMinLength)]
        [MaxLength(GlobalConstants.ProjectNameMaxLength)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlArray("Tasks")]
        public ImpotTaskDto[] Tasks { get; set; }
    }
}
