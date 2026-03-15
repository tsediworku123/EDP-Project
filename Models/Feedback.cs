using System;

namespace ClinicAppointmentSystem.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int? DoctorId { get; set; } // Nullable for clinic feedback
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public int Rating { get; set; } // 1-5 stars
        public string Comment { get; set; }
        public DateTime FeedbackDate { get; set; }
        public string FeedbackType { get; set; } // "Doctor" or "Clinic"
    }
}