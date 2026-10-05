using Scalar.AspNetCore;
using SmartHealthMonitoring.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Services from Services folder
builder.Services.AddApplicationAndApiServices(builder.Configuration);

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
