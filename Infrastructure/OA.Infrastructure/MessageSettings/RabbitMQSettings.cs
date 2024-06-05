namespace OA.Infrastructure.Messaging
{
    public class RabbitMQSettings
    {
        public string? HostName { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? QueueName { get; set; }
        public SslOptionSettings Ssl { get; set; }
    }
    public class SslOptionSettings
    {
        public bool Enabled { get; set; }
    }

}
