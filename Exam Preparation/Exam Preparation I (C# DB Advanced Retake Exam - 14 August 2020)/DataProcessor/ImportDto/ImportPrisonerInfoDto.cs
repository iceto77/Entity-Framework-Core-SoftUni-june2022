﻿namespace SoftJail.DataProcessor.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Prisoner")]
    public class ImportPrisonerInfoDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
