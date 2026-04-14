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
    }
}
