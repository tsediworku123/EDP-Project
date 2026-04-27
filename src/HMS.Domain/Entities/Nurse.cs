using System.Collections.Generic;

namespace HMS.Core.Domain.Entities
{
    public class Nurse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
