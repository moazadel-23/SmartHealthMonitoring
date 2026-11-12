using System.ComponentModel.DataAnnotations;
using MediatR;
using SmartHealthMonitoring.Application.DTOs;

namespace SmartHealthMonitoring.Application.Features.Doctor.CurrentPatient.Commands.SetCurrentPatient;

public enum SetCurrentPatientResult
{
    Success,
    PatientNotFound
}

public sealed record SetCurrentPatientResponse(
    SetCurrentPatientResult Status,
    CurrentPatientResponseDto? CurrentPatient = null);

public class SetCurrentPatientCommand : IRequest<SetCurrentPatientResponse>
{
    [Range(1, int.MaxValue, ErrorMessage = "PatientId must be greater than 0.")]
    public int PatientId { get; set; }
}
