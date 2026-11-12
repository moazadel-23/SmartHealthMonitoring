using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using System.Collections.Generic;

namespace SmartHealthMonitoring.Application.Features.Reception.Nurses.Queries.GetAllNurses;

public record GetAllNursesQuery() : IRequest<IEnumerable<NurseResponseDto>>;
