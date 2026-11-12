using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Nurses.Queries.GetAllNurses;

public class GetAllNursesHandler : IRequestHandler<GetAllNursesQuery, IEnumerable<NurseResponseDto>>
{
    private readonly INurseRepository _nurseRepository;

    public GetAllNursesHandler(INurseRepository nurseRepository)
    {
        _nurseRepository = nurseRepository;
    }

    public async Task<IEnumerable<NurseResponseDto>> Handle(GetAllNursesQuery request, CancellationToken cancellationToken)
    {
        var nurses = await _nurseRepository.GetAllAsync(cancellationToken);

        return nurses.Select(n => new NurseResponseDto
        {
            Id = n.Id,
            FName = n.FName,
            LName = n.LName,
            Phone = n.Phone,
            Email = n.Email,
            Specialization = n.Specialization,
            Assignment = n.Assignment
        });
    }
}
