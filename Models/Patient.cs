using System;

namespace ClinicAppointmentSystem.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string GrandfatherName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        
        // Administrative details
        public bool IsActive { get; set; } = true;
        public string AssignedDoctorName { get; set; } = "None";

        // New Optional / Custom fields
        public string NationalIdOrPassport { get; set; }
        public string Email { get; set; }
        public string AllergiesOrChronicConditions { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhone { get; set; }
        public string EmergencyContact { get; set; }
        public string MedicalNotes { get; set; }
        public string BloodGroup { get; set; } = "Unknown";
        public string CurrentMedications { get; set; } = "None";
        public string ChronicConditions { get; set; } = "None";
        public string PreferredLanguage { get; set; }
        public string PhotoPath { get; set; }
        public string InsuranceNumber { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string PatientCode { get; set; }
    }
}
