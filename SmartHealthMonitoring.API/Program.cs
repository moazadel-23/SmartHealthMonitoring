using Scalar.AspNetCore;
using SmartHealthMonitoring.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on all network interfaces (for ESP32 on Wi-Fi)
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5008);
});

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

app.UseRouting();

app.UseCors("AllowFrontend");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
