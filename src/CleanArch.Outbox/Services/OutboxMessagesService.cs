using CleanArch.Outbox.Abstractions;

namespace CleanArch.Outbox.Services;

internal class OutboxMessagesService : IOutboxMessagesService
{
    public Task<IEnumerable<OutboxRecord>> GetUnprocessedListAsync()
    {
        throw new NotImplementedException();
    }

    public Task MarkAsProcessedAsync(Guid messageId)
    {
        throw new NotImplementedException();
    }
}
