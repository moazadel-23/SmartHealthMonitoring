using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Doctors.Queries.GetAllDoctors;

public class GetAllDoctorsHandler : IRequestHandler<GetAllDoctorsQuery, IEnumerable<DoctorResponseDto>>
{
    private readonly IDoctorRepository _doctorRepository;

    public GetAllDoctorsHandler(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<IEnumerable<DoctorResponseDto>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
    {
        var doctors = await _doctorRepository.GetAllAsync(cancellationToken);

        return doctors.Select(d => new DoctorResponseDto
        {
            Id = d.Id,
            FName = d.FName,
            LName = d.LName,
            Phone = d.Phone,
            Email = d.Email,
            Specialization = d.Specialization
        });
    }
}
