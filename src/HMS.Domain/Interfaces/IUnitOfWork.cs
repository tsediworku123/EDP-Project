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
        
        int Complete();
    }
}
