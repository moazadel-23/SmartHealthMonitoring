using MediatR;

namespace SmartHealthMonitoring.Application.Features.Reception.Dashboard.Queries.GetTodayPatientsCount
{
    public record GetTodayPatientsCountQuery() : IRequest<int>;
}
