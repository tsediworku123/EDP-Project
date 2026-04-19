using System;

namespace HMS.Core.Domain.Entities
{
    public class DashboardActivity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public string Icon { get; set; } = "Bell";
    }
}
