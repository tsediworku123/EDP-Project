using System;

namespace HMS.Core.Domain.Entities
{
    public class AuditLogEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Username { get; set; }
        public string Action { get; set; }
        public string Module { get; set; }
    }
}
