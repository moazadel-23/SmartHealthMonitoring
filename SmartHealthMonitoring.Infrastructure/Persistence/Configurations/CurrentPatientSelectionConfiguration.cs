using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Infrastructure.Persistence.Configurations;

public class CurrentPatientSelectionConfiguration : IEntityTypeConfiguration<CurrentPatientSelection>
{
    public void Configure(EntityTypeBuilder<CurrentPatientSelection> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.PatientId)
            .IsRequired();

        builder.Property(c => c.SelectedAt)
            .IsRequired();

        builder.Ignore(c => c.SelectionVersion);

        builder.HasOne(c => c.Patient)
            .WithMany()
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
