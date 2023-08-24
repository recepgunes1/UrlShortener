namespace Shared.DTOs
{
    public class ExpiredUrlDto
    {
        public string LongUrl { get; set; } = null!;
        public DateTime ExpireDateTime { get; set; }
    }
}
