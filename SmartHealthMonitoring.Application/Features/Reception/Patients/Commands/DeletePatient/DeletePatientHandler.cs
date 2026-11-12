using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Patients.Commands.DeletePatient;

public class DeletePatientHandler : IRequestHandler<DeletePatientCommands, DeletePatientResult>
{
    private readonly IPatientRepository _patientRepository;
    private readonly ICurrentPatientRepository _currentPatientRepository;

    public DeletePatientHandler(
        IPatientRepository patientRepository,
        ICurrentPatientRepository currentPatientRepository)
    {
        _patientRepository = patientRepository;
        _currentPatientRepository = currentPatientRepository;
    }

    public async Task<DeletePatientResult> Handle(DeletePatientCommands request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetOne(request.Id, cancellationToken);
        if (patient == null)
        {
            return DeletePatientResult.PatientNotFound;
        }

        var currentSelection = await _currentPatientRepository.GetCurrentSelectionAsync(cancellationToken);
        if (currentSelection != null && currentSelection.PatientId == request.Id)
        {
            return DeletePatientResult.PatientCurrentlySelected;
        }

        _patientRepository.Delete(patient);
        await _patientRepository.SaveChangesAsync(cancellationToken);
        return DeletePatientResult.Success;
    }
}
