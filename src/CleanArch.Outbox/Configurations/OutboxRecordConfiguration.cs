using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Outbox.Configurations;

public class OutboxRecordConfiguration : IEntityTypeConfiguration<OutboxRecord>
{
    public void Configure(EntityTypeBuilder<OutboxRecord> builder)
    {
        builder.ToTable("OutboxRecords");

        builder.HasKey(x => x.Id);

        builder.Property(o => o.Content).HasMaxLength(2000).HasColumnType("jsonb");

        builder.HasIndex(x => new { x.ProcessedAt, x.OccurredAt })
                .HasDatabaseName("IX_OutboxRecords_Unprocessed");
    }
}
