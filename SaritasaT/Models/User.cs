using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SaritasaT.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^([\w\!\#$\%\&\'*\+\-\/\=\?\^`{\|\}\~]+\.)*[\w\!\#$\%\&\'*\+\-\/\=\?\^`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$", ErrorMessage = "Email address must be a valid email address.")]
        public string Email { get; set; }
        [Required]
        [JsonIgnore]
        public string Password { get; set; }
        public Storage? Storage { get; set; }
    }
}
