using System;
using System.Collections.Generic;

namespace SmartHealthMonitoring.Domain.Entities;

public class Patient
{
    public int Id { get; set; }
    public string FName { get; set; } = string.Empty;
    public string LName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int DoctorId { get; set; }

    public Doctor Doctor { get; set; } = null!;
    public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
}
