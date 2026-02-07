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
            new OutboxRecordConfiguration().Configure(modelBuilder.Entity<OutboxRecord>());

        if (_includeMessageInbox)
            new InboxRecordConfiguration().Configure(modelBuilder.Entity<InboxRecord>());
    }

    private readonly bool _includeMessageInbox = true;
    private readonly bool _includeMessageOutbox = true;
}