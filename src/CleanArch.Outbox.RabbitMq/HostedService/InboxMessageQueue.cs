using System.Collections.Concurrent;

namespace CleanArch.Outbox.RabbitMq.HostedService;

internal class InboxMessageQueue : IInboxMessageQueue
{
    public void Enqueue(InboxMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        _queue.Enqueue(message);
        _signal.Release();
    }

    public async Task<InboxMessage?> DequeueAsync(CancellationToken cancellationToken = default)
    {
        await _signal.WaitAsync(cancellationToken);

        if (_queue.TryDequeue(out var message))
            return message;

        return null;
    }

    private SemaphoreSlim _signal = new SemaphoreSlim(0);
    private ConcurrentQueue<InboxMessage> _queue = new ConcurrentQueue<InboxMessage>();
}


public interface IInboxMessageQueue
{
    void Enqueue(InboxMessage message);

    Task<InboxMessage?> DequeueAsync(CancellationToken cancellationToken = default);
}