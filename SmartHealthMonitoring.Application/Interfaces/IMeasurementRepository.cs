using Microsoft.EntityFrameworkCore;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Application.Interfaces;

public interface IMeasurementRepository
{
    Task AddAsync(Measurement measurement, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
