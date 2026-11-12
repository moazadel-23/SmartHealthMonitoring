using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Doctors.Commands.DeleteDoctor
{
    public class DeleteDoctorHandler : IRequestHandler<DeleteDoctorCommands, bool>
    {
        private readonly IDoctorRepository _doctor;

        public DeleteDoctorHandler(IDoctorRepository doctor)
        {
            _doctor = doctor;
        }

        public async Task<bool> Handle(DeleteDoctorCommands request, CancellationToken cancellationToken)
        {
            var doctor = await _doctor.GetOneAsync(request.Id, cancellationToken);
            if(doctor == null)
            {
                return false;
            }
            _doctor.Delete(doctor);
            await _doctor.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
