namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using SoftJail.Data.Common;

    using Newtonsoft.Json;

    [JsonObject]
    public class ImportCellInfoDto
    {
        [JsonProperty(nameof(CellNumber))]
        [Range(GlobalConstants.CellNumberMinValue, GlobalConstants.CellNumberMaxValue)]
        public int CellNumber { get; set; }

        [JsonProperty(nameof(HasWindow))]
        public bool HasWindow { get; set; }
    }
}
