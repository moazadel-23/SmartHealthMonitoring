using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Nurses.Queries.GetNurse;

public class GetNurseHandler : IRequestHandler<GetNurseQuery, NurseResponseDto?>
{
    private readonly INurseRepository _nurseRepository;

    public GetNurseHandler(INurseRepository nurseRepository)
    {
        _nurseRepository = nurseRepository;
    }

    public async Task<NurseResponseDto?> Handle(GetNurseQuery request, CancellationToken cancellationToken)
    {
        var nurse = await _nurseRepository.GetOneAsync(request.Id, cancellationToken);
        if (nurse == null)
        {
            return null;
        }

        return new NurseResponseDto
        {
            Id = nurse.Id,
            FName = nurse.FName,
            LName = nurse.LName,
            Phone = nurse.Phone,
            Email = nurse.Email,
            Specialization = nurse.Specialization,
            Assignment = nurse.Assignment
        };
    }
}
