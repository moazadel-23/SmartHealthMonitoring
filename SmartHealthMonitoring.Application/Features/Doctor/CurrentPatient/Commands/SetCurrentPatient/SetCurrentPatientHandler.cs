using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;

namespace SmartHealthMonitoring.Application.Features.Doctor.CurrentPatient.Commands.SetCurrentPatient;

public class SetCurrentPatientHandler : IRequestHandler<SetCurrentPatientCommand, SetCurrentPatientResponse>
{
    private readonly IPatientRepository _patientRepository;
    private readonly ICurrentPatientRepository _currentPatientRepository;

    public SetCurrentPatientHandler(
        IPatientRepository patientRepository,
        ICurrentPatientRepository currentPatientRepository)
    {
        _patientRepository = patientRepository;
        _currentPatientRepository = currentPatientRepository;
    }

    public async Task<SetCurrentPatientResponse> Handle(SetCurrentPatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetOne(request.PatientId, cancellationToken);
        if (patient == null)
        {
            return new SetCurrentPatientResponse(SetCurrentPatientResult.PatientNotFound);
        }

        var selection = await _currentPatientRepository.SetCurrentPatientAsync(request.PatientId, cancellationToken);
        await _currentPatientRepository.SaveChangesAsync(cancellationToken);

        return new SetCurrentPatientResponse(
            SetCurrentPatientResult.Success,
            CurrentPatientResponseDto.FromSelection(selection, patient));
    }
}
