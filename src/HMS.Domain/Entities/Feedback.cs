using System;

namespace HMS.Core.Domain.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int Rating { get; set; } // 1-5
        public string Comments { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
