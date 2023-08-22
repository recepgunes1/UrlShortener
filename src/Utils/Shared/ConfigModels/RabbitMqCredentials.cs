namespace Shared.ConfigModels
{
    public class RabbitMqCredentials
    {
        public string Host { get; set; } = null!;
        public ushort Port { get; set; }
        public string VirtualHost { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
