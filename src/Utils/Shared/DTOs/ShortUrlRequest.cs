namespace Shared.DTOs
{
    public class ShortUrlRequest
    {
        public string Url { get; set; } = null!;
        public int ExpireDate { get; set; }
        public bool IsPublic { get; set; }

    }
}
