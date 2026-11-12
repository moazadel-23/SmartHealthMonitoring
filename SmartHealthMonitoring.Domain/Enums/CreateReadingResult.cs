namespace SmartHealthMonitoring.Domain.Enums;

public enum CreateReadingResult
{
    Success,
    NoPatientSelected,
    NotCurrentPatient,
    SessionMismatch,
    PatientNotFound
}
