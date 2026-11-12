using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Application.Interfaces;

public interface INurseRepository
{
    Task<IEnumerable<Nurse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Nurse?> GetOneAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Nurse nurse, CancellationToken cancellationToken = default);
    void Update(Nurse nurse);
    void Delete(Nurse nurse);
    Task<int> GetNurseCountAsync();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
