using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Doctors.Queries.GetDoctor;

public class GetDoctorHandler : IRequestHandler<GetDoctorQuery, DoctorResponseDto?>
{
    private readonly IDoctorRepository _doctorRepository;

    public GetDoctorHandler(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<DoctorResponseDto?> Handle(GetDoctorQuery request, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetOneAsync(request.Id, cancellationToken);
        if (doctor == null)
        {
            return null;
        }

        return new DoctorResponseDto
        {
            Id = doctor.Id,
            FName = doctor.FName,
            LName = doctor.LName,
            Phone = doctor.Phone,
            Email = doctor.Email,
            Specialization = doctor.Specialization
        };
    }
}
