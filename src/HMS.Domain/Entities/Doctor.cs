using System;
using System.Collections.Generic;

namespace HMS.Core.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public string Department { get; set; } = "General Medicine";
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; } 
        public string Address { get; set; }     
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int SlotDurationMinutes { get; set; } = 30;
        public int BufferMinutes { get; set; } = 5;
        public int SlotStepMinutes { get; set; } = 15;
        public TimeSpan WorkingHoursStart { get; set; } = new TimeSpan(8, 0, 0); 
        public TimeSpan WorkingHoursEnd { get; set; } = new TimeSpan(16, 0, 0); 
        
        public string AssignedShift { get; set; } = "Morning";
        public TimeSpan BreakTimeStart { get; set; } = new TimeSpan(12, 0, 0);
        public TimeSpan BreakTimeEnd { get; set; } = new TimeSpan(13, 0, 0);

        public string WorkingShifts { get; set; } = "Morning, Afternoon";
        public string WorkingDays { get; set; } = "Mon, Tue, Wed, Thu, Fri";
        public string CalendarColor { get; set; } = "#2196F3"; 
        public string PhotoPath { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsOnLeave { get; set; } = false;
        public string CurrentStatus { get; set; } = "Available";
        public List<DateTime> BlockedTimes { get; set; } = new List<DateTime>();
    }
}
