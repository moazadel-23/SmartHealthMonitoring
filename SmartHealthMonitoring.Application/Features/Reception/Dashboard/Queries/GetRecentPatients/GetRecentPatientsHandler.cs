using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Dashboard.Queries.GetRecentPatients
{
    public class GetRecentPatientsHandler : IRequestHandler<GetRecentPatientsQuery, IEnumerable<PatientResponseDto>>
    {
        private readonly IPatientRepository _patientRepository;

        public GetRecentPatientsHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<PatientResponseDto>> Handle(GetRecentPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await _patientRepository.GetRecentPatientsAsync(request.Count, cancellationToken);

            return patients.Select(p => new PatientResponseDto
            {
                Id = p.Id,
                FName = p.FName,
                LName = p.LName,
                CreatedAt = p.CreatedAt,
            });
        }
    }
}
