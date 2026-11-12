using MediatR;

namespace SmartHealthMonitoring.Application.Features.Reception.Nurses.Commands.DeleteNurse;

public class DeleteNurseCommands : IRequest<bool>
{
    public DeleteNurseCommands(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}
