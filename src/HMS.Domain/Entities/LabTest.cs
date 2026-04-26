using System;

namespace HMS.Core.Domain.Entities
{
    public class LabTest
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }
        public string TestName { get; set; }
        public string TestCategory { get; set; } // Hematology, Biochemistry, etc.
        public DateTime RequestedDate { get; set; } = DateTime.Now;
        public DateTime? CompletedDate { get; set; }
        public string Status { get; set; } = "Requested"; // Requested, Sample Collected, In Progress, Completed, Cancelled
        public string ResultSummary { get; set; }
        public string ResultDetails { get; set; }
        public string LabTechnicianName { get; set; }
        public string ClinicalNotes { get; set; }
        public bool IsUrgent { get; set; } = false;
        public bool IsCriticalValue { get; set; } = false;
        public string CriticalValueNote { get; set; }
        public string PatientName { get; set; }
        public string RejectionReason { get; set; }
        public string ReferenceRange { get; set; }
        public string NumericalValue { get; set; }
        public string Unit { get; set; } // mg/dL, g/L, etc.
    }
}
