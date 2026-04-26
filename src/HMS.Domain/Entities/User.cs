namespace HMS.Core.Domain.Entities
{
    public class User
    {
        public const string Admin = "Admin";
        public const string Doctor = "Doctor";
        public const string Patient = "Patient";
        public const string Receptionist = "Receptionist";
        public const string Nurse = "Nurse";
        public const string Pharmacist = "Pharmacist";
        public const string LabTechnician = "LabTechnician";

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Admin, Doctor, Patient, Receptionist, Nurse, Pharmacist
        public string Email { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
        public int? NurseId { get; set; }
        public int? PharmacistId { get; set; }
        public int? LabTechnicianId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
