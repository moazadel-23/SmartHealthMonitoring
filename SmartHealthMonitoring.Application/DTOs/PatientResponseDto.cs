namespace SmartHealthMonitoring.Application.DTOs;

public class PatientResponseDto
{

    public string FName { get; set; } = string.Empty;
    public string LName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
