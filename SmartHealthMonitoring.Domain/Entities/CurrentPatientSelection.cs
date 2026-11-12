using System;
using System.Globalization;

namespace SmartHealthMonitoring.Domain.Entities;

public class CurrentPatientSelection
{
    public int Id { get; set; } = 1;
    public int PatientId { get; set; }
    public DateTime SelectedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Deterministic string version derived from the monotonic UTC selection timestamp
    /// to detect stale sensor sessions without precision loss in JSON clients.
    /// </summary>
    public string SelectionVersion => SelectedAt.Ticks.ToString(CultureInfo.InvariantCulture);

    public Patient? Patient { get; set; }
}
