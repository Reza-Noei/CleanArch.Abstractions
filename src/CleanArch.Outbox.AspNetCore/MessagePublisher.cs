using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace CleanArch.Outbox.RabbitMq;

public sealed class MessagePublisher : IMessagePublisher, IAsyncDisposable
{
    public MessagePublisher(IConnection connection,
                            IOptions<MessagePublisherOptions> options,
                            ILogger<MessagePublisher> logger)
    {
        _connection = connection;
        _options = options.Value;
        _logger = logger;
    }

    private async Task EnsureInitializedAsync(CancellationToken cancellationToken = default)
    {
        if (_isInitialized)
            return;

        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        _isInitialized = true;

        _logger.LogInformation(
            "RabbitMQ Publisher initialized - Exchange: {ExchangeName}, RoutingKey: {RoutingKey}",
            _options.ExchangeName,
            _options.RoutingKey);
    }

    public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : OutboxRecord
    {
        await EnsureInitializedAsync(cancellationToken);

        if (_channel is null)
        {
            throw new InvalidOperationException("RabbitMQ channel not available");
        }

        var body = Encoding.UTF8.GetBytes(message.Content);

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json",
            MessageId = message.Id.ToString(),
            Timestamp = new AmqpTimestamp(
                new DateTimeOffset(message.OccurredAt).ToUnixTimeSeconds()),
            Type = message.Type // Full type name for routing in handler
        };

        await _channel.BasicPublishAsync(
            exchange: _options.ExchangeName,
            routingKey: _options.RoutingKey, // Single routing key
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);

        _logger.LogInformation("Published message {MessageId} (Type: {MessageType}) to exchange {Exchange} with routing key {RoutingKey}",
            message.Id,
            message.Type,
            _options.ExchangeName,
            _options.RoutingKey);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }
    }


    private readonly IConnection _connection;
    private readonly ILogger<MessagePublisher> _logger;
    private readonly MessagePublisherOptions _options;
    private IChannel? _channel;
    private bool _isInitialized;
}
