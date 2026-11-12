using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using SmartHealthMonitoring.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Doctor.DashBoard.LatestVitals.Queries.LatestVitalPatient
{
    public class LatestVitalHandler : IRequestHandler<LatestVitalQuerry, LatestVitalResponse?>
    {
        private readonly IHeartRateRepository _heartRateRepository;
        private readonly ITemperatureRepository _temperatureRepository;
        private readonly IPatientRepository _patientRepository;

        public LatestVitalHandler(
            IHeartRateRepository heartRateRepository,
            ITemperatureRepository temperatureRepository, 
            IPatientRepository patientRepository)
        {
            _heartRateRepository = heartRateRepository;
            _temperatureRepository = temperatureRepository;
            _patientRepository = patientRepository;
        }

        public async Task<LatestVitalResponse?> Handle(LatestVitalQuerry request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetOne(request.PatientId, cancellationToken);

            if (patient == null)
            {
                return null;
            }

            var latestHeartRate = await _heartRateRepository.GetLatestByPatientIdAsync(request.PatientId, cancellationToken);
            var latestTemperature = await _temperatureRepository.GetLatestByPatientIdAsync(request.PatientId, cancellationToken);

            DateTime recordedAt = DateTime.UtcNow;
            if (latestHeartRate != null && latestTemperature != null)
            {
                recordedAt = latestHeartRate.RecordedAt >= latestTemperature.RecordedAt
                    ? latestHeartRate.RecordedAt
                    : latestTemperature.RecordedAt;
            }
            else if (latestHeartRate != null)
            {
                recordedAt = latestHeartRate.RecordedAt;
            }
            else if (latestTemperature != null)
            {
                recordedAt = latestTemperature.RecordedAt;
            }

            return new LatestVitalResponse
            {
                PatientId = patient.Id,
                PatientName = $"{patient.FName} {patient.LName}".Trim(),
                HeartRate = latestHeartRate != null ? (int)Math.Round(latestHeartRate.HeartRateValue) : 0,
                Spo2 = latestHeartRate != null ? (int)Math.Round(latestHeartRate.SpO2) : 0,
                Temperature = latestTemperature != null ? (double)latestTemperature.TempValue : 0.0,
                RecordedAt = recordedAt
            };
        }
    }
}
