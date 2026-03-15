namespace ClinicAppointmentSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int PatientId { get; set; }
        public string Email { get; set; }  // Make sure this line exists
    }
}