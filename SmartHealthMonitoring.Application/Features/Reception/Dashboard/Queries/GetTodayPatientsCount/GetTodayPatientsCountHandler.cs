using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Dashboard.Queries.GetTodayPatientsCount
{
    public class GetTodayPatientsCountHandler : IRequestHandler<GetTodayPatientsCountQuery, int>
    {
        private readonly IPatientRepository _patientRepository;

        public GetTodayPatientsCountHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<int> Handle(GetTodayPatientsCountQuery request, CancellationToken cancellationToken)
        {
            return await _patientRepository.GetTodayPatientsAsync(cancellationToken);
        }
    }
}
