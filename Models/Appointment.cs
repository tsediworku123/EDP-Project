using System;

namespace ClinicAppointmentSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } = "Scheduled"; // Scheduled, Checked-In, In Progress, Completed, Cancelled, No-Show
        public decimal ConsultationFee { get; set; } = 0;
        public bool IsPaid { get; set; } = false;
        
        public DateTime? CompletionTime { get; set; }
        public int? WaitTimeMinutes { get; set; }
        public string NoShowReason { get; set; }
        public bool IsEmergency { get; set; }
        public string ClinicalNotes { get; set; }
        public string AppointmentType { get; set; } = "Regular";
        public DateTime? CheckInTime { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string QueueNumber { get; set; }
        public string Priority { get; set; } = "Normal";

        // Doctor Notes
        public string ConsultationNote { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty;
        
        // Patient Rating
        public int PatientRating { get; set; } = 0; // 0 = unrated
        public string PatientFeedback { get; set; } = string.Empty;
    }
}
