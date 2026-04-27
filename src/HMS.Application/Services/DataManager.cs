using HMS.Core.Domain.Entities;
using HMS.Core.Persistence;
using HMS.Core.Persistence.Repositories;
using HMS.Core.Domain.Interfaces;
using HMS.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HMS.Core.Persistence.Context;

namespace HMS.Core.AppLogic.Services
{
    public static class DataManager
    {
        public static List<Doctor> Doctors { get; set; } = new List<Doctor>();
        public static List<Patient> Patients { get; set; } = new List<Patient>();
        public static List<User> Users { get; set; } = new List<User>();
        public static List<string> Departments { get; set; } = new List<string> { "General Medicine", "Pediatrics", "Cardiology", "Dermatology", "Neurology", "Orthopedics", "Psychiatry", "ENT", "Radiology", "Dental", "Gynecology" };
        public static List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public static List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public static List<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public static List<Notification> Notifications { get; set; } = new List<Notification>();
        public static List<AuditLogEntry> AuditLogs { get; set; } = new List<AuditLogEntry>();
        public static List<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public static List<LabTest> LabTests { get; set; } = new List<LabTest>();
        public static List<Nurse> Nurses { get; set; } = new List<Nurse>();
        public static List<Pharmacist> Pharmacists { get; set; } = new List<Pharmacist>();
        public static List<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();
        public static List<PatientVital> PatientVitals { get; set; } = new List<PatientVital>();
        public static List<LabTechnician> LabTechnicians { get; set; } = new List<LabTechnician>();
        public static List<Bill> Bills { get; set; } = new List<Bill>();
        public static List<Payment> Payments { get; set; } = new List<Payment>();
        public static List<InsuranceClaim> InsuranceClaims { get; set; } = new List<InsuranceClaim>();
        public static List<BillingStaff> BillingStaff { get; set; } = new List<BillingStaff>();

        private static bool isInitialized = false;

        public static User CurrentUser { get; set; }
        public static TimeSpan ClinicOpStart = new TimeSpan(0, 0, 0);
        public static TimeSpan ClinicOpEnd = new TimeSpan(23, 59, 59);
        public static int DefaultSlotDuration = 30;
        public static bool AllowDoubleBooking = false;
        public static DateTime LastBackupTime { get; set; } = DateTime.MinValue;

        private static IUnitOfWork CreateUnitOfWork() => new UnitOfWork(DatabaseFactory.CreateContext());

        static DataManager()
        {
            EnsureLoaded();
        }

        public static void EnsureLoaded()
        {
            if (isInitialized) return;
            isInitialized = true;

            try 
            {
                using (var context = DatabaseFactory.CreateContext())
                {
                    System.Diagnostics.Debug.WriteLine("DEBUG: Calling EnsureCreated...");
                    context.Database.EnsureCreated();
                    
                    System.Diagnostics.Debug.WriteLine("DEBUG: Calling RunMigrations...");
                    RunMigrations(context);
                }

                System.Diagnostics.Debug.WriteLine("DEBUG: Calling ReloadAllData...");
                ReloadAllData();
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("crashlog.txt", $"\nDEBUG TRACE: Error in EnsureLoaded. Step failed. Message: {ex.Message}");
                throw;
            }

            if (Doctors.Count < 5) SeedClinicalData();
            if (!Nurses.Any()) SeedNurses();
            if (!Pharmacists.Any()) SeedPharmacists();
            if (!PatientVitals.Any()) SeedPatientVitals();
            if (!Inventory.Any()) SeedInventory();
            if (!LabTests.Any()) SeedLabTests();
            if (!LabTechnicians.Any()) SeedLabTechnicians();
            if (!BillingStaff.Any()) SeedBillingStaff();
            if (!Bills.Any()) SeedBillingData();
        }

        private static void RunMigrations(HMSDbContext context)
        {
            try
            {
                // Fix: rename legacy 'Inventory' table to 'InventoryItems' if it exists under the old name.
                // This happened because EF used the class name before the [Table("InventoryItems")] attribute was added.
                context.Database.ExecuteSqlRaw(@"
                    IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Inventory')
                    AND NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'InventoryItems')
                    EXEC sp_rename 'Inventory', 'InventoryItems'");

                // Ensure DashboardActivities table (registered in DbContext but never in manual migrations)
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DashboardActivities')
                    CREATE TABLE DashboardActivities (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Title NVARCHAR(MAX),
                        Summary NVARCHAR(MAX),
                        Date DATETIME2 NOT NULL,
                        Icon NVARCHAR(MAX)
                    )");

                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'NurseId') ALTER TABLE Users ADD NurseId INT NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'PharmacistId') ALTER TABLE Users ADD PharmacistId INT NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'LabTechnicianId') ALTER TABLE Users ADD LabTechnicianId INT NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'BillingStaffId') ALTER TABLE Users ADD BillingStaffId INT NULL");

                // Ensure Doctors table columns (many were added recently)
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'SlotDurationMinutes') ALTER TABLE Doctors ADD SlotDurationMinutes INT NOT NULL DEFAULT 30");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'BufferMinutes') ALTER TABLE Doctors ADD BufferMinutes INT NOT NULL DEFAULT 5");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'SlotStepMinutes') ALTER TABLE Doctors ADD SlotStepMinutes INT NOT NULL DEFAULT 15");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'WorkingHoursStart') ALTER TABLE Doctors ADD WorkingHoursStart TIME NOT NULL DEFAULT '08:00:00'");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'WorkingHoursEnd') ALTER TABLE Doctors ADD WorkingHoursEnd TIME NOT NULL DEFAULT '16:00:00'");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'MaxPatientsPerSlot') ALTER TABLE Doctors ADD MaxPatientsPerSlot INT NOT NULL DEFAULT 1");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'AssignedShift') ALTER TABLE Doctors ADD AssignedShift NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'BreakTimeStart') ALTER TABLE Doctors ADD BreakTimeStart TIME NOT NULL DEFAULT '12:00:00'");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'BreakTimeEnd') ALTER TABLE Doctors ADD BreakTimeEnd TIME NOT NULL DEFAULT '13:00:00'");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'WorkingShifts') ALTER TABLE Doctors ADD WorkingShifts NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'WorkingDays') ALTER TABLE Doctors ADD WorkingDays NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'CalendarColor') ALTER TABLE Doctors ADD CalendarColor NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'PhotoPath') ALTER TABLE Doctors ADD PhotoPath NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'IsOnLeave') ALTER TABLE Doctors ADD IsOnLeave BIT NOT NULL DEFAULT 0");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'CurrentStatus') ALTER TABLE Doctors ADD CurrentStatus NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'BlockedTimes') ALTER TABLE Doctors ADD BlockedTimes NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'Gender') ALTER TABLE Doctors ADD Gender NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'DateOfBirth') ALTER TABLE Doctors ADD DateOfBirth DATETIME2 NOT NULL DEFAULT '1980-01-01'");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'Address') ALTER TABLE Doctors ADD Address NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'PhoneNumber') ALTER TABLE Doctors ADD PhoneNumber NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'Email') ALTER TABLE Doctors ADD Email NVARCHAR(MAX) NULL");

                // Ensure Patients table columns
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'GrandfatherName') ALTER TABLE Patients ADD GrandfatherName NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'Phone') ALTER TABLE Patients ADD Phone NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'Address') ALTER TABLE Patients ADD Address NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'Gender') ALTER TABLE Patients ADD Gender NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'IsActive') ALTER TABLE Patients ADD IsActive BIT NOT NULL DEFAULT 1");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'AssignedDoctorName') ALTER TABLE Patients ADD AssignedDoctorName NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'NationalIdOrPassport') ALTER TABLE Patients ADD NationalIdOrPassport NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'Email') ALTER TABLE Patients ADD Email NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'AllergiesOrChronicConditions') ALTER TABLE Patients ADD AllergiesOrChronicConditions NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'EmergencyContactName') ALTER TABLE Patients ADD EmergencyContactName NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'EmergencyContactPhone') ALTER TABLE Patients ADD EmergencyContactPhone NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'EmergencyContact') ALTER TABLE Patients ADD EmergencyContact NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'MedicalNotes') ALTER TABLE Patients ADD MedicalNotes NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'BloodGroup') ALTER TABLE Patients ADD BloodGroup NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'CurrentMedications') ALTER TABLE Patients ADD CurrentMedications NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'ChronicConditions') ALTER TABLE Patients ADD ChronicConditions NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'PreferredLanguage') ALTER TABLE Patients ADD PreferredLanguage NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'PhotoPath') ALTER TABLE Patients ADD PhotoPath NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'InsuranceNumber') ALTER TABLE Patients ADD InsuranceNumber NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Patients') AND name = 'PatientCode') ALTER TABLE Patients ADD PatientCode NVARCHAR(MAX) NULL");

                // Ensure Users table columns
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'Password') ALTER TABLE Users ADD Password NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'Role') ALTER TABLE Users ADD Role NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'Email') ALTER TABLE Users ADD Email NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'PatientId') ALTER TABLE Users ADD PatientId INT NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'DoctorId') ALTER TABLE Users ADD DoctorId INT NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'IsActive') ALTER TABLE Users ADD IsActive BIT NOT NULL DEFAULT 1");

                // Ensure Nurses table
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Nurses')
                    CREATE TABLE Nurses (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        FullName NVARCHAR(MAX),
                        Specialization NVARCHAR(MAX),
                        Phone NVARCHAR(MAX),
                        Email NVARCHAR(MAX),
                        Department NVARCHAR(MAX),
                        IsActive BIT NOT NULL DEFAULT 1
                    )");

                // Ensure Pharmacists table
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Pharmacists')
                    CREATE TABLE Pharmacists (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        FullName NVARCHAR(MAX),
                        LicenseNumber NVARCHAR(MAX),
                        Phone NVARCHAR(MAX),
                        Email NVARCHAR(MAX),
                        IsActive BIT NOT NULL DEFAULT 1
                    )");

                // Ensure InventoryItems table
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'InventoryItems')
                    CREATE TABLE InventoryItems (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(MAX),
                        Category NVARCHAR(MAX),
                        SKU NVARCHAR(MAX),
                        Description NVARCHAR(MAX),
                        Manufacturer NVARCHAR(MAX),
                        StockQuantity INT NOT NULL DEFAULT 0,
                        ReorderLevel INT NOT NULL DEFAULT 10,
                        PurchaseUnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
                        SellingUnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
                        UnitType NVARCHAR(MAX) NULL,
                        StorageLocation NVARCHAR(MAX),
                        ExpiryDate DATETIME2 NULL,
                        LastStockUpdate DATETIME2 NOT NULL DEFAULT GETDATE(),
                        SupplierName NVARCHAR(MAX),
                        IsActive BIT NOT NULL DEFAULT 1
                    )");

                // Ensure LabTechnicians table
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LabTechnicians')
                    CREATE TABLE LabTechnicians (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        FullName NVARCHAR(MAX),
                        Specialization NVARCHAR(MAX),
                        Phone NVARCHAR(MAX),
                        Email NVARCHAR(MAX),
                        IsActive BIT NOT NULL DEFAULT 1
                    )");

                // Ensure BillingStaff table
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BillingStaff')
                    CREATE TABLE BillingStaff (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        FullName NVARCHAR(MAX),
                        Position NVARCHAR(MAX),
                        Phone NVARCHAR(MAX),
                        Email NVARCHAR(MAX),
                        IsActive BIT NOT NULL DEFAULT 1
                    )");

                // Ensure Bills table
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Bills')
                    CREATE TABLE Bills (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        PatientId INT NOT NULL,
                        PatientName NVARCHAR(MAX),
                        BillDate DATETIME2 NOT NULL,
                        DueDate DATETIME2 NOT NULL,
                        TotalAmount DECIMAL(18,2) NOT NULL,
                        PaidAmount DECIMAL(18,2) NOT NULL,
                        Status NVARCHAR(MAX),
                        PaymentMethod NVARCHAR(MAX),
                        InsuranceProvider NVARCHAR(MAX),
                        InsuranceClaimNumber NVARCHAR(MAX),
                        Notes NVARCHAR(MAX)
                    )");

                // Ensure BillItems table
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BillItems')
                    CREATE TABLE BillItems (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        BillId INT NOT NULL,
                        Description NVARCHAR(MAX),
                        UnitPrice DECIMAL(18,2) NOT NULL,
                        Quantity INT NOT NULL,
                        TotalPrice DECIMAL(18,2) NOT NULL,
                        ItemType NVARCHAR(MAX)
                    )");

                // Ensure Payments table
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Payments')
                    CREATE TABLE Payments (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        BillId INT NOT NULL,
                        PatientId INT NOT NULL,
                        Amount DECIMAL(18,2) NOT NULL,
                        PaymentDate DATETIME2 NOT NULL,
                        PaymentMethod NVARCHAR(MAX),
                        TransactionReference NVARCHAR(MAX),
                        Status NVARCHAR(MAX),
                        HandledBy NVARCHAR(MAX)
                    )");

                // Ensure InsuranceClaims table
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'InsuranceClaims')
                    CREATE TABLE InsuranceClaims (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        BillId INT NOT NULL,
                        PatientId INT NOT NULL,
                        InsuranceProvider NVARCHAR(MAX),
                        PolicyNumber NVARCHAR(MAX),
                        ClaimNumber NVARCHAR(MAX),
                        ClaimAmount DECIMAL(18,2) NOT NULL,
                        Status NVARCHAR(MAX),
                        SubmittedDate DATETIME2 NOT NULL,
                        LastUpdated DATETIME2 NOT NULL
                    )");

                // Ensure PrescriptionItems table
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PrescriptionItems')
                    CREATE TABLE PrescriptionItems (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        PrescriptionId INT NOT NULL,
                        MedicineName NVARCHAR(MAX),
                        Dosage NVARCHAR(MAX),
                        Frequency NVARCHAR(MAX),
                        DurationDays INT NOT NULL DEFAULT 0,
                        Quantity INT NOT NULL DEFAULT 1,
                        Instructions NVARCHAR(MAX),
                        PharmacistInstructions NVARCHAR(MAX),
                        UnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
                        IsDispensed BIT NOT NULL DEFAULT 0
                    )");

                // Ensure PatientVitals table
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PatientVitals')
                    CREATE TABLE PatientVitals (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        PatientId INT NOT NULL,
                        PatientName NVARCHAR(MAX),
                        RoomNumber NVARCHAR(MAX),
                        BloodPressure NVARCHAR(MAX),
                        Temperature NVARCHAR(MAX),
                        Pulse NVARCHAR(MAX),
                        SPO2 NVARCHAR(MAX),
                        LastUpdated DATETIME2 NOT NULL DEFAULT GETDATE(),
                        UpdatedBy NVARCHAR(MAX)
                    )");

                // Ensure LabTests table
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LabTests')
                    CREATE TABLE LabTests (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        PatientId INT NOT NULL,
                        DoctorId INT NOT NULL,
                        AppointmentId INT NOT NULL,
                        TestName NVARCHAR(MAX),
                        TestCategory NVARCHAR(MAX),
                        RequestedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
                        CompletedDate DATETIME2 NULL,
                        Status NVARCHAR(MAX),
                        ResultSummary NVARCHAR(MAX),
                        ResultDetails NVARCHAR(MAX),
                        LabTechnicianName NVARCHAR(MAX),
                        ClinicalNotes NVARCHAR(MAX),
                        IsUrgent BIT NOT NULL DEFAULT 0,
                        IsCriticalValue BIT NOT NULL DEFAULT 0,
                        CriticalValueNote NVARCHAR(MAX),
                        PatientName NVARCHAR(MAX),
                        RejectionReason NVARCHAR(MAX),
                        ReferenceRange NVARCHAR(MAX),
                        NumericalValue NVARCHAR(MAX),
                        Unit NVARCHAR(MAX)
                    )");

                // Ensure new LabTests columns
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LabTests') AND name = 'IsUrgent') ALTER TABLE LabTests ADD IsUrgent BIT NOT NULL DEFAULT 0");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LabTests') AND name = 'IsCriticalValue') ALTER TABLE LabTests ADD IsCriticalValue BIT NOT NULL DEFAULT 0");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LabTests') AND name = 'CriticalValueNote') ALTER TABLE LabTests ADD CriticalValueNote NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LabTests') AND name = 'PatientName') ALTER TABLE LabTests ADD PatientName NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LabTests') AND name = 'RejectionReason') ALTER TABLE LabTests ADD RejectionReason NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LabTests') AND name = 'ReferenceRange') ALTER TABLE LabTests ADD ReferenceRange NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LabTests') AND name = 'NumericalValue') ALTER TABLE LabTests ADD NumericalValue NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LabTests') AND name = 'Unit') ALTER TABLE LabTests ADD Unit NVARCHAR(MAX) NULL");

                // Ensure Notifications columns
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Notifications') AND name = 'PatientId') ALTER TABLE Notifications ADD PatientId INT NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Notifications') AND name = 'DoctorId') ALTER TABLE Notifications ADD DoctorId INT NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Notifications') AND name = 'UserId') ALTER TABLE Notifications ADD UserId INT NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Notifications') AND name = 'Type') ALTER TABLE Notifications ADD Type NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Notifications') AND name = 'TargetRole') ALTER TABLE Notifications ADD TargetRole NVARCHAR(MAX) NULL");

                // Ensure Appointments columns
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'ConsultationFee') ALTER TABLE Appointments ADD ConsultationFee DECIMAL(18,2) NOT NULL DEFAULT 0");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'IsPaid') ALTER TABLE Appointments ADD IsPaid BIT NOT NULL DEFAULT 0");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'CompletionTime') ALTER TABLE Appointments ADD CompletionTime DATETIME2 NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'WaitTimeMinutes') ALTER TABLE Appointments ADD WaitTimeMinutes INT NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'NoShowReason') ALTER TABLE Appointments ADD NoShowReason NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'IsEmergency') ALTER TABLE Appointments ADD IsEmergency BIT NOT NULL DEFAULT 0");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'ClinicalNotes') ALTER TABLE Appointments ADD ClinicalNotes NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'AppointmentType') ALTER TABLE Appointments ADD AppointmentType NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'CheckInTime') ALTER TABLE Appointments ADD CheckInTime DATETIME2 NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'Diagnosis') ALTER TABLE Appointments ADD Diagnosis NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'Prescription') ALTER TABLE Appointments ADD Prescription NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'QueueNumber') ALTER TABLE Appointments ADD QueueNumber NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'Priority') ALTER TABLE Appointments ADD Priority NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'ConsultationNote') ALTER TABLE Appointments ADD ConsultationNote NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'Recommendation') ALTER TABLE Appointments ADD Recommendation NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'PatientRating') ALTER TABLE Appointments ADD PatientRating INT NOT NULL DEFAULT 0");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Appointments') AND name = 'PatientFeedback') ALTER TABLE Appointments ADD PatientFeedback NVARCHAR(MAX) NULL");

                // Ensure new Pharmacy/Prescription columns
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Prescriptions') AND name = 'DispensedDate') ALTER TABLE Prescriptions ADD DispensedDate DATETIME2 NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Prescriptions') AND name = 'DispensedBy') ALTER TABLE Prescriptions ADD DispensedBy NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Prescriptions') AND name = 'DispensingNotes') ALTER TABLE Prescriptions ADD DispensingNotes NVARCHAR(MAX) NULL");

                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PrescriptionItems') AND name = 'PharmacistInstructions') ALTER TABLE PrescriptionItems ADD PharmacistInstructions NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PrescriptionItems') AND name = 'UnitPrice') ALTER TABLE PrescriptionItems ADD UnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PrescriptionItems') AND name = 'IsDispensed') ALTER TABLE PrescriptionItems ADD IsDispensed BIT NOT NULL DEFAULT 0");

                // Fix PrescriptionItems name mismatch if exists
                context.Database.ExecuteSqlRaw("IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PrescriptionItems') AND name = 'MedicationName') AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PrescriptionItems') AND name = 'MedicineName') EXEC sp_rename 'PrescriptionItems.MedicationName', 'MedicineName', 'COLUMN'");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PrescriptionItems') AND name = 'MedicineName') ALTER TABLE PrescriptionItems ADD MedicineName NVARCHAR(MAX) NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PrescriptionItems') AND name = 'DurationDays') ALTER TABLE PrescriptionItems ADD DurationDays INT NOT NULL DEFAULT 0");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PrescriptionItems') AND name = 'Quantity') ALTER TABLE PrescriptionItems ADD Quantity INT NOT NULL DEFAULT 1");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Schema migration failed: {ex.Message}");
                throw new Exception("CRITICAL: Schema migration failed. The database might be corrupted or inaccessible.", ex);
            }
        }

        public static void ReloadUsers()
        {
            using (var uow = CreateUnitOfWork())
            {
                Users = uow.Users.GetAll().ToList();
            }
        }

        public static void ReloadAllData()
        {
            using (var uow = CreateUnitOfWork())
            {
                Users = uow.Users.GetAll().ToList();
                Doctors = uow.Doctors.GetAll().ToList();
                Patients = uow.Patients.GetAll().ToList();
                Appointments = uow.Appointments.GetAll().ToList();
                Feedbacks = uow.Feedbacks.GetAll().ToList();
                MedicalRecords = uow.MedicalRecords.GetAll().ToList();
                
                // Load Prescriptions with Items
                Prescriptions = uow.Prescriptions.GetAll().ToList();
                var prescriptionItems = uow.PrescriptionItems.GetAll().ToList();
                foreach (var p in Prescriptions)
                {
                    p.Items = prescriptionItems.Where(i => i.PrescriptionId == p.Id).ToList();
                }

                LabTests = uow.LabTests.GetAll().ToList();
                Notifications = uow.Notifications.GetAll().ToList();
                AuditLogs = uow.AuditLogs.GetAll().ToList();
                Nurses = uow.Nurses.GetAll().ToList();
                Pharmacists = uow.Pharmacists.GetAll().ToList();
                Inventory = uow.InventoryItems.GetAll().ToList();
                LabTechnicians = uow.LabTechnicians.GetAll().ToList();
                Payments = uow.Payments.GetAll().ToList();
                InsuranceClaims = uow.InsuranceClaims.GetAll().ToList();
                BillingStaff = uow.BillingStaff.GetAll().ToList();
                
                // Load Bills with Items
                Bills = uow.Bills.GetAll().ToList();
                var billItems = uow.BillItems.GetAll().ToList();
                foreach (var b in Bills)
                {
                    b.Items = billItems.Where(i => i.BillId == b.Id).ToList();
                }

                PatientVitals = uow.PatientVitals.GetAll().ToList();
            }
        }

        public static void ReloadInventory()
        {
            using (var uow = CreateUnitOfWork())
            {
                Inventory = uow.InventoryItems.GetAll().ToList();
            }
        }

        public static void AddInventoryItem(InventoryItem item)
        {
            using (var uow = CreateUnitOfWork())
            {
                uow.InventoryItems.Add(item);
                uow.Complete();
            }
            // Reload so the new item appears with its DB-generated Id
            ReloadInventory();
        }

        public static void SeedClinicalData()
        {
            Doctors.Clear();
            var clinicalStaff = new List<Doctor> {
                new Doctor { FullName = "Dr. Elena Rodriguez", Specialization = "Cardiology", Department = "Cardiology", IsActive = true, WorkingDays = "Mon,Tue,Wed,Thu,Fri", WorkingHoursStart = new TimeSpan(8,0,0), WorkingHoursEnd = new TimeSpan(16,0,0) },
                new Doctor { FullName = "Dr. Marcus Thorne", Specialization = "Orthopedics", Department = "Orthopedics", IsActive = true, WorkingDays = "Mon,Tue,Wed,Fri", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(17,0,0) },
                new Doctor { FullName = "Dr. Sarah Jenkins", Specialization = "Pediatrics", Department = "Pediatrics", IsActive = true, WorkingDays = "Mon,Wed,Fri", WorkingHoursStart = new TimeSpan(10,0,0), WorkingHoursEnd = new TimeSpan(16,0,0) },
                new Doctor { FullName = "Dr. Robert Chen", Specialization = "Neurology", Department = "Neurology", IsActive = true, WorkingDays = "Tue,Thu,Sat", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(17,0,0) },
                new Doctor { FullName = "Dr. Aisha Patel", Specialization = "Dermatology", Department = "Dermatology", IsActive = true, WorkingDays = "Mon,Tue,Thu,Fri", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(17,30,0) }
            };
            foreach(var d in clinicalStaff) if(!Doctors.Any(x => x.FullName == d.FullName)) Doctors.Add(d);
            SaveDoctors();
        }

        public static void SeedNurses()
        {
            if (Nurses.Any()) return;
            Nurses.Add(new Nurse { FullName = "Nurse Sarah Jenkins", Specialization = "Triage", Department = "General Ward", IsActive = true });
            SaveNurses();
        }



        public static void SeedLabTests()
        {
            if (LabTests.Any()) return;
            LabTests.Add(new LabTest { PatientId = 1, TestName = "CBC", Status = "Completed", ResultSummary = "Normal" });
            SaveLabTests();
        }

        public static void SeedPharmacists()
        {
            if (Pharmacists.Any()) return;
            Pharmacists.Add(new Pharmacist { FullName = "Pharm. John Doe", LicenseNumber = "PHARM-123", IsActive = true });
            SavePharmacists();
        }

        public static void SeedLabTechnicians()
        {
            if (LabTechnicians.Any()) return;
            LabTechnicians.Add(new LabTechnician { FullName = "Tech. Alex Smith", EmployeeCode = "LAB-001", IsActive = true });
            SaveLabTechnicians();
        }

        public static void SeedBillingStaff()
        {
            if (BillingStaff.Any()) return;
            BillingStaff.Add(new BillingStaff { FullName = "Accountant Sarah Lee", EmployeeCode = "ACC-001", Position = "Senior Accountant", IsActive = true });
            SaveBillingStaff();
        }

        public static void SeedBillingData()
        {
            if (Bills.Any()) return;
            var bill = new Bill { 
                PatientId = 1, 
                TotalAmount = 250.00m, 
                PaidAmount = 250.00m, 
                Status = "Paid", 
                PaymentMethod = "Cash",
                Items = new List<BillItem> {
                    new BillItem { Description = "Consultation Fee", UnitPrice = 50.00m, TotalPrice = 50.00m, ItemType = "Consultation" },
                    new BillItem { Description = "Laboratory - CBC", UnitPrice = 100.00m, TotalPrice = 100.00m, ItemType = "Lab" },
                    new BillItem { Description = "Pharmacy - Antibiotics", UnitPrice = 100.00m, TotalPrice = 100.00m, ItemType = "Pharmacy" }
                }
            };
            Bills.Add(bill);
            Payments.Add(new Payment { BillId = 1, PatientId = 1, Amount = 250.00m, PaymentMethod = "Cash", HandledBy = "Sarah Lee" });
            SaveBills();
            SavePayments();
        }

        public static User AuthenticateUser(string email, string password)
        {
            var user = Users.FirstOrDefault(u => u.Email != null && u.Email.ToLower() == email.Trim().ToLower());
            if (user != null && PasswordHasher.VerifyPassword(password, user.Password))
            {
                CurrentUser = user;
                return user;
            }
            return null;
        }

        public static void Logout() => CurrentUser = null;

        public static Patient GetCurrentPatient()
        {
            if (CurrentUser == null || CurrentSession.Instance.LoggedInPatient == null) return null;
            return Patients.FirstOrDefault(p => p.Id == CurrentSession.Instance.LoggedInPatient.Id);
        }

        public static bool AddAppointment(Appointment appointment)
        {
            if (appointment.AppointmentDate < DateTime.Now.AddMinutes(-5)) return false; 
            Appointments.Add(appointment);
            SaveAppointments();
            return true;
        }

        public static bool IsSlotAvailable(int doctorId, DateTime slotTime)
        {
            var doctor = Doctors.FirstOrDefault(d => d.Id == doctorId);
            if (doctor == null || !doctor.IsActive || doctor.IsOnLeave) return false;

            // 1. Check Working Days
            string dayShort = slotTime.ToString("ddd");
            if (doctor.WorkingDays != null && !doctor.WorkingDays.Contains(dayShort)) return false;

            // 2. Check Working Hours
            var timeOfDay = slotTime.TimeOfDay;
            if (timeOfDay < doctor.WorkingHoursStart || timeOfDay >= doctor.WorkingHoursEnd) return false;

            // 3. Check Break Times
            if (timeOfDay >= doctor.BreakTimeStart && timeOfDay < doctor.BreakTimeEnd) return false;

            // 4. Check Existing Appointments against MaxPatientsPerSlot
            int currentlyBooked = Appointments.Count(a => a.DoctorId == doctorId && 
                                                      a.AppointmentDate == slotTime && 
                                                      a.Status != "Cancelled" && 
                                                      a.Status != "No Show");
            
            return currentlyBooked < doctor.MaxPatientsPerSlot;
        }

        public static List<DateTime> GetAvailableTimeSlots(int doctorId, DateTime date)
        {
            var slots = new List<DateTime>();
            var doc = Doctors.FirstOrDefault(d => d.Id == doctorId);
            if (doc == null || !doc.IsActive || doc.IsOnLeave) return slots;

            DateTime current = date.Date.Add(doc.WorkingHoursStart);
            DateTime end = date.Date.Add(doc.WorkingHoursEnd);
            int interval = doc.SlotDurationMinutes > 0 ? doc.SlotDurationMinutes : 30;

            while (current < end) {
                if (IsSlotAvailable(doctorId, current)) 
                {
                    slots.Add(current);
                }
                current = current.AddMinutes(interval);
            }
            return slots;
        }

        public static bool IsPhoneUnique(string phone) => !Patients.Any(p => p.Phone == phone);

        public static void RegisterPatient(Patient patient)
        {
            try 
            {
                if (!Patients.Contains(patient)) Patients.Add(patient);
                if (string.IsNullOrEmpty(patient.PatientCode)) patient.PatientCode = "PAT-PENDING"; 
                SavePatients(); 
                if (patient.Id != 0 && !Users.Any(u => u.PatientId == patient.Id))
                {
                    var newUser = new User { Email = string.IsNullOrWhiteSpace(patient.Email) ? $"{patient.Phone}@patient.local" : patient.Email, Password = PasswordHasher.HashPassword("password123"), Role = "Patient", PatientId = patient.Id, IsActive = true };
                    Users.Add(newUser);
                    SaveUsers();
                }
                if (patient.PatientCode == "PAT-PENDING" && patient.Id != 0) {
                    patient.PatientCode = $"PAT-{patient.Id:D5}";
                    SavePatients();
                }
            }
            catch (Exception ex)
            {
                LogAudit("System", $"Patient registration failed: {ex.Message}", "Error");
                throw;
            }
        }

        /// <summary>
        /// Registers a new patient AND creates a linked User login account atomically.
        /// The admin provides explicit email + password for the patient's login credentials.
        /// </summary>
        public static void RegisterPatientWithCredentials(Patient patient, string loginEmail, string password)
        {
            try 
            {
                if (!Patients.Contains(patient)) Patients.Add(patient);
                if (string.IsNullOrEmpty(patient.PatientCode)) patient.PatientCode = "PAT-PENDING"; 
                SavePatients(); 

                // Create the linked User account with the admin-provided credentials
                if (patient.Id != 0 && !Users.Any(u => u.PatientId == patient.Id))
                {
                    var newUser = new User 
                    { 
                        Email = loginEmail,
                        Password = PasswordHasher.HashPassword(password),
                        Role = "Patient", 
                        PatientId = patient.Id, 
                        IsActive = true 
                    };
                    Users.Add(newUser);
                    SaveUsers();
                }

                // Generate patient code
                if (patient.PatientCode == "PAT-PENDING" && patient.Id != 0) 
                {
                    patient.PatientCode = $"PAT-{patient.Id:D5}";
                    SavePatients();
                }
            }
            catch (Exception ex)
            {
                LogAudit("System", $"Patient registration failed: {ex.Message}", "Error");
                throw;
            }
        }

        /// <summary>
        /// Registers a new doctor AND creates a linked User login account atomically.
        /// Accepts an optional explicit password; defaults to "Doctor@123" if not provided.
        /// </summary>
        public static void RegisterDoctor(Doctor doctor, string password = null)
        {
            try 
            {
                if (!Doctors.Contains(doctor))
                {
                    Doctors.Add(doctor);
                }

                SaveDoctors();

                if (doctor.Id != 0 && !Users.Any(u => u.Role == "Doctor" && u.DoctorId == doctor.Id))
                {
                    var newUser = new User 
                    { 
                        Email = string.IsNullOrWhiteSpace(doctor.Email) ? $"doctor{doctor.Id}@hospital.com" : doctor.Email,
                        Password = PasswordHasher.HashPassword(password ?? "Doctor@123"),
                        Role = "Doctor", 
                        DoctorId = doctor.Id,
                        IsActive = true 
                    };
                    Users.Add(newUser);
                    SaveUsers();
                }
            }
            catch (Exception ex)
            {
                LogAudit("System", $"Doctor registration failed: {ex.Message}", "Error");
                throw;
            }
        }

        public static void RegisterNurse(Nurse nurse, string password = null)
        {
            try 
            {
                if (!Nurses.Contains(nurse)) Nurses.Add(nurse);
                SaveNurses();

                if (nurse.Id != 0 && !Users.Any(u => u.Role == "Nurse" && u.NurseId == nurse.Id))
                {
                    var newUser = new User 
                    { 
                        Email = string.IsNullOrWhiteSpace(nurse.Email) ? $"nurse{nurse.Id}@hospital.com" : nurse.Email,
                        Password = PasswordHasher.HashPassword(password ?? "Nurse@123"),
                        Role = "Nurse", 
                        NurseId = nurse.Id,
                        IsActive = true 
                    };
                    Users.Add(newUser);
                    SaveUsers();
                }
            }
            catch (Exception ex)
            {
                LogAudit("System", $"Nurse registration failed: {ex.Message}", "Error");
                throw;
            }
        }

        public static void RegisterPharmacist(Pharmacist pharmacist, string password = null)
        {
            try 
            {
                if (!Pharmacists.Contains(pharmacist)) Pharmacists.Add(pharmacist);
                SavePharmacists();

                if (pharmacist.Id != 0 && !Users.Any(u => u.Role == "Pharmacist" && u.PharmacistId == pharmacist.Id))
                {
                    var newUser = new User 
                    { 
                        Email = string.IsNullOrWhiteSpace(pharmacist.Email) ? $"pharm{pharmacist.Id}@hospital.com" : pharmacist.Email,
                        Password = PasswordHasher.HashPassword(password ?? "Pharm@123"),
                        Role = "Pharmacist", 
                        PharmacistId = pharmacist.Id,
                        IsActive = true 
                    };
                    Users.Add(newUser);
                    SaveUsers();
                }
            }
            catch (Exception ex)
            {
                LogAudit("System", $"Pharmacist registration failed: {ex.Message}", "Error");
                throw;
            }
        }

        public static void RegisterLabTechnician(LabTechnician tech, string password = null)
        {
            try 
            {
                if (!LabTechnicians.Contains(tech)) LabTechnicians.Add(tech);
                SaveLabTechnicians();

                if (tech.Id != 0 && !Users.Any(u => u.Role == "LabTechnician" && u.LabTechnicianId == tech.Id))
                {
                    var newUser = new User 
                    { 
                        Email = string.IsNullOrWhiteSpace(tech.Email) ? $"tech{tech.Id}@hospital.com" : tech.Email,
                        Password = PasswordHasher.HashPassword(password ?? "Lab@123"),
                        Role = "LabTechnician", 
                        LabTechnicianId = tech.Id,
                        IsActive = true 
                    };
                    Users.Add(newUser);
                    SaveUsers();
                }
            }
            catch (Exception ex)
            {
                LogAudit("System", $"Lab Technician registration failed: {ex.Message}", "Error");
                throw;
            }
        }

        public static void RegisterBillingStaff(BillingStaff staff, string password = null)
        {
            try 
            {
                if (!BillingStaff.Contains(staff)) BillingStaff.Add(staff);
                SaveBillingStaff();

                if (staff.Id != 0 && !Users.Any(u => u.Role == "Receptionist" && u.BillingStaffId == staff.Id))
                {
                    var newUser = new User 
                    { 
                        Email = string.IsNullOrWhiteSpace(staff.Email) ? $"staff{staff.Id}@hospital.com" : staff.Email,
                        Password = PasswordHasher.HashPassword(password ?? "Staff@123"),
                        Role = "Receptionist", 
                        BillingStaffId = staff.Id,
                        IsActive = true 
                    };
                    Users.Add(newUser);
                    SaveUsers();
                }
            }
            catch (Exception ex)
            {
                LogAudit("System", $"Billing Staff registration failed: {ex.Message}", "Error");
                throw;
            }
        }

        public static void AddFeedback(Feedback f) { Feedbacks.Add(f); SaveFeedbacks(); }
        public static List<Appointment> GetPatientAppointments(int patientId) => Appointments.Where(a => a.PatientId == patientId).ToList();
        public static List<MedicalRecord> GetPatientMedicalRecords(int patientId) => MedicalRecords.Where(r => r.PatientId == patientId).ToList();
        public static List<Notification> GetPatientNotifications(int patientId) => Notifications.Where(n => n.PatientId == patientId).ToList();
        public static List<Feedback> GetDoctorFeedback(int doctorId) => Feedbacks.Where(f => f.DoctorId == doctorId).ToList();
        public static List<Prescription> GetPatientPrescriptions(int patientId) => Prescriptions.Where(p => p.PatientId == patientId).ToList();
        public static List<LabTest> GetPatientLabTests(int patientId) => LabTests.Where(t => t.PatientId == patientId).ToList();
        public static List<Bill> GetPatientBills(int patientId) => Bills.Where(b => b.PatientId == patientId).ToList();
        public static void UpdateBill(Bill bill) { SaveBills(); } 
        public static Doctor GetDoctorById(int doctorId) => Doctors.FirstOrDefault(d => d.Id == doctorId);
        public static List<PrescriptionItem> GetItemsForPrescription(int prescriptionId)
        {
            var p = Prescriptions.FirstOrDefault(x => x.Id == prescriptionId);
            return p?.Items ?? new List<PrescriptionItem>();
        }

        public static Appointment GetLastPatientVisit(int patientId)
        {
            return Appointments.Where(a => a.PatientId == patientId && a.AppointmentDate < DateTime.Now && a.Status == "Completed").OrderByDescending(a => a.AppointmentDate).FirstOrDefault();
        }

        public static Appointment GetNextPatientAppointment(int patientId)
        {
            return Appointments.Where(a => a.PatientId == patientId && a.AppointmentDate >= DateTime.Now && a.Status != "Cancelled").OrderBy(a => a.AppointmentDate).FirstOrDefault();
        }

        public static bool HasRated(int appointmentId) => Feedbacks.Any(f => f.AppointmentId == appointmentId);
        public static bool HasBeenRated(int appointmentId) => Feedbacks.Any(f => f.AppointmentId == appointmentId);
        public static double GetDoctorAverageRating(int doctorId) { var f = GetDoctorFeedback(doctorId); return f.Any() ? f.Average(x => x.Rating) : 0; }
        public static Patient FindPatientByPhone(string phone) => Patients.FirstOrDefault(p => p.Phone == phone);
        public static void MarkNotificationAsRead(int notificationId) { var n = Notifications.FirstOrDefault(x => x.Id == notificationId); if (n != null) n.IsRead = true; }

        public static void LogAudit(string userEmail, string action, string module = "Security") {
            AuditLogs.Add(new AuditLogEntry { Timestamp = DateTime.Now, UserEmail = userEmail ?? "System", Action = action, Module = module });
        }

        public static void SaveAllData() { 
            SaveUsers(); SaveDoctors(); SavePatients(); SaveAppointments(); SaveFeedbacks();
            SaveMedicalRecords(); SavePrescriptions(); SaveLabTests(); SaveNotifications();
            SaveNurses(); SavePharmacists(); SaveInventory(); SaveLabTechnicians();
            SaveBills(); SavePayments(); SaveInsuranceClaims(); SaveBillingStaff();
            SavePatientVitals();
        }

        public static void SaveUsers() { 
            using (var uow = CreateUnitOfWork())
            {
                // Deduplicate in-memory list before saving
                var groups = Users
                    .Where(u => !string.IsNullOrEmpty(u.Email))
                    .GroupBy(u => u.Email.ToLower().Trim())
                    .ToList();

                var uniqueUsers = new List<User>();
                var toRemoveFromDb = new List<User>();

                foreach (var group in groups)
                {
                    var sorted = group.OrderByDescending(u => u.Id).ToList();
                    uniqueUsers.Add(sorted.First());
                    if (sorted.Count > 1)
                    {
                        toRemoveFromDb.AddRange(sorted.Skip(1).Where(u => u.Id != 0));
                    }
                }

                // Add any users without emails
                uniqueUsers.AddRange(Users.Where(u => string.IsNullOrEmpty(u.Email)));

                Users = uniqueUsers;

                foreach (var u in toRemoveFromDb) uow.Users.Remove(u);
                
                foreach(var u in Users) {
                    if(u.Id == 0) uow.Users.Add(u);
                    else uow.Users.Update(u);
                }
                uow.Complete(); 
            }
        }

        public static void SaveDoctors() { 
            using (var uow = CreateUnitOfWork())
            {
                foreach(var d in Doctors) {
                    if(d.Id == 0) uow.Doctors.Add(d);
                    else uow.Doctors.Update(d);
                }
                uow.Complete();
            }
        }

        public static void SavePatients() { 
            using (var uow = CreateUnitOfWork())
            {
                foreach(var p in Patients.ToList()) {
                    if(p.Id == 0) uow.Patients.Add(p);
                    else uow.Patients.Update(p);
                }
                uow.Complete(); 
            }
        }

        public static void SaveAppointments() { 
            using (var uow = CreateUnitOfWork())
            {
                foreach(var a in Appointments) {
                    if(a.Id == 0) uow.Appointments.Add(a);
                    else uow.Appointments.Update(a);
                }
                uow.Complete(); 
            }
        }

        public static void SaveFeedbacks() { 
            using (var uow = CreateUnitOfWork())
            {
                foreach(var f in Feedbacks) {
                    if(f.Id == 0) uow.Feedbacks.Add(f);
                    else uow.Feedbacks.Update(f);
                }
                uow.Complete(); 
            }
        }

        public static void SaveMedicalRecords() { 
            using (var uow = CreateUnitOfWork())
            {
                foreach(var m in MedicalRecords) {
                    if(m.Id == 0) uow.MedicalRecords.Add(m);
                    else uow.MedicalRecords.Update(m);
                }
                uow.Complete(); 
            }
        }

        public static void SaveDepartments() { /* Not implemented in DB yet */ }

        public static void SavePrescriptions() { 
            using (var uow = CreateUnitOfWork())
            {
                foreach(var p in Prescriptions) {
                    if(p.Id == 0) uow.Prescriptions.Add(p);
                    else uow.Prescriptions.Update(p);
                }
                uow.Complete(); 
            }
        }

        public static void SaveLabTests() { 
            using (var uow = CreateUnitOfWork())
            {
                foreach(var l in LabTests) {
                    if(l.Id == 0) uow.LabTests.Add(l);
                    else uow.LabTests.Update(l);
                }
                uow.Complete(); 
            }
        }

        public static void ReloadLabTests()
        {
            using (var uow = CreateUnitOfWork())
            {
                LabTests = uow.LabTests.GetAll().ToList();
            }
        }
 
        public static void SaveNurses() { 
            using (var uow = CreateUnitOfWork())
            {
                foreach(var n in Nurses) {
                    if(n.Id == 0) uow.Nurses.Add(n);
                    else uow.Nurses.Update(n);
                }
                uow.Complete(); 
            }
        }
 
        public static void SavePharmacists() { 
            using (var uow = CreateUnitOfWork())
            {
                foreach(var p in Pharmacists) {
                    if(p.Id == 0) uow.Pharmacists.Add(p);
                    else uow.Pharmacists.Update(p);
                }
                uow.Complete(); 
            }
        }
 
        public static void SaveInventory() { 
            using (var uow = CreateUnitOfWork())
            {
                foreach(var i in Inventory) {
                    if(i.Id == 0) uow.InventoryItems.Add(i);
                    else uow.InventoryItems.Update(i);
                }
                uow.Complete(); 
            }
        }
 
        public static void SavePatientVitals() { 
            using (var uow = CreateUnitOfWork())
            {
                foreach(var v in PatientVitals) {
                    if(v.Id == 0) uow.PatientVitals.Add(v);
                    else uow.PatientVitals.Update(v);
                }
                uow.Complete(); 
            }
        }

        public static void SaveNotifications() { 
            using (var uow = CreateUnitOfWork())
            {
                foreach(var n in Notifications) {
                    if(n.Id == 0) uow.Notifications.Add(n);
                    else uow.Notifications.Update(n);
                }
                uow.Complete(); 
            }
        }

        public static void SeedPatientVitals()
        {
            if (PatientVitals.Any()) return;
            var rand = new Random(42);
            foreach (var p in Patients.Take(15))
            {
                PatientVitals.Add(new PatientVital
                {
                    PatientId = p.Id,
                    PatientName = p.FullName,
                    RoomNumber = "Room " + (200 + p.Id % 50),
                    BloodPressure = $"{rand.Next(110, 145)}/{rand.Next(65, 90)}",
                    Temperature = $"{(36.0 + rand.NextDouble() * 2.5):F1}°C",
                    Pulse = $"{rand.Next(60, 100)} bpm",
                    SPO2 = $"{rand.Next(94, 100)}%",
                    LastUpdated = DateTime.Now.AddMinutes(-rand.Next(10, 300)),
                    UpdatedBy = "System"
                });
            }
            SavePatientVitals();
        }
 
        public static void SeedInventory()
        {
            if (Inventory.Any()) return;

            var medicines = new List<InventoryItem>
            {
                new InventoryItem { Name = "Amoxicillin 500mg", SKU = "MED-001", Category = "Medicine", UnitType = "Capsules", StockQuantity = 500, SellingUnitPrice = 2.50m, ReorderLevel = 50, ExpiryDate = DateTime.Now.AddYears(2), LastStockUpdate = DateTime.Now, IsActive = true },
                new InventoryItem { Name = "Paracetamol 500mg", SKU = "MED-002", Category = "Medicine", UnitType = "Tablets", StockQuantity = 1000, SellingUnitPrice = 0.80m, ReorderLevel = 100, ExpiryDate = DateTime.Now.AddYears(2), LastStockUpdate = DateTime.Now, IsActive = true },
                new InventoryItem { Name = "Ibuprofen 400mg", SKU = "MED-003", Category = "Medicine", UnitType = "Tablets", StockQuantity = 600, SellingUnitPrice = 1.20m, ReorderLevel = 60, ExpiryDate = DateTime.Now.AddYears(2), LastStockUpdate = DateTime.Now, IsActive = true },
                new InventoryItem { Name = "Metformin 850mg", SKU = "MED-004", Category = "Medicine", UnitType = "Tablets", StockQuantity = 400, SellingUnitPrice = 3.00m, ReorderLevel = 40, ExpiryDate = DateTime.Now.AddMonths(18), LastStockUpdate = DateTime.Now, IsActive = true },
                new InventoryItem { Name = "Atorvastatin 20mg", SKU = "MED-005", Category = "Medicine", UnitType = "Tablets", StockQuantity = 300, SellingUnitPrice = 5.50m, ReorderLevel = 30, ExpiryDate = DateTime.Now.AddYears(2), LastStockUpdate = DateTime.Now, IsActive = true },
                new InventoryItem { Name = "Ciprofloxacin 250mg", SKU = "MED-006", Category = "Medicine", UnitType = "Tablets", StockQuantity = 200, SellingUnitPrice = 4.00m, ReorderLevel = 25, ExpiryDate = DateTime.Now.AddYears(1), LastStockUpdate = DateTime.Now, IsActive = true },
                new InventoryItem { Name = "Omeprazole 20mg", SKU = "MED-007", Category = "Medicine", UnitType = "Capsules", StockQuantity = 350, SellingUnitPrice = 2.80m, ReorderLevel = 35, ExpiryDate = DateTime.Now.AddYears(2), LastStockUpdate = DateTime.Now, IsActive = true },
                new InventoryItem { Name = "Vitamin D3 1000IU", SKU = "MED-008", Category = "Medicine", UnitType = "Tablets", StockQuantity = 800, SellingUnitPrice = 1.50m, ReorderLevel = 80, ExpiryDate = DateTime.Now.AddYears(3), LastStockUpdate = DateTime.Now, IsActive = true },
                new InventoryItem { Name = "Amlodipine 5mg", SKU = "MED-009", Category = "Medicine", UnitType = "Tablets", StockQuantity = 250, SellingUnitPrice = 3.50m, ReorderLevel = 25, ExpiryDate = DateTime.Now.AddYears(2), LastStockUpdate = DateTime.Now, IsActive = true },
                new InventoryItem { Name = "Normal Saline 0.9% 500ml", SKU = "SUP-001", Category = "Medical Supply", UnitType = "Bottles", StockQuantity = 120, SellingUnitPrice = 15.00m, ReorderLevel = 20, ExpiryDate = DateTime.Now.AddYears(2), LastStockUpdate = DateTime.Now, IsActive = true },
                new InventoryItem { Name = "Surgical Gloves (Box)", SKU = "SUP-002", Category = "Medical Supply", UnitType = "Bottles", StockQuantity = 50, SellingUnitPrice = 45.00m, ReorderLevel = 10, LastStockUpdate = DateTime.Now, IsActive = true },
                new InventoryItem { Name = "Insulin Regular 10ml", SKU = "MED-010", Category = "Medicine", UnitType = "Vials", StockQuantity = 8, SellingUnitPrice = 120.00m, ReorderLevel = 10, ExpiryDate = DateTime.Now.AddMonths(6), LastStockUpdate = DateTime.Now, IsActive = true },
            };

            foreach (var item in medicines)
            {
                AddInventoryItem(item);
            }
        }
        public static void SaveLabTechnicians() { using (var uow = CreateUnitOfWork()) { foreach(var l in LabTechnicians) { if(l.Id == 0) uow.LabTechnicians.Add(l); else uow.LabTechnicians.Update(l); } uow.Complete(); } }

        public static void SaveBills() { using (var uow = CreateUnitOfWork()) { foreach(var b in Bills) { if(b.Id == 0) uow.Bills.Add(b); else uow.Bills.Update(b); } uow.Complete(); } }
        public static void SavePayments() { using (var uow = CreateUnitOfWork()) { foreach(var p in Payments) { if(p.Id == 0) uow.Payments.Add(p); else uow.Payments.Update(p); } uow.Complete(); } }
        public static void SaveInsuranceClaims() { using (var uow = CreateUnitOfWork()) { foreach(var c in InsuranceClaims) { if(c.Id == 0) uow.InsuranceClaims.Add(c); else uow.InsuranceClaims.Update(c); } uow.Complete(); } }
        public static void SaveBillingStaff() { using (var uow = CreateUnitOfWork()) { foreach(var s in BillingStaff) { if(s.Id == 0) uow.BillingStaff.Add(s); else uow.BillingStaff.Update(s); } uow.Complete(); } }


        public static void BackupData() { LastBackupTime = DateTime.Now; }
    }
}
