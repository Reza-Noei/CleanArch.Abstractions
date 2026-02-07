using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArch.Outbox.RabbitMq.HostedService;

internal class InboxHostedService : BackgroundService
{
    public InboxHostedService(IServiceProvider serviceProvider,
                              IInboxMessageQueue inboxMessageQueue,
                              IMessagePublisher messagePublisher,
                              ILogger<OutboxHostedService> logger)
    {
        // Non-Singleton instances can't be injected into a Singleton service.
        // Thus we need Service provider to create scope for other services.
        _serviceProvider = serviceProvider;

        _inboxMessageQueue = inboxMessageQueue;
        _messagePublisher = messagePublisher;

        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Parcel Export Hosted Service is running.");

        // Todo: Read and Enqueue all unprocessed messages here. 
        // Todo: RabbitMq Subscribe

        while (!cancellationToken.IsCancellationRequested)
        {
            InboxRecord? message = await _inboxMessageQueue.DequeueAsync(cancellationToken);

            // Todo 1: Mediator.Send(message)
            // Todo 2: Update State Inbox Message
        }

        _logger.LogInformation("Parcel Export Hosted Service is stopping.");
    }


    private readonly IServiceProvider _serviceProvider;
    private readonly IInboxMessageQueue _inboxMessageQueue;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<OutboxHostedService> _logger;
}