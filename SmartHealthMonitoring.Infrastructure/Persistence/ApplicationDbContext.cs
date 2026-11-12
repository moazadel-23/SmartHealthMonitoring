using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SmartHealthMonitoring.Domain.Entities;

namespace SmartHealthMonitoring.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<HeartRate> HeartRates => Set<HeartRate>();
    public DbSet<Nurse> Nurses => Set<Nurse>();
    public DbSet<Temperature> Temperatures => Set<Temperature>();
    public DbSet<CurrentPatientSelection> CurrentPatientSelections => Set<CurrentPatientSelection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
