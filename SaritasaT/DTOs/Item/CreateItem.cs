using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SaritasaT.DTOs.Item
{
    public class CreateItem
    {
        public string? FileName { get; set; }
        public IFormFile? File { get; set; }
        public string? Text { get; set; }
        public int? StorageId { get; set; }
        [DefaultValue(false)]
        public bool IsAutoDelEnabled { get; set; }
    }
}
