using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Application.Interfaces;

public interface ITemperatureRepository
{
    Task AddAsync(Temperature reading, CancellationToken cancellationToken = default);
    Task<Temperature?> GetOneAsync(int patientId, CancellationToken cancellationToken = default);
    Task<Temperature?> GetLatestByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Temperature>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
