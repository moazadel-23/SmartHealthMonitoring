using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Patients.Queries.GetAllPatients;

public class GetAllPatientsHandler : IRequestHandler<GetAllPatientsQuery, IEnumerable<PatientResponseDto>>
{
    private readonly IPatientRepository _patientRepository;

    public GetAllPatientsHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<IEnumerable<PatientResponseDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var patients = await _patientRepository.GetAllAsync(cancellationToken);

        return patients.Select(p => new PatientResponseDto
        {
            Id = p.Id,
            FName = p.FName,
            LName = p.LName,
            Gender = p.Gender,
            Age = p.Age,
            Address = p.Address,
            Phone = p.Phone,
            DoctorId = p.DoctorId,
            CreatedAt = p.CreatedAt
        });
    }
}
