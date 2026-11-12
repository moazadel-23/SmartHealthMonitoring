using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Patients.Commands.CreatePatient
{
    public class CreatePatientHandler : IRequestHandler<CreatePatientCommands, int?>
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

        public async Task<int?> Handle(CreatePatientCommands request, CancellationToken cancellationToken)
        {
            SmartHealthMonitoring.Domain.Entities.Doctor? doctor = null;
            if (request.DoctorId > 0)
            {
                doctor = await _doctorRepository.GetOneAsync(request.DoctorId, cancellationToken);
            }

            if (doctor == null)
            {
                var allDoctors = await _doctorRepository.GetAllAsync(cancellationToken);
                doctor = allDoctors.FirstOrDefault();

                if (doctor != null)
                {
                    request.DoctorId = doctor.Id;
                }
                else
                {
                    doctor = new SmartHealthMonitoring.Domain.Entities.Doctor
                    {
                        FName = "General",
                        LName = "Practitioner",
                        Phone = "01000000000",
                        Email = "doctor@smarthealth.com",
                        Specialization = "General Medicine"
                    };
                    await _doctorRepository.AddAsync(doctor, cancellationToken);
                    await _doctorRepository.SaveChangesAsync(cancellationToken);
                    request.DoctorId = doctor.Id;
                }
            }

            var patient = new Patient
            {
                FName = request.FName,
                LName = request.LName,
                Gender = request.Gender,
                Age = request.Age,
                Address = request.Address,
                Phone = request.Phone,
                DoctorId = request.DoctorId,
                CreatedAt = DateTime.UtcNow
            };

            await _patientRepository.AddAsync(patient, cancellationToken);
            await _patientRepository.SaveChangesAsync(cancellationToken);

            return patient.Id;
        }
    }
}
