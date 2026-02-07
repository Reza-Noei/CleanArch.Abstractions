namespace CleanArch.Outbox.RabbitMq;

public sealed class MessagePublisherOptions
{
    public const string Section = nameof(MessagePublisherOptions);

    public string ExchangeName { get; set; } = null!;

    public string RoutingKey { get; set; } = null!;
}
