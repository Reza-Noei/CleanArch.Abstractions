using Microsoft.EntityFrameworkCore;

namespace CleanArch.Outbox.Extensions;

public static class DbContextOptionBuilderExtensions
{
    extension(DbContextOptionsBuilder builder)
    {
        public void OutboxMessageOnly()
        {
            builder.Options.WithExtension(new OutboxMessageOnlySupportOption());
        }


        public void InboxMessageOnly()
        {
            builder.Options.WithExtension(new OutboxMessageOnlySupportOption());
        }
    }
}
