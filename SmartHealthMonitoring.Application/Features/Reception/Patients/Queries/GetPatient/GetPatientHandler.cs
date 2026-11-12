using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Patients.Queries.GetPatient;

public class GetPatientHandler : IRequestHandler<GetPatientQuery, PatientResponseDto?>
{
    private readonly IPatientRepository _patientRepository;

    public GetPatientHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<PatientResponseDto?> Handle(GetPatientQuery request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetOne(request.Id, cancellationToken);
        if (patient == null)
        {
            return null;
        }

        return new PatientResponseDto
        {
            Id = patient.Id,
            FName = patient.FName,
            LName = patient.LName,
            Gender = patient.Gender,
            Age = patient.Age,
            Address = patient.Address,
            Phone = patient.Phone,
            CreatedAt = patient.CreatedAt,
            DoctorId = patient.DoctorId
        };
    }
}
