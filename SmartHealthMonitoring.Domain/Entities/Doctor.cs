namespace SmartHealthMonitoring.Domain.Entities;

public class Doctor
{
    public int Id { get; set; }
    public string FName { get; set; } = string.Empty;
    public string LName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;

    // Navigation property
    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
