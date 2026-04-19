using System;
using System.Collections.Generic;

namespace HMS.Core.Domain.Entities
{
    public class Bill
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int? AppointmentId { get; set; }
        public int? AdmissionId { get; set; } // If hospitalized
        public DateTime BillDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal PaidAmount { get; set; } = 0;
        public decimal TaxAmount { get; set; } = 0;
        public string Status { get; set; } = "Unpaid"; // Unpaid, Partially Paid, Paid, Cancelled, Refunded
        public string InvoiceNumber { get; set; } = $"INV-{DateTime.Now.Ticks}";
        public List<BillItem> Items { get; set; } = new List<BillItem>();
        public string PaymentMethod { get; set; } // Cash, Insurance, CreditCard, BankTransfer
        public string FinancialNotes { get; set; }
        public string HandledBy { get; set; }
    }

    public class BillItem
    {
        public int Id { get; set; }
        public string Description { get; set; } // Consultation fee, Lab Fee, Medicine, Room Rent, etc.
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0;
        public string ItemType { get; set; } // Consultation, Pharmacy, Lab, Service, Other
    }
}
