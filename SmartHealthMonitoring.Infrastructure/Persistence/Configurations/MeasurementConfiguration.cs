using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Infrastructure.Persistence.Configurations;

public class MeasurementConfiguration : IEntityTypeConfiguration<Measurement>
{
    public void Configure(EntityTypeBuilder<Measurement> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.HeartRate)
            .IsRequired();

        builder.Property(m => m.SpO2)
            .IsRequired();

        builder.Property(m => m.RecordedAt)
            .IsRequired();
    }
}
