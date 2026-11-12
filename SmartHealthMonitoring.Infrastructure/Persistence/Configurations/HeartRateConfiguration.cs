using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Infrastructure.Persistence.Configurations;

public class HeartRateConfiguration : IEntityTypeConfiguration<HeartRate>
{
    public void Configure(EntityTypeBuilder<HeartRate> builder)
    {
        builder.ToTable("HeartRates");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.HeartRateValue)
            .HasColumnName("HeartRate")
            .IsRequired();

        builder.Property(m => m.SpO2)
            .IsRequired();

        builder.Property(m => m.RecordedAt)
            .IsRequired();
    }
}
