using System.Collections.Concurrent;

namespace CleanArch.Outbox.RabbitMq.HostedService;

internal class OutboxMessageQueue : IOutboxMessageQueue
{
    public void Enqueue(OutboxMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        _queue.Enqueue(message);
        _signal.Release();
    }

    public async Task<OutboxMessage?> DequeueAsync(CancellationToken cancellationToken = default)
    {
        await _signal.WaitAsync(cancellationToken);

        if (_queue.TryDequeue(out var message))
            return message;

        return null;
    }

    private SemaphoreSlim _signal = new SemaphoreSlim(0);
    private ConcurrentQueue<OutboxMessage> _queue = new ConcurrentQueue<OutboxMessage>();
}

public interface IOutboxMessageQueue
{
    void Enqueue(OutboxMessage message);

    Task<OutboxMessage?> DequeueAsync(CancellationToken cancellationToken = default);
}
