using SmartHealthMonitoring.Domain.Enums;

namespace SmartHealthMonitoring.Domain.Entities;

public class Nurse
{
    public int Id { get; set; }
    public string FName { get; set; } = string.Empty;
    public string LName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public ShiftAssignment Assignment { get; set; }
}