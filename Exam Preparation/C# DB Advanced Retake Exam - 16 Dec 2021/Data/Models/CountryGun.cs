﻿namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CountryGun
    {
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [ForeignKey(nameof(Gun))]
        public int GunId { get; set; }
        public virtual Gun Gun { get; set; }
    }
}
