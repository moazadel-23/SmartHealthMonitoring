using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Doctors.Commands.DeleteDoctor
{
    public class DeleteDoctorCommands : IRequest<bool>
    {
        public DeleteDoctorCommands(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
       
    }
}
