using MediatR;

namespace SmartHealthMonitoring.Application.Features.Reception.Patients.Commands.DeletePatient;

public enum DeletePatientResult
{
    Success,
    PatientNotFound,
    PatientCurrentlySelected
}

public class DeletePatientCommands : IRequest<DeletePatientResult>
{
    public DeletePatientCommands(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}
