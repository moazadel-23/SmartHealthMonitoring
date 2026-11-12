using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Doctors.Commands.UpdateDoctor;

public class UpdateDoctorHandler : IRequestHandler<UpdateDoctorCommands, bool>
{
    private readonly IDoctorRepository _doctorRepository;

    public UpdateDoctorHandler(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<bool> Handle(UpdateDoctorCommands request, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetOneAsync(request.Id, cancellationToken);
        if (doctor == null)
        {
            return false;
        }

        doctor.FName = request.FName;
        doctor.LName = request.LName;
        doctor.Phone = request.Phone;
        doctor.Email = request.Email;
        doctor.Specialization = request.Specialization;

        _doctorRepository.Update(doctor);
        await _doctorRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
