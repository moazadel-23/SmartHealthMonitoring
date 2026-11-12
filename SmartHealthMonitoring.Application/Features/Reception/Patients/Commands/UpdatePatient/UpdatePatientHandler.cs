using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Patients.Commands.UpdatePatient;

public class UpdatePatientHandler : IRequestHandler<UpdatePatientCommands, UpdatePatientResult>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;

    public UpdatePatientHandler(
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
    }

    public async Task<UpdatePatientResult> Handle(UpdatePatientCommands request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetOne(request.Id, cancellationToken);
        if (patient == null)
        {
            return UpdatePatientResult.PatientNotFound;
        }

        SmartHealthMonitoring.Domain.Entities.Doctor? doctor = null;

        if (request.DoctorId > 0)
        {
            doctor = await _doctorRepository.GetOneAsync(
                request.DoctorId,
                cancellationToken);

            if (doctor == null)
                return UpdatePatientResult.DoctorNotFound;

            patient.DoctorId = doctor.Id;
        }

       
        patient.FName = request.FName;
        patient.LName = request.LName;
        patient.Gender = request.Gender;
        patient.Age = request.Age;
        patient.Address = request.Address;
        patient.Phone = request.Phone;

        _patientRepository.Update(patient);
        await _patientRepository.SaveChangesAsync(cancellationToken);

        return UpdatePatientResult.Success;
    }
}
