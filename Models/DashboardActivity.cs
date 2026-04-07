using System;

namespace ClinicAppointmentSystem.Models
{
    public class DashboardActivity
    {
        public string Title { get; set; } = "Medical Update";
        public string Summary { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
        public string Icon { get; set; } = "MessageMedical";
    }
}
