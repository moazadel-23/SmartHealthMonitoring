namespace SmartHealthMonitoring.Application.DTOs;

public class HeartRateResponseDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public double HeartRate { get; set; }
    public double SpO2 { get; set; }
    public DateTime RecordedAt { get; set; }
}
