using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.Application.Features.Reception.Dashboard.Queries.GetAllCounts;
using SmartHealthMonitoring.Application.Features.Reception.Dashboard.Queries.GetRecentPatients;
using SmartHealthMonitoring.Application.Features.Reception.Dashboard.Queries.GetTodayPatientsCount;

namespace SmartHealthMonitoring.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Counts")]
        public async Task<IActionResult> GetAllCounts()
        {
            var result = await _mediator.Send(new GetAllCountsQuery());
            return Ok(result);
        }

        [HttpGet("/api/RecentPatients")]
        public async Task<IActionResult> GetRecentPatients([FromQuery] int count = 3)
        {
            var result = await _mediator.Send(new GetRecentPatientsQuery(count));
            return Ok(result);
        }

        [HttpGet("/api/TodayPatientsCount")]
        public async Task<IActionResult> GetTodayPatientsCount()
        {
            var count = await _mediator.Send(new GetTodayPatientsCountQuery());
            return Ok(new { count });
        }
    }
}
