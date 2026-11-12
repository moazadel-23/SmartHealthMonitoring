using MediatR;
using SmartHealthMonitoring.Application.DTOs;

namespace SmartHealthMonitoring.Application.Features.Reception.Nurses.Queries.GetNurse;

public record GetNurseQuery(int Id) : IRequest<NurseResponseDto?>;
