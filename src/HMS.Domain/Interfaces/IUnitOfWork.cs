using System;
using HMS.Core.Domain.Entities;

namespace HMS.Core.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Doctor> Doctors { get; }
        IRepository<Patient> Patients { get; }
        IRepository<Appointment> Appointments { get; }
        IRepository<Feedback> Feedbacks { get; }
        IRepository<MedicalRecord> MedicalRecords { get; }
        IRepository<Prescription> Prescriptions { get; }
        IRepository<LabTest> LabTests { get; }
        IRepository<AuditLogEntry> AuditLogs { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<Nurse> Nurses { get; }
        IRepository<Pharmacist> Pharmacists { get; }
        IRepository<InventoryItem> InventoryItems { get; }
        IRepository<LabTechnician> LabTechnicians { get; }
        IRepository<Payment> Payments { get; }
        IRepository<InsuranceClaim> InsuranceClaims { get; }
        IRepository<BillingStaff> BillingStaff { get; }
        IRepository<Bill> Bills { get; }
        IRepository<PatientVital> PatientVitals { get; }
        
        int Complete();
    }
}
