using Microsoft.EntityFrameworkCore;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using SmartHealthMonitoring.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Infrastructure.Repositories;

public class NurseRepository : INurseRepository
{
    private readonly ApplicationDbContext _context;

    public NurseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Nurse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Nurses.ToListAsync(cancellationToken);
    }

    public async Task<Nurse?> GetOneAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Nurses.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task AddAsync(Nurse nurse, CancellationToken cancellationToken = default)
    {
        await _context.Nurses.AddAsync(nurse, cancellationToken);
    }

    public void Update(Nurse nurse)
    {
        _context.Nurses.Update(nurse);
    }

    public void Delete(Nurse nurse)
    {
        _context.Nurses.Remove(nurse);
    }

    public async Task<int> GetNurseCountAsync()
    {
        return await _context.Nurses.CountAsync();
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
