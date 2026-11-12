using MediatR;
using SmartHealthMonitoring.Domain.Enums;

namespace SmartHealthMonitoring.Application.Common.Interfaces;

/// <summary>
/// Contract for any sensor or vital reading command that must run against
/// the single active Current Patient Selection.
/// </summary>
public interface IPatientBoundReadingCommand : IRequest<CreateReadingResult>
{
    /// <summary>
    /// Optional PatientId sent by the sensor/client for cross-verification.
    /// When specified, it must match the currently active patient; when null,
    /// the centralized validation pipeline assigns the active patient automatically.
    /// </summary>
    int? PatientId { get; }

    /// <summary>
    /// Optional selection version token obtained from GET /api/current-patient.
    /// When specified, rejects stale in-flight readings after a patient switch.
    /// </summary>
    string? SelectionVersion { get; }

    /// <summary>
    /// Authoritative PatientId populated by <see cref="Behaviors.CurrentPatientValidationBehavior{TRequest, TResponse}"/>.
    /// Measurement handlers must persist readings using this value.
    /// </summary>
    int ResolvedPatientId { get; set; }
}
