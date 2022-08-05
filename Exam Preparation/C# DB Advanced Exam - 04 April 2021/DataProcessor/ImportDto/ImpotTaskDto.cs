namespace TeisterMask.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using TeisterMask.Data.Common;

    [XmlType("Task")]
    public class ImpotTaskDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(GlobalConstants.TasktNameMinLength)]
        [MaxLength(GlobalConstants.TasktNameMaxLength)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        [Required]
        public string DueDate { get; set; }

        [XmlElement("ExecutionType")]
        [Required]
        public string ExecutionType { get; set; }

        [XmlElement("LabelType")]
        [Required]
        public string LabelType { get; set; }
    }
}
