using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Outbox.Configurations;

public class InboxRecordConfiguration : IEntityTypeConfiguration<InboxRecord>
{
    public void Configure(EntityTypeBuilder<InboxRecord> builder)
    {
        builder.ToTable("InboxRecords");

        builder.HasKey(x => x.Id);

        builder.Property(o => o.Content).HasMaxLength(2000).HasColumnType("jsonb");

        builder.HasIndex(x => new { x.ProcessedAt, x.OccurredAt })
            .HasDatabaseName("IX_InboxRecords_Unprocessed");
    }
}
