using Microsoft.EntityFrameworkCore;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using SmartHealthMonitoring.Infrastructure.Persistence;

namespace SmartHealthMonitoring.Infrastructure.Repositories;

public class CurrentPatientRepository : ICurrentPatientRepository
{
    private readonly ApplicationDbContext _context;

    public CurrentPatientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CurrentPatientSelection?> GetCurrentSelectionAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CurrentPatientSelections
            .AsNoTracking()
            .Include(c => c.Patient)
            .OrderBy(c => c.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CurrentPatientSelection> SetCurrentPatientAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var selection = await _context.CurrentPatientSelections
            .OrderBy(c => c.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var selectedAt = DateTime.UtcNow;

        if (selection == null)
        {
            selection = new CurrentPatientSelection
            {
                Id = 1,
                PatientId = patientId,
                SelectedAt = selectedAt
            };
            await _context.CurrentPatientSelections.AddAsync(selection, cancellationToken);
        }
        else
        {
            // Guarantee monotonic SelectedAt ticks even on rapid consecutive switches.
            if (selectedAt <= selection.SelectedAt)
            {
                selectedAt = selection.SelectedAt.AddTicks(1);
            }

            selection.PatientId = patientId;
            selection.SelectedAt = selectedAt;
            _context.CurrentPatientSelections.Update(selection);
        }

        return selection;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
