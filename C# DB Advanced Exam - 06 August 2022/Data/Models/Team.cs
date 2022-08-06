namespace Footballers.Data.Models
{
    using Footballers.Data.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Team
    {
        public Team()
        {
            this.TeamsFootballers = new HashSet<TeamFootballer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.TeamNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(GlobalConstants.TeamNationalityMaxLength)]
        public string Nationality { get; set; }

        public int Trophies { get; set; }

        public virtual ICollection<TeamFootballer> TeamsFootballers  { get; set; }
    }
}
