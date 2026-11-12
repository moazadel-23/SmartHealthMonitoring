using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartHealthMonitoring.Domain.Entities;
using SmartHealthMonitoring.Domain.Enums;
using System;

namespace SmartHealthMonitoring.Infrastructure.Persistence.Configurations;

public class NurseConfiguration : IEntityTypeConfiguration<Nurse>
{
    public void Configure(EntityTypeBuilder<Nurse> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.FName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(n => n.LName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(n => n.Phone)
            .HasMaxLength(20);

        builder.Property(n => n.Email)
            .HasMaxLength(100);

        builder.Property(n => n.Specialization)
            .HasMaxLength(100);

        var shiftConverter = new ValueConverter<ShiftAssignment, string>(
            v => v.ToString(),
            v => ParseShiftAssignment(v)
        );

        builder.Property(n => n.Assignment)
            .HasConversion(shiftConverter)
            .HasMaxLength(30)
            .IsRequired();
    }

    private static ShiftAssignment ParseShiftAssignment(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return ShiftAssignment.MorningShift;

        if (Enum.TryParse<ShiftAssignment>(value, true, out var result))
            return result;

        var lower = value.ToLowerInvariant();
        if (lower.Contains("night"))
            return ShiftAssignment.NightShift;
        if (lower.Contains("afternoon"))
            return ShiftAssignment.AfternoonShift;
       

        return ShiftAssignment.MorningShift;
    }
}
