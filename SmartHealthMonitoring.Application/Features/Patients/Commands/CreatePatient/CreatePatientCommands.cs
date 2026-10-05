using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Patients.Commands.CreatePatient
{
    public class CreatePatientCommands : IRequest<bool>
    {
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

        [Range(1, int.MaxValue, ErrorMessage = "DoctorId must be a valid ID.")]
        public int DoctorId { get; set; }

    }
}
