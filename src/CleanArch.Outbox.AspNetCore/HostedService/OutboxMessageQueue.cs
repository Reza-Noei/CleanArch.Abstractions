using System.Collections.Concurrent;

namespace CleanArch.Outbox.RabbitMq.HostedService;

internal class OutboxMessageQueue : IOutboxMessageQueue
{
    public void Enqueue(OutboxRecord message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        _queue.Enqueue(message);
        _signal.Release();
    }

    public async Task<OutboxRecord?> DequeueAsync(CancellationToken cancellationToken = default)
    {
        await _signal.WaitAsync(cancellationToken);

        if (_queue.TryDequeue(out var message))
            return message;

        return null;
    }

    private SemaphoreSlim _signal = new SemaphoreSlim(0);
    private ConcurrentQueue<OutboxRecord> _queue = new ConcurrentQueue<OutboxRecord>();
}

public interface IOutboxMessageQueue
{
    void Enqueue(OutboxRecord message);

    Task<OutboxRecord?> DequeueAsync(CancellationToken cancellationToken = default);
}
