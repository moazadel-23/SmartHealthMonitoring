using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Infrastructure.Persistence.Configurations;

public class TemperatureConfiguration : IEntityTypeConfiguration<Temperature>
{
    public void Configure(EntityTypeBuilder<Temperature> builder)
    {
        builder.ToTable("Temperatures");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TempValue)
            .IsRequired()
            .HasColumnType("decimal(5,2)");

        builder.Property(t => t.RecordedAt)
            .IsRequired();

        builder.HasOne(t => t.Patient)
            .WithMany(p => p.Temperatures)
            .HasForeignKey(t => t.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
