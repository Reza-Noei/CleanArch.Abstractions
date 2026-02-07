namespace CleanArch.Outbox.RabbitMq;

public sealed class MessageSubscriberOptions
{
    public const string Section = nameof(MessageSubscriberOptions);

    public string ExchangeName { get; set; } = null!;

    public string QueueName { get; set; } = null!;

    public string RoutingKey { get; set; } = null!;

    public ushort PrefetchCount { get; set; }
}
