using System;

namespace HMS.Core.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public string Type { get; set; } = "General"; // Appointment, Result, System
    }
}
