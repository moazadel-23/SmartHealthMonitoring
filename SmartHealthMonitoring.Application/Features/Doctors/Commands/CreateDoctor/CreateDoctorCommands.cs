using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SmartHealthMonitoring.Application.Features.Doctors.Commands.CreateDoctor;

public class CreateDoctorCommands : IRequest<bool>
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string FName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string LName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number.")]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;
}
