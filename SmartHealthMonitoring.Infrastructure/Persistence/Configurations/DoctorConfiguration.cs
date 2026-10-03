using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Infrastructure.Persistence.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.FName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.LName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Phone)
            .HasMaxLength(20);

        builder.Property(d => d.Email)
            .HasMaxLength(100);

        builder.Property(d => d.Specialization)
            .HasMaxLength(100);

        // One Doctor has many Patients
        builder.HasMany(d => d.Patients)
            .WithOne(p => p.Doctor)
            .HasForeignKey(p => p.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
