using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Patients.Commands.CreatePatient
{
    public class CreatePatientHandler : IRequestHandler<CreatePatientCommands, bool>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public CreatePatientHandler(
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<bool> Handle(CreatePatientCommands request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorRepository.GetOneAsync(request.DoctorId, cancellationToken);
            if (doctor == null)
            {
                return false;
            }

            await _patientRepository.AddAsync(new Patient
            {
                FName = request.FName,
                LName = request.LName,
                Gender = request.Gender,
                Age = request.Age,
                Address = request.Address,
                Phone = request.Phone,
                DoctorId = request.DoctorId
            }, cancellationToken);

            await _patientRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
