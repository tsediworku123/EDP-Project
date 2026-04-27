using System;

namespace HMS.Core.Domain.Entities
{
    public class InsuranceClaim
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int PatientId { get; set; }
        public string InsuranceProvider { get; set; }
        public string PolicyNumber { get; set; }
        public string ClaimNumber { get; set; }
        public decimal ClaimAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public DateTime SubmittedDate { get; set; } = DateTime.Now;
        public DateTime? ProcessedDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Approved, Partially Approved, Rejected, Paid
        public string RejectionReason { get; set; }
        public string Notes { get; set; }
    }
}
