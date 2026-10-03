using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.DTOs
{
    public class ReadingDto
    {
        public int PatientId { get; set; }
        public double HeartRate { get; set; }
        public double SpO2 { get; set; }
    }
}
