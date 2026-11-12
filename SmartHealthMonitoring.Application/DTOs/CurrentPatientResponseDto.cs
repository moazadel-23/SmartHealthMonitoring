using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Application.DTOs;

public class CurrentPatientResponseDto
{
    public int PatientId { get; set; }
    public string? PatientName { get; set; }
    public DateTime SelectedAt { get; set; }
    public string SelectionVersion { get; set; } = string.Empty;

    public static CurrentPatientResponseDto FromSelection(CurrentPatientSelection selection, Patient? patient = null)
    {
        var resolvedPatient = patient ?? selection.Patient;
        var fullName = resolvedPatient == null
            ? null
            : $"{resolvedPatient.FName} {resolvedPatient.LName}".Trim();

        return new CurrentPatientResponseDto
        {
            PatientId = selection.PatientId,
            PatientName = string.IsNullOrWhiteSpace(fullName) ? null : fullName,
            SelectedAt = selection.SelectedAt,
            SelectionVersion = selection.SelectionVersion
        };
    }
}
