namespace Footballers.Data.Models
{
    using Footballers.Data.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Coach
    {
        public Coach()
        {
            this.Footballers = new HashSet<Footballer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.CoachNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string Nationality { get; set; }

        public virtual ICollection<Footballer> Footballers { get; set; }
    }
}
