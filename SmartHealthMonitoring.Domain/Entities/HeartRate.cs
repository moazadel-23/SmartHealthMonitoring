using System;

namespace SmartHealthMonitoring.Domain.Entities;

public class HeartRate
{
    public int Id { get; set; }
    public double HeartRateValue { get; set; }
    public double SpO2 { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
}
