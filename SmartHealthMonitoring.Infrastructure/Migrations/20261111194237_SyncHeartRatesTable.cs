using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHealthMonitoring.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncHeartRatesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // HeartRates and its keys/indexes (PK_HeartRates, FK_HeartRates_Patients_PatientId, IX_HeartRates_PatientId)
            // are already defined directly in InitialCreate and present on the live database.
            // Retained as an applied migration snapshot alignment without altering or renaming tables.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
