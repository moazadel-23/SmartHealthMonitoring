using MediatR;
using SmartHealthMonitoring.Application.Common.Interfaces;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Enums;

namespace SmartHealthMonitoring.Application.Common.Behaviors;

/// <summary>
/// Centralized MediatR pipeline behavior that guarantees all sensor reading commands
/// validate against the single Current Patient Selection and resolve the same PatientId.
/// </summary>
public class CurrentPatientValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ICurrentPatientRepository _currentPatientRepository;

    public CurrentPatientValidationBehavior(ICurrentPatientRepository currentPatientRepository)
    {
        _currentPatientRepository = currentPatientRepository;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not IPatientBoundReadingCommand boundCommand)
        {
            return await next(cancellationToken);
        }

        if (typeof(TResponse) != typeof(CreateReadingResult))
        {
            throw new InvalidOperationException(
                $"{typeof(TRequest).Name} implements {nameof(IPatientBoundReadingCommand)} and must return {nameof(CreateReadingResult)}.");
        }

        var selection = await _currentPatientRepository.GetCurrentSelectionAsync(cancellationToken);
        if (selection == null)
        {
            return (TResponse)(object)CreateReadingResult.NoPatientSelected;
        }

        if (selection.Patient == null)
        {
            return (TResponse)(object)CreateReadingResult.PatientNotFound;
        }

        if (boundCommand.PatientId.HasValue && boundCommand.PatientId.Value != selection.PatientId)
        {
            return (TResponse)(object)CreateReadingResult.NotCurrentPatient;
        }

        if (!string.IsNullOrWhiteSpace(boundCommand.SelectionVersion) &&
            !string.Equals(boundCommand.SelectionVersion.Trim(), selection.SelectionVersion, StringComparison.Ordinal))
        {
            return (TResponse)(object)CreateReadingResult.SessionMismatch;
        }

        boundCommand.ResolvedPatientId = selection.PatientId;

        return await next(cancellationToken);
    }
}
