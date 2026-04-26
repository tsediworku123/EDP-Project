namespace HMS.Core.Domain.Entities
{
    public class LabTechnician
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        public string Specialization { get; set; } // Hematology, Biochemistry, Microbiology, etc.
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Department { get; set; } = "Laboratory";
        public bool IsActive { get; set; } = true;
    }
}
