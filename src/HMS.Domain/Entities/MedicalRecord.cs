using System;

namespace HMS.Core.Domain.Entities
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string MedicalNotes { get; set; }
        public string Prescription { get; set; }
        public string TestResults { get; set; } = "None";
        public string DoctorName { get; set; }
        public string FilePath { get; set; }
    }
}
