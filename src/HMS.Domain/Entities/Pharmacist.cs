namespace HMS.Core.Domain.Entities
{
    public class Pharmacist
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string LicenseNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
