using System;
using HMS.Core.Domain.Interfaces;
using HMS.Core.Persistence.Context;
using HMS.Core.Domain.Entities;

namespace HMS.Core.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HMSDbContext _context;

        public UnitOfWork(HMSDbContext context)
        {
            _context = context;
            Users = new Repository<User>(_context);
            Doctors = new Repository<Doctor>(_context);
            Patients = new Repository<Patient>(_context);
            Appointments = new Repository<Appointment>(_context);
            Feedbacks = new Repository<Feedback>(_context);
            MedicalRecords = new Repository<MedicalRecord>(_context);
            Prescriptions = new Repository<Prescription>(_context);
            LabTests = new Repository<LabTest>(_context);
            AuditLogs = new Repository<AuditLogEntry>(_context);
            Notifications = new Repository<Notification>(_context);
            Nurses = new Repository<Nurse>(_context);
            Pharmacists = new Repository<Pharmacist>(_context);
            InventoryItems = new Repository<InventoryItem>(_context);
            LabTechnicians = new Repository<LabTechnician>(_context);
            Payments = new Repository<Payment>(_context);
            InsuranceClaims = new Repository<InsuranceClaim>(_context);
            BillingStaff = new Repository<BillingStaff>(_context);
            Bills = new Repository<Bill>(_context);
        }

        public IRepository<User> Users { get; private set; }
        public IRepository<Doctor> Doctors { get; private set; }
        public IRepository<Patient> Patients { get; private set; }
        public IRepository<Appointment> Appointments { get; private set; }
        public IRepository<Feedback> Feedbacks { get; private set; }
        public IRepository<MedicalRecord> MedicalRecords { get; private set; }
        public IRepository<Prescription> Prescriptions { get; private set; }
        public IRepository<LabTest> LabTests { get; private set; }
        public IRepository<AuditLogEntry> AuditLogs { get; private set; }
        public IRepository<Notification> Notifications { get; private set; }
        public IRepository<Nurse> Nurses { get; private set; }
        public IRepository<Pharmacist> Pharmacists { get; private set; }
        public IRepository<InventoryItem> InventoryItems { get; private set; }
        public IRepository<LabTechnician> LabTechnicians { get; private set; }
        public IRepository<Payment> Payments { get; private set; }
        public IRepository<InsuranceClaim> InsuranceClaims { get; private set; }
        public IRepository<BillingStaff> BillingStaff { get; private set; }
        public IRepository<Bill> Bills { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
