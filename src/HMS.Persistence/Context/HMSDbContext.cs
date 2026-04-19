using Microsoft.EntityFrameworkCore;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Core.Persistence.Context
{
    public class HMSDbContext : DbContext
    {
        public HMSDbContext(DbContextOptions<HMSDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<AuditLogEntry> AuditLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillItem> BillItems { get; set; }
        public DbSet<DashboardActivity> DashboardActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure value converters for complex types if necessary
            // For example, Doctor.BlockedTimes (List<DateTime>)
            modelBuilder.Entity<Doctor>()
                .Property(d => d.BlockedTimes)
                .HasConversion(
                    v => string.Join(",", v.Select(t => t.ToString("O"))),
                    v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(t => DateTime.Parse(t))
                          .ToList()
                );

            // Add other configurations if needed
        }
    }
}
