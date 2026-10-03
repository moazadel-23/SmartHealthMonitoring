using Scalar.AspNetCore;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Application.Services;
using SmartHealthMonitoring.Infrastructure;
using SmartHealthMonitoring.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure services (ApplicationDbContext)
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddScoped<IMeasurementService, MeasurementService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar/v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
