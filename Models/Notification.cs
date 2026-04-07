using System;

namespace ClinicAppointmentSystem.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string Type { get; set; } // "Info", "Success", "Warning"
    }
}
