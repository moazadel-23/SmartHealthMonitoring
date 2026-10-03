using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using SmartHealthMonitoring.Infrastructure.Persistence;

namespace SmartHealthMonitoring.Infrastructure.Repositories;

public class MeasurementRepository : IMeasurementRepository
{
    private readonly ApplicationDbContext _context;

    public MeasurementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

   
    public async Task AddAsync(Measurement measurement, CancellationToken cancellationToken = default)
    {
        await _context.Measurements.AddAsync(measurement, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
