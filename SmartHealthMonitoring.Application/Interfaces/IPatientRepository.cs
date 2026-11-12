using SmartHealthMonitoring.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Patient?> GetOne(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Patient patient, CancellationToken cancellationToken = default);
        void Update(Patient patient);
        void Delete(Patient patient);
        Task<int> GetPatientCountAsync();
        Task<int> GetTodayPatientsAsync(CancellationToken cancellationToken = default);
        Task<int> GetTodayPatientsCountAsync(CancellationToken cancellationToken = default);
        Task<List<Patient>> GetRecentPatientsAsync(int count = 3, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
