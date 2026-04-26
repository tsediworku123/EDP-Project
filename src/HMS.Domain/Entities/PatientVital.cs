using System;

namespace HMS.Core.Domain.Entities
{
    public class PatientVital
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string RoomNumber { get; set; }
        public string BloodPressure { get; set; }
        public string Temperature { get; set; }
        public string Pulse { get; set; }
        public string SPO2 { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
    }
}
