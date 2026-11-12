using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SmartHealthMonitoring.Application.Features.Reception.Patients.Commands.UpdatePatient;

public enum UpdatePatientResult
{
    Success,
    PatientNotFound,
    DoctorNotFound
}

public class UpdatePatientCommands : IRequest<UpdatePatientResult>
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    public string FName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    public string LName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gender is required")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Age is required")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    public string Phone { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "DoctorId must be a valid ID.")]
    public int DoctorId { get; set; } = 0;
}
