using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;

namespace SmartHealthMonitoring.Application.Features.Doctor.CurrentPatient.Queries.GetCurrentPatient;

public class GetCurrentPatientHandler : IRequestHandler<GetCurrentPatientQuery, CurrentPatientResponseDto?>
{
    private readonly ICurrentPatientRepository _currentPatientRepository;

    public GetCurrentPatientHandler(ICurrentPatientRepository currentPatientRepository)
    {
        _currentPatientRepository = currentPatientRepository;
    }

    public async Task<CurrentPatientResponseDto?> Handle(GetCurrentPatientQuery request, CancellationToken cancellationToken)
    {
        var selection = await _currentPatientRepository.GetCurrentSelectionAsync(cancellationToken);
        if (selection == null)
        {
            return null;
        }

        return CurrentPatientResponseDto.FromSelection(selection);
    }
}
