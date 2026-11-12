using Microsoft.EntityFrameworkCore;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using SmartHealthMonitoring.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ApplicationDbContext _context;

    public DoctorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.ToListAsync(cancellationToken);
    }

    public async Task<Doctor?> GetOneAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.FindAsync(id, cancellationToken);
    }

    public async Task AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        await _context.Doctors.AddAsync(doctor, cancellationToken);
    }

    public void Update(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
    }
    public void Delete(Doctor doctor)
    {
        _context.Doctors.Remove(doctor);
    }

    public async Task<int> GetDoctorCountAsync()
    {
        return await _context.Doctors.CountAsync();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
