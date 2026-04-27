using System;

namespace HMS.Core.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int PatientId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public decimal Amount { get; set; } = 0;
        public string PaymentMethod { get; set; } // Cash, Credit Card, Bank Transfer, Insurance, Digital Wallet
        public string TransactionId { get; set; }
        public string Status { get; set; } = "Successful"; // Successful, Failed, Pending, Refunded
        public string Notes { get; set; }
        public string HandledBy { get; set; }
    }
}
