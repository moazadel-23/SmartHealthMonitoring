using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.DTOs
{
    public class LatestVitalResponse
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public int HeartRate { get; set; }
        public int Spo2 { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
