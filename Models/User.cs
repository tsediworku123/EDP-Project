namespace ClinicAppointmentSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Email { get; set; }  

        public const string Admin = "Admin";
        public const string Doctor = "Doctor";
        public const string Receptionist = "Receptionist";
        public const string Patient = "Patient";

        public bool IsActive { get; set; } = true;
    }
}
