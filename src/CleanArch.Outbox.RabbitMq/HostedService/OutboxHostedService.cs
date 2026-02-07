using CleanArch.Outbox.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArch.Outbox.RabbitMq.HostedService;

internal class OutboxHostedService : BackgroundService
{
    public OutboxHostedService(IServiceProvider serviceProvider, 
                               IOutboxMessageQueue outboxMessageQueue,
                               IMessagePublisher messagePublisher,
                               ILogger<OutboxHostedService> logger)
    {
        // Non-Singleton instances can't be injected into a Singleton service.
        // Thus we need Service provider to create scope for other services.
        _serviceProvider = serviceProvider;

        _outboxMessageQueue = outboxMessageQueue;
        _messagePublisher = messagePublisher;
        
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Parcel Export Hosted Service is running.");

        // Todo: Read and Enqueue all unprocessed messages here. 

        while (!cancellationToken.IsCancellationRequested)
        {
            OutboxMessage? message = await _inboxMessageQueue.DequeueAsync(cancellationToken);

            // Todo 1: Save OutboxMessage.
            // Todo 2: publish Message using RabbitMq.
            // Todo 3: after publish update outbox message.
        }

        _logger.LogInformation("Parcel Export Hosted Service is stopping.");
    }


    private readonly IServiceProvider _serviceProvider;
    private readonly IOutboxMessageQueue _outboxMessageQueue;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<OutboxHostedService> _logger;
}
