using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Outbox.Configurations;

public class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable("InboxMessages");

        builder.HasKey(x => x.Id);

        builder.Property(o => o.Content).HasMaxLength(2000).HasColumnType("jsonb");

        builder.HasIndex(x => new { x.ProcessedAt, x.OccurredAt })
            .HasDatabaseName("IX_InboxMessages_Unprocessed");
    }
}
