using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaritasaT.Models
{
    public class Storage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int OwnerID { get; set; }
        public User User { get; set; }
        public ICollection<StorageItem> Items { get; } = new List<StorageItem>();
    }
}
