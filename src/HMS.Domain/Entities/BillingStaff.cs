namespace HMS.Core.Domain.Entities
{
    public class BillingStaff
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        public string Position { get; set; } // Senior Accountant, Billing Clerk, etc.
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Department { get; set; } = "Finance/Billing";
        public bool IsActive { get; set; } = true;
    }
}
