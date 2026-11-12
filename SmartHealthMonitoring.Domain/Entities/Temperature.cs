using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Domain.Entities
{
    public class Temperature
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public decimal TempValue { get; set; }
        public DateTime RecordedAt { get; set; }
        public Patient Patient { get; set; } = null!;
    }
}
