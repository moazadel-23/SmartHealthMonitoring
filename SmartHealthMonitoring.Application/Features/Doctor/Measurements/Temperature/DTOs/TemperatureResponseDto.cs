namespace SmartHealthMonitoring.Application.Features.Doctor.Measurements.Temperature.DTOs;

public class TemperatureResponseDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public decimal TempValue { get; set; }
    public DateTime RecordedAt { get; set; }
}
