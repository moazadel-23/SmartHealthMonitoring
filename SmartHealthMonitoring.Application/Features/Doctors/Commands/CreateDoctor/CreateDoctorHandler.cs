using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Doctors.Commands.CreateDoctor;

public class CreateDoctorHandler : IRequestHandler<CreateDoctorCommands, bool>
{
    private readonly IDoctorRepository _doctorRepository;

    public CreateDoctorHandler(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<bool> Handle(CreateDoctorCommands request, CancellationToken cancellationToken)
    {
        var doctor = new Doctor
        {
            FName = request.FName,
            LName = request.LName,
            Phone = request.Phone,
            Email = request.Email,
            Specialization = request.Specialization
        };

        await _doctorRepository.AddAsync(doctor, cancellationToken);
        await _doctorRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
