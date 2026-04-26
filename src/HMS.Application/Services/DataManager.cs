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

            using (var context = DatabaseFactory.CreateContext())
            {
                context.Database.EnsureCreated();
                RunMigrations(context);
            }

            LoadFromDb();

            if (Doctors.Count < 5) SeedClinicalData();
            if (!Nurses.Any()) SeedNurses();
            if (!Pharmacists.Any()) SeedPharmacists();
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
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'NurseId') ALTER TABLE Users ADD NurseId INT NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'PharmacistId') ALTER TABLE Users ADD PharmacistId INT NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'LabTechnicianId') ALTER TABLE Users ADD LabTechnicianId INT NULL");
                context.Database.ExecuteSqlRaw("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'BillingStaffId') ALTER TABLE Users ADD BillingStaffId INT NULL");

                context.Database.ExecuteSqlRaw(@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('Nurses') AND type = 'U') CREATE TABLE Nurses (Id INT PRIMARY KEY IDENTITY(1,1), FullName NVARCHAR(MAX), Specialization NVARCHAR(MAX), Phone NVARCHAR(MAX), Email NVARCHAR(MAX), Department NVARCHAR(MAX), IsActive BIT NOT NULL DEFAULT 1)");
                context.Database.ExecuteSqlRaw(@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('Pharmacists') AND type = 'U') CREATE TABLE Pharmacists (Id INT PRIMARY KEY IDENTITY(1,1), FullName NVARCHAR(MAX), LicenseNumber NVARCHAR(MAX), Phone NVARCHAR(MAX), Email NVARCHAR(MAX), IsActive BIT NOT NULL DEFAULT 1)");
                context.Database.ExecuteSqlRaw(@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('Inventory') AND type = 'U') CREATE TABLE Inventory (Id INT PRIMARY KEY IDENTITY(1,1), Name NVARCHAR(MAX), Category NVARCHAR(MAX), SKU NVARCHAR(MAX), Description NVARCHAR(MAX), Manufacturer NVARCHAR(MAX), StockQuantity INT NOT NULL DEFAULT 0, ReorderLevel INT NOT NULL DEFAULT 10, PurchaseUnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0, SellingUnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0, StorageLocation NVARCHAR(MAX), ExpiryDate DATETIME2 NULL, LastStockUpdate DATETIME2 NOT NULL DEFAULT GETDATE(), SupplierName NVARCHAR(MAX), IsActive BIT NOT NULL DEFAULT 1)");
                context.Database.ExecuteSqlRaw(@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('LabTechnicians') AND type = 'U') CREATE TABLE LabTechnicians (Id INT PRIMARY KEY IDENTITY(1,1), FullName NVARCHAR(MAX), EmployeeCode NVARCHAR(MAX), Specialization NVARCHAR(MAX), Phone NVARCHAR(MAX), Email NVARCHAR(MAX), Department NVARCHAR(MAX) DEFAULT 'Laboratory', IsActive BIT NOT NULL DEFAULT 1)");
                context.Database.ExecuteSqlRaw(@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('BillingStaff') AND type = 'U') CREATE TABLE BillingStaff (Id INT PRIMARY KEY IDENTITY(1,1), FullName NVARCHAR(MAX), EmployeeCode NVARCHAR(MAX), Position NVARCHAR(MAX), Phone NVARCHAR(MAX), Email NVARCHAR(MAX), Department NVARCHAR(MAX) DEFAULT 'Finance/Billing', IsActive BIT NOT NULL DEFAULT 1)");
                context.Database.ExecuteSqlRaw(@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('Payments') AND type = 'U') CREATE TABLE Payments (Id INT PRIMARY KEY IDENTITY(1,1), BillId INT NOT NULL, PatientId INT NOT NULL, PaymentDate DATETIME2 NOT NULL, Amount DECIMAL(18,2) NOT NULL, PaymentMethod NVARCHAR(MAX), TransactionId NVARCHAR(MAX), Status NVARCHAR(MAX), Notes NVARCHAR(MAX), HandledBy NVARCHAR(MAX))");
                context.Database.ExecuteSqlRaw(@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('InsuranceClaims') AND type = 'U') CREATE TABLE InsuranceClaims (Id INT PRIMARY KEY IDENTITY(1,1), BillId INT NOT NULL, PatientId INT NOT NULL, InsuranceProvider NVARCHAR(MAX), PolicyNumber NVARCHAR(MAX), ClaimNumber NVARCHAR(MAX), ClaimAmount DECIMAL(18,2) NOT NULL, ApprovedAmount DECIMAL(18,2) NOT NULL, SubmittedDate DATETIME2 NOT NULL, ProcessedDate DATETIME2 NULL, Status NVARCHAR(MAX), RejectionReason NVARCHAR(MAX), Notes NVARCHAR(MAX))");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Schema migration failed: {ex.Message}");
            }
        }

        private static void LoadFromDb()
        {
            using (var uow = CreateUnitOfWork())
            {
                Users = uow.Users.GetAll().ToList();
                Doctors = uow.Doctors.GetAll().ToList();
                Patients = uow.Patients.GetAll().ToList();
                Appointments = uow.Appointments.GetAll().ToList();
                Feedbacks = uow.Feedbacks.GetAll().ToList();
                MedicalRecords = uow.MedicalRecords.GetAll().ToList();
                Prescriptions = uow.Prescriptions.GetAll().ToList();
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
                Bills = uow.Bills.GetAll().ToList();
            }
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

        public static void SeedInventory()
        {
            if (Inventory.Any()) return;
            Inventory.Add(new InventoryItem { Name = "Paracetamol 500mg", Category = "Analgesics", StockQuantity = 500, SellingUnitPrice = 2.50m });
            Inventory.Add(new InventoryItem { Name = "Amoxicillin 250mg", Category = "Antibiotics", StockQuantity = 200, SellingUnitPrice = 15.00m });
            SaveInventory();
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

        public static User AuthenticateUser(string username, string password)
        {
            var user = Users.FirstOrDefault(u => u.Username == username);
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
            if (slotTime.TimeOfDay < ClinicOpStart || slotTime.TimeOfDay >= ClinicOpEnd) return false;
            return !Appointments.Any(a => a.DoctorId == doctorId && a.AppointmentDate == slotTime && a.Status != "Cancelled");
        }

        public static List<DateTime> GetAvailableTimeSlots(int doctorId, DateTime date)
        {
            var slots = new List<DateTime>();
            var doc = Doctors.FirstOrDefault(d => d.Id == doctorId);
            if (doc == null || !doc.IsActive) return slots;
            string dayShort = date.ToString("ddd");
            if (doc.WorkingDays != null && !doc.WorkingDays.Contains(dayShort)) return slots;
            DateTime current = date.Date.Add(doc.WorkingHoursStart);
            DateTime end = date.Date.Add(doc.WorkingHoursEnd);
            while (current < end) {
                if (IsSlotAvailable(doctorId, current)) slots.Add(current);
                current = current.AddMinutes(30);
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
                    var newUser = new User { Username = patient.Phone, Password = PasswordHasher.HashPassword(patient.Password ?? "1234"), Role = "Patient", PatientId = patient.Id, IsActive = true };
                    Users.Add(newUser);
                    SaveUsers();
                }
                if (patient.PatientCode == "PAT-PENDING" && patient.Id != 0) {
                    patient.PatientCode = $"PAT-{patient.Id:D5}";
                    SavePatients();
                }
            }
            catch (Exception ex) { LogAudit("System", $"Patient registration failed: {ex.Message}", "Error"); throw; }
        }

        public static void AddFeedback(Feedback f) { Feedbacks.Add(f); SaveFeedbacks(); }
        public static List<Appointment> GetPatientAppointments(int patientId) => Appointments.Where(a => a.PatientId == patientId).ToList();
        public static List<MedicalRecord> GetPatientMedicalRecords(int patientId) => MedicalRecords.Where(r => r.PatientId == patientId).ToList();
        public static List<Notification> GetPatientNotifications(int patientId) => Notifications.Where(n => n.PatientId == patientId).ToList();
        public static List<Feedback> GetDoctorFeedback(int doctorId) => Feedbacks.Where(f => f.DoctorId == doctorId).ToList();

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

        public static void LogAudit(string user, string action, string module = "Security") {
            AuditLogs.Add(new AuditLogEntry { Timestamp = DateTime.Now, Username = user ?? "System", Action = action, Module = module });
        }

        public static void SaveAllData() { 
            SaveUsers(); SaveDoctors(); SavePatients(); SaveAppointments(); SaveFeedbacks();
            SaveMedicalRecords(); SavePrescriptions(); SaveLabTests(); SaveNotifications();
            SaveNurses(); SavePharmacists(); SaveInventory(); SaveLabTechnicians();
            SaveBills(); SavePayments(); SaveInsuranceClaims(); SaveBillingStaff();
        }

        public static void SaveUsers() { using (var uow = CreateUnitOfWork()) { foreach(var u in Users) { if(u.Id == 0) uow.Users.Add(u); else uow.Users.Update(u); } uow.Complete(); } }
        public static void SaveDoctors() { using (var uow = CreateUnitOfWork()) { foreach(var d in Doctors) { if(d.Id == 0) uow.Doctors.Add(d); else uow.Doctors.Update(d); } uow.Complete(); } }
        public static void SavePatients() { using (var uow = CreateUnitOfWork()) { foreach(var p in Patients.ToList()) { if(p.Id == 0) uow.Patients.Add(p); else uow.Patients.Update(p); } uow.Complete(); } }
        public static void SaveAppointments() { using (var uow = CreateUnitOfWork()) { foreach(var a in Appointments) { if(a.Id == 0) uow.Appointments.Add(a); else uow.Appointments.Update(a); } uow.Complete(); } }
        public static void SaveFeedbacks() { using (var uow = CreateUnitOfWork()) { foreach(var f in Feedbacks) { if(f.Id == 0) uow.Feedbacks.Add(f); else uow.Feedbacks.Update(f); } uow.Complete(); } }
        public static void SaveMedicalRecords() { using (var uow = CreateUnitOfWork()) { foreach(var m in MedicalRecords) { if(m.Id == 0) uow.MedicalRecords.Add(m); else uow.MedicalRecords.Update(m); } uow.Complete(); } }
        public static void SavePrescriptions() { using (var uow = CreateUnitOfWork()) { foreach(var p in Prescriptions) { if(p.Id == 0) uow.Prescriptions.Add(p); else uow.Prescriptions.Update(p); } uow.Complete(); } }
        public static void SaveLabTests() { using (var uow = CreateUnitOfWork()) { foreach(var l in LabTests) { if(l.Id == 0) uow.LabTests.Add(l); else uow.LabTests.Update(l); } uow.Complete(); } }
        public static void SaveNotifications() { using (var uow = CreateUnitOfWork()) { foreach(var n in Notifications) { if(n.Id == 0) uow.Notifications.Add(n); else uow.Notifications.Update(n); } uow.Complete(); } }
        public static void SaveNurses() { using (var uow = CreateUnitOfWork()) { foreach(var n in Nurses) { if(n.Id == 0) uow.Nurses.Add(n); else uow.Nurses.Update(n); } uow.Complete(); } }
        public static void SavePharmacists() { using (var uow = CreateUnitOfWork()) { foreach(var p in Pharmacists) { if(p.Id == 0) uow.Pharmacists.Add(p); else uow.Pharmacists.Update(p); } uow.Complete(); } }
        public static void SaveInventory() { using (var uow = CreateUnitOfWork()) { foreach(var i in Inventory) { if(i.Id == 0) uow.InventoryItems.Add(i); else uow.InventoryItems.Update(i); } uow.Complete(); } }
        public static void SaveLabTechnicians() { using (var uow = CreateUnitOfWork()) { foreach(var l in LabTechnicians) { if(l.Id == 0) uow.LabTechnicians.Add(l); else uow.LabTechnicians.Update(l); } uow.Complete(); } }
        
        public static void SaveBills() { using (var uow = CreateUnitOfWork()) { foreach(var b in Bills) { if(b.Id == 0) uow.Bills.Add(b); else uow.Bills.Update(b); } uow.Complete(); } }
        public static void SavePayments() { using (var uow = CreateUnitOfWork()) { foreach(var p in Payments) { if(p.Id == 0) uow.Payments.Add(p); else uow.Payments.Update(p); } uow.Complete(); } }
        public static void SaveInsuranceClaims() { using (var uow = CreateUnitOfWork()) { foreach(var c in InsuranceClaims) { if(c.Id == 0) uow.InsuranceClaims.Add(c); else uow.InsuranceClaims.Update(c); } uow.Complete(); } }
        public static void SaveBillingStaff() { using (var uow = CreateUnitOfWork()) { foreach(var s in BillingStaff) { if(s.Id == 0) uow.BillingStaff.Add(s); else uow.BillingStaff.Update(s); } uow.Complete(); } }

        public static void BackupData() { LastBackupTime = DateTime.Now; }
    }
}
