using Microsoft.EntityFrameworkCore;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using SmartHealthMonitoring.Infrastructure.Persistence;

namespace SmartHealthMonitoring.Infrastructure.Repositories;

public class HeartRateRepository : IHeartRateRepository
{
    private readonly ApplicationDbContext _context;

    public HeartRateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(HeartRate reading, CancellationToken cancellationToken = default)
    {
        await _context.HeartRates.AddAsync(reading, cancellationToken);
    }

    public async Task<HeartRate?> GetOneAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.HeartRates.FirstOrDefaultAsync(e => e.PatientId == patientId, cancellationToken);
    }

    public async Task<HeartRate?> GetLatestByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.HeartRates
            .AsNoTracking()
            .Where(e => e.PatientId == patientId)
            .OrderByDescending(e => e.RecordedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<HeartRate>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.HeartRates
            .AsNoTracking()
            .Where(e => e.PatientId == patientId)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
