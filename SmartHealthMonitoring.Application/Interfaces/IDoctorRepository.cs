using SmartHealthMonitoring.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Interfaces;

public interface IDoctorRepository
{
    Task<Doctor?> GetOneAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Doctor doctor, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
