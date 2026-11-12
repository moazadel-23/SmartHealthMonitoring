using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Doctor.DashBoard.LatestVitals.Queries.LatestVitalPatient
{
    public class LatestVitalQuerry : IRequest<LatestVitalResponse?>
    {
        public int PatientId { get; set; }
        public LatestVitalQuerry(int patientId)
        {
            PatientId = patientId;
        }
    }
}
