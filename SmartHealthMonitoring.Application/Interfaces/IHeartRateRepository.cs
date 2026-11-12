using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Application.Interfaces;

public interface IHeartRateRepository
{
    Task AddAsync(HeartRate reading, CancellationToken cancellationToken = default);
    Task<HeartRate?> GetOneAsync(int patientId, CancellationToken cancellationToken = default);
    Task<HeartRate?> GetLatestByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<HeartRate>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
