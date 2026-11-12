using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Nurses.Commands.DeleteNurse;

public class DeleteNurseHandler : IRequestHandler<DeleteNurseCommands, bool>
{
    private readonly INurseRepository _nurseRepository;

    public DeleteNurseHandler(INurseRepository nurseRepository)
    {
        _nurseRepository = nurseRepository;
    }

    public async Task<bool> Handle(DeleteNurseCommands request, CancellationToken cancellationToken)
    {
        var nurse = await _nurseRepository.GetOneAsync(request.Id, cancellationToken);
        if (nurse == null)
        {
            return false;
        }

        _nurseRepository.Delete(nurse);
        await _nurseRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
