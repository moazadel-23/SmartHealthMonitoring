using Microsoft.EntityFrameworkCore;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using SmartHealthMonitoring.Infrastructure.Persistence;

namespace SmartHealthMonitoring.Infrastructure.Repositories;

public class TemperatureRepository : ITemperatureRepository
{
    private readonly ApplicationDbContext _context;

    public TemperatureRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Temperature reading, CancellationToken cancellationToken = default)
    {
        await _context.Temperatures.AddAsync(reading, cancellationToken);
    }

    public async Task<Temperature?> GetOneAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Temperatures
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.PatientId == patientId, cancellationToken);
    }

    public async Task<Temperature?> GetLatestByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Temperatures
            .AsNoTracking()
            .Where(e => e.PatientId == patientId)
            .OrderByDescending(e => e.RecordedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Temperature>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Temperatures
            .AsNoTracking()
            .Where(e => e.PatientId == patientId)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
