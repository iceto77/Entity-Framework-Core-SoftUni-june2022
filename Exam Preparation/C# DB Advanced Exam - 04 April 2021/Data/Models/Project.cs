namespace TeisterMask.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TeisterMask.Data.Common;

    public class Project
    {
        public Project()
        {
            this.Tasks = new HashSet<Task>();        
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.ProjectNameMaxLength)]
        public string Name { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime? DueDate { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }

    }
}
