using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartHealthMonitoring.Application.Common.Interfaces;

namespace SmartHealthMonitoring.Application.Common.Models;

/// <summary>
/// Shared base command for sensor readings so patient-binding fields and
/// validation rules are never duplicated across sensor features.
/// </summary>
public abstract class PatientBoundReadingCommandBase : IPatientBoundReadingCommand
{
    [Range(1, int.MaxValue, ErrorMessage = "PatientId must be greater than 0.")]
    public int? PatientId { get; set; }

    public string? SelectionVersion { get; set; }

    [JsonIgnore]
    [BindNever]
    public int ResolvedPatientId { get; set; }
}
