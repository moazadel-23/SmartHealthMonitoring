using System.Threading;
using System.Threading.Tasks;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Application.Interfaces;

/// <summary>
/// Centralized repository managing the single active CurrentPatientSelection state.
/// </summary>
public interface ICurrentPatientRepository
{
    Task<CurrentPatientSelection?> GetCurrentSelectionAsync(CancellationToken cancellationToken = default);
    Task<CurrentPatientSelection> SetCurrentPatientAsync(int patientId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
