namespace MusicHub.Data.Models
{
    using Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public  class Album
    {
        public Album()
        {
            this.Songs = new HashSet<Song>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.AlbumNameMaxLength)]
        public string Name { get; set; }

        public DateTime ReleaseDate  { get; set; }

        [Required]
        [NotMapped]
        public decimal Price => this.Songs.Count > 0
                  ? this.Songs.Sum(s => s.Price) : 0;

        [ForeignKey(nameof(Producer))]
        public int? ProducerId  { get; set; }
        public virtual Producer Producer { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
