namespace SmartHealthMonitoring.Domain.Entities;

public class Measurement
{
    public int Id { get; set; }
    public double HeartRate { get; set; }
    public double SpO2 { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;


    
}
