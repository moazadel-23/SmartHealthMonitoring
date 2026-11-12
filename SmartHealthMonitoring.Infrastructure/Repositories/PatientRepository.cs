using Microsoft.EntityFrameworkCore;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using SmartHealthMonitoring.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Patients.ToListAsync(cancellationToken);
        }

        public async Task<Patient?> GetOne(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Patients.FindAsync(id, cancellationToken);
        }

        public async Task AddAsync(Patient patient, CancellationToken cancellationToken = default)
        {
            await _context.Patients.AddAsync(patient, cancellationToken);
        }

        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);
        }

        public void Delete(Patient patient)
        {
            _context.Patients.Remove(patient);
        }

        public async Task<int> GetPatientCountAsync()
        {
            return await _context.Patients.CountAsync();
        }

        public async Task<int> GetTodayPatientsAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            return await _context.Patients
                .CountAsync(e => e.CreatedAt >= today && e.CreatedAt < tomorrow, cancellationToken);
        }

        public async Task<int> GetTodayPatientsCountAsync(CancellationToken cancellationToken = default)
            => await GetTodayPatientsAsync(cancellationToken);

        public async Task<List<Patient>> GetRecentPatientsAsync(int count = 3, CancellationToken cancellationToken = default)
        {
            return await _context.Patients
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
