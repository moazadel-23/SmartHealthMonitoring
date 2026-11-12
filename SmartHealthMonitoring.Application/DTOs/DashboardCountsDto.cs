namespace SmartHealthMonitoring.Application.DTOs
{
    public record DashboardCountsDto(int Patients, int Doctors, int Nurses, int TodayPatients = 0);
}
