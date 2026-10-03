using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.FName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.LName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Gender)
            .HasMaxLength(20);

        builder.Property(p => p.Address)
            .HasMaxLength(200);

        builder.Property(p => p.Phone)
            .HasMaxLength(20);

        // One Patient has many Measurements
        builder.HasMany(p => p.Measurements)
            .WithOne(m => m.Patient)
            .HasForeignKey(m => m.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
