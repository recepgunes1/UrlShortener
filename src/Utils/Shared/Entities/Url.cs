namespace Shared.Entities
{
    public class Url
    {
        public Guid Id { get; set; }
        public string LongUrl { get; set; } = null!;
        public string? ShortPath { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastRequestedDate { get; set; } // Nullable because it might not be set initially
        public int RequestCounter { get; set; }
        public DateTime? ExpireDate { get; set; } // Nullable because it might not be set initially
        public bool IsPublic { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Url: {LongUrl}, ShortPath: {ShortPath}, CreatedDate: {CreatedDate},{Environment.NewLine}" +
                $"LastRequestedDate: {LastRequestedDate}, RequestCounter: {RequestCounter},{Environment.NewLine}" +
                $"ExpireDate: {ExpireDate}, IsPublic: {IsPublic}{Environment.NewLine}";
        }
    }
}
