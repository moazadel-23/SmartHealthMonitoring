using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Dashboard.Queries.GetAllCounts
{
    public class GetAllCountsHandler : IRequestHandler<GetAllCountsQuery, DashboardCountsDto>
    {
        private readonly IDoctorRepository _doctor;
        private readonly IPatientRepository _patient;
        private readonly INurseRepository _nurse;

        public GetAllCountsHandler(
            IPatientRepository patient,
            IDoctorRepository doctor,
            INurseRepository nurse)
        {
            _patient = patient;
            _doctor = doctor;
            _nurse = nurse;
        }

        public async Task<DashboardCountsDto> Handle(GetAllCountsQuery request, CancellationToken cancellationToken)
        {
            var patientCount = await _patient.GetPatientCountAsync();
            var doctorCount = await _doctor.GetDoctorCountAsync();
            var nurseCount = await _nurse.GetNurseCountAsync();
            var todayCount = await _patient.GetTodayPatientsAsync(cancellationToken);

            return new DashboardCountsDto(patientCount, doctorCount, nurseCount, todayCount);
        }
    }
}
