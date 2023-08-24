namespace Shared.DTOs
{
    public class ShortedUrlDto
    {
        public string Url { get; set; } = null!;
        public DateTime ExpireDateTime { get; set; }
        public bool IsPublic { get; set; }
    }
}
