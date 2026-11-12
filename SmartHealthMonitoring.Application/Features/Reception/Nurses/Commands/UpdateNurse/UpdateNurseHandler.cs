using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Nurses.Commands.UpdateNurse;

public class UpdateNurseHandler : IRequestHandler<UpdateNurseCommands, bool>
{
    private readonly INurseRepository _nurseRepository;

    public UpdateNurseHandler(INurseRepository nurseRepository)
    {
        _nurseRepository = nurseRepository;
    }

    public async Task<bool> Handle(UpdateNurseCommands request, CancellationToken cancellationToken)
    {
        var nurse = await _nurseRepository.GetOneAsync(request.Id, cancellationToken);
        if (nurse == null)
        {
            return false;
        }

        nurse.FName = request.FName;
        nurse.LName = request.LName;
        nurse.Phone = request.Phone;
        nurse.Email = request.Email;
        nurse.Specialization = request.Specialization;
        nurse.Assignment = request.Assignment;

        _nurseRepository.Update(nurse);
        await _nurseRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
