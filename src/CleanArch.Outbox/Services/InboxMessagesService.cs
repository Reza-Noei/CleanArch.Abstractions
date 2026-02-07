using CleanArch.Outbox.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CleanArch.Outbox.Services;

internal class InboxMessagesService<TContext>(TContext context) : IInboxMessagesService
    where TContext : DbContext
{
    public async Task<bool> TryInsertAsync<TMessage>(Guid messageId, TMessage message, DateTime occurredAt) 
        where TMessage : class
    {
        string serializedMessage = JsonSerializer.Serialize(message, message.GetType());

        return await TryInsertAsync(messageId, message.GetType().FullName!, serializedMessage, occurredAt);
    }

    public async Task<bool> TryInsertAsync(Guid messageId, string messageType, string content, DateTime occurredAt)
    {
        bool exists = await context.Set<InboxMessage>()
            .AnyAsync(m => m.Id == messageId);

        if (exists)
            return false;

        var inboxMessage = new InboxMessage
        {
            Id = messageId,
            Type = messageType,
            Content = content,
            OccurredAt = occurredAt
        };

        await context.Set<InboxMessage>().AddAsync(inboxMessage);

        await context.SaveChangesAsync();

        return true;
    }

    public async Task RemoveAsync(Guid messageId)
    {
        var inboxMessages = context.Set<InboxMessage>();
        var message = inboxMessages.FirstOrDefault();
        if (message != null)
        {
            inboxMessages.Remove(message);
            await context.SaveChangesAsync();
        }
    }
}