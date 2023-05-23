namespace SaritasaT.DTOs.Item
{
    public class Blob
    {
        public string? Name { get; set; }
        public string? Uri { get; set; }
        public string? ContentType { get; set; }
        public Stream? Content { get; set; }
    }
}
