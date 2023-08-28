namespace Shortener.Infrastructure.Models
{
    public class OutboxMessage
    {
        public OutboxMessage()
        {
            EventDate = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public string EventType { get; set; } = null!;
        public string EventPayload { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public bool IsSent { get; set; }
        public DateTime? SentDate { get; set; }

    }
}
