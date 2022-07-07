namespace P03_FootballBetting.Data.Models
{

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;
    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.UserNameMaxLength)]
        public string Username { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PasswordNameMaxLength)]
        public string Password { get; set; }

        [Required]
        [MaxLength(GlobalConstants.EmailNameMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(GlobalConstants.NameNameMaxLength)]
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
