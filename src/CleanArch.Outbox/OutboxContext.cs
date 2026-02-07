using CleanArch.Outbox.Configurations;
using CleanArch.Outbox.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Outbox;

public abstract class OutboxContext : DbContext
{
    public OutboxContext(DbContextOptions<OutboxContext> options) : base(options)
    {
        _includeMessageInbox = options.FindExtension<InboxMessageOnlySupportOption>() != null;
        _includeMessageOutbox = options.FindExtension<OutboxMessageOnlySupportOption>() != null;
    }

    public OutboxContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (_includeMessageOutbox)
            new OutboxMessageConfiguration().Configure(modelBuilder.Entity<OutboxMessage>());

        if (_includeMessageInbox)
            new InboxMessageConfiguration().Configure(modelBuilder.Entity<InboxMessage>());
    }

    private readonly bool _includeMessageInbox = true;
    private readonly bool _includeMessageOutbox = true;
}