using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Application.Interfaces;

public interface IDoctorRepository
{
    Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Doctor?> GetOneAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Doctor doctor, CancellationToken cancellationToken = default);
    void Update(Doctor doctor);
    void Delete(Doctor doctor);
    Task<int> GetDoctorCountAsync();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
