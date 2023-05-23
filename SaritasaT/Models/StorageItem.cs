using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SaritasaT.Models
{
    public class StorageItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ShareURL { get; set; }
        public string? FileName { get; set; }
        public string? Text { get; set; }
        public int? StorageId { get; set; }
        [JsonIgnore]
        public Storage? Storage { get; set; }
        [DefaultValue(false)]
        public bool IsAutoDelEnabled { get; set;}
    }
}
