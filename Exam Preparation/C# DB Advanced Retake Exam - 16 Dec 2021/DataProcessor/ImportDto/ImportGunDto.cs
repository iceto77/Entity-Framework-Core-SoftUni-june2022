namespace Artillery.DataProcessor.ImportDto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Artillery.Data.Common;
    using Artillery.Data.Models.Enums;
    
    using Newtonsoft.Json;

    [JsonObject]
    public class ImportGunDto
    {
        [JsonProperty(nameof(ManufacturerId))]
        public int ManufacturerId { get; set; }

        [JsonProperty(nameof(GunWeight))]
        [Range(GlobalConstants.GunGunWeightMin, GlobalConstants.GunGunWeightMax)]
        public int GunWeight { get; set; }

        [JsonProperty(nameof(BarrelLength))]
        [Range(GlobalConstants.GunBarrelLengthMin, GlobalConstants.GunBarrelLengthMax)]
        public double BarrelLength { get; set; }

        [JsonProperty(nameof(NumberBuild))]
        public int? NumberBuild { get; set; }

        [JsonProperty(nameof(Range))]
        [Range(GlobalConstants.GunRangeMin, GlobalConstants.GunRangeMax)]
        public int Range { get; set; }

        [EnumDataType(typeof(GunType))]
        [Required]
        public string GunType { get; set; }

        [JsonProperty(nameof(ShellId))]
        public int ShellId { get; set; }

        [JsonProperty(nameof(Countries))]
        public ImportGunCountriesDto[] Countries { get; set; }
    }
}
