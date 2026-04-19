using System;
using System.Collections.Generic;

namespace HMS.Core.Domain.Entities
{
    public class Prescription
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }
        public DateTime PrescribedDate { get; set; } = DateTime.Now;
        public List<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();
        public string Notes { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Dispensed, Cancelled
    }

    public class PrescriptionItem
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public string Dosage { get; set; } // e.g., 500mg
        public string Frequency { get; set; } // e.g., Twice daily
        public int DurationDays { get; set; }
        public int Quantity { get; set; }
        public string Instructions { get; set; }
    }
}
