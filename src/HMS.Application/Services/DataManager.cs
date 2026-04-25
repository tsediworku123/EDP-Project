using HMS.Core.Domain.Entities;
using HMS.Core.Persistence;
using HMS.Core.Persistence.Repositories;
using HMS.Core.Domain.Interfaces;
using HMS.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

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

            // Ensure database is created
            using (var context = DatabaseFactory.CreateContext())
            {
                context.Database.EnsureCreated();
            }

            LoadFromDb();

            if (Doctors.Count < 5) SeedClinicalData();
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
                new Doctor { FullName = "Dr. Aisha Patel", Specialization = "Dermatology", Department = "Dermatology", IsActive = true, WorkingDays = "Mon,Tue,Thu,Fri", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(17,30,0) },
                new Doctor { FullName = "Dr. Thomas Miller", Specialization = "General Medicine", Department = "General Medicine", IsActive = true, WorkingDays = "Mon,Wed,Fri", WorkingHoursStart = new TimeSpan(8,30,0), WorkingHoursEnd = new TimeSpan(16,30,0) },
                new Doctor { FullName = "Dr. Linda Zhao", Specialization = "Internal Med", Department = "General Medicine", IsActive = true, WorkingDays = "Tue,Wed,Fri", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(17,0,0) },
                new Doctor { FullName = "Dr. Kevin Brooks", Specialization = "Psychiatry", Department = "Psychiatry", IsActive = true, WorkingDays = "Mon,Thu,Sat", WorkingHoursStart = new TimeSpan(10,0,0), WorkingHoursEnd = new TimeSpan(18,0,0) },
                new Doctor { FullName = "Dr. Sophia Walsh", Specialization = "Gynecology", Department = "Gynecology", IsActive = true, WorkingDays = "Mon,Tue,Wed,Fri", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(15,0,0) },
                new Doctor { FullName = "Dr. David Kim", Specialization = "Dental Care", Department = "Dental", IsActive = true, WorkingDays = "Mon,Wed,Fri", WorkingHoursStart = new TimeSpan(8,0,0), WorkingHoursEnd = new TimeSpan(17,0,0) },
                new Doctor { FullName = "Dr. Angela Hope", Specialization = "ENT Surgery", Department = "ENT", IsActive = true, WorkingDays = "Tue,Thu,Sat", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(17,0,0) },
                new Doctor { FullName = "Dr. James Carter", Specialization = "General Medicine", Department = "General Medicine", IsActive = true, WorkingDays = "Mon,Tue,Wed,Thu,Fri", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(18,0,0) }
            };

            foreach(var d in clinicalStaff) if(!Doctors.Any(x => x.FullName == d.FullName)) Doctors.Add(d);
            SaveDoctors();

            var currentPat = Patients.FirstOrDefault();
            if (currentPat != null && !Appointments.Any(a => a.PatientId == currentPat.Id))
            {
                Appointments.Add(new Appointment { PatientId = currentPat.Id, DoctorId = 1, AppointmentDate = DateTime.Now.AddDays(-10), Status = "Completed", Reason = "Initial consultation for migraine.", Diagnosis = "Stress-induced Migraine", Recommendation = "Reduced screen time." });
                Appointments.Add(new Appointment { PatientId = currentPat.Id, DoctorId = 2, AppointmentDate = DateTime.Now.AddDays(-5), Status = "Completed", Reason = "Follow-up blood work.", Diagnosis = "Vitamin D Deficiency", Recommendation = "Take Vitamin D supplement." });
                Appointments.Add(new Appointment { PatientId = currentPat.Id, DoctorId = 3, AppointmentDate = DateTime.Now.AddDays(-1), Status = "Completed", Reason = "Cardio checkup.", PatientRating = 0 });
                Appointments.Add(new Appointment { PatientId = currentPat.Id, DoctorId = 4, AppointmentDate = DateTime.Now.AddDays(3), Status = "Scheduled", Reason = "Annual checkup." });
            }
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
            if (CurrentUser == null || CurrentUser.Role != "Patient") return null;
            return Patients.FirstOrDefault(p => p.Id == CurrentUser.PatientId);
        }

        public static bool AddAppointment(Appointment appointment)
        {
            if (appointment.AppointmentDate < DateTime.Now.AddMinutes(-5)) return false; 
            appointment.Id = Appointments.Any() ? Appointments.Max(a => a.Id) + 1 : 1;
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
                // Add to in-memory list if not already there
                if (!Patients.Contains(patient))
                {
                    Patients.Add(patient);
                }

                // 1. Save patient first to generate database record
                // The database handles ID generation via Identity
                if (string.IsNullOrEmpty(patient.PatientCode)) 
                {
                    patient.PatientCode = "PAT-PENDING"; 
                }
                
                SavePatients(); 

                // 2. Now that we have patient.Id (updated by EF), create the corresponding user
                if (patient.Id != 0 && !Users.Any(u => u.Role == "Patient" && u.PatientId == patient.Id))
                {
                    var newUser = new User 
                    { 
                        Username = patient.Phone, 
                        Password = PasswordHasher.HashPassword(patient.Password ?? "password123"), 
                        Role = "Patient", 
                        PatientId = patient.Id,
                        IsActive = true 
                    };
                    Users.Add(newUser);
                    SaveUsers();
                }
                
                // 3. Update patient code with the generated ID if it was pending
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

        public static void AddFeedback(Feedback f) { Feedbacks.Add(f); SaveFeedbacks(); }

        public static List<Appointment> GetPatientAppointments(int patientId) => Appointments.Where(a => a.PatientId == patientId).ToList();
        public static List<MedicalRecord> GetPatientMedicalRecords(int patientId) => MedicalRecords.Where(r => r.PatientId == patientId).ToList();
        public static List<Notification> GetPatientNotifications(int patientId) => Notifications.Where(n => n.PatientId == patientId).ToList();
        public static List<Feedback> GetDoctorFeedback(int doctorId) => Feedbacks.Where(f => f.DoctorId == doctorId).ToList();

        public static Appointment GetLastPatientVisit(int patientId)
        {
            return Appointments
                .Where(a => a.PatientId == patientId && a.AppointmentDate < DateTime.Now && a.Status == "Completed")
                .OrderByDescending(a => a.AppointmentDate)
                .FirstOrDefault();
        }

        public static Appointment GetNextPatientAppointment(int patientId)
        {
            return Appointments
                .Where(a => a.PatientId == patientId && a.AppointmentDate >= DateTime.Now && a.Status != "Cancelled")
                .OrderBy(a => a.AppointmentDate)
                .FirstOrDefault();
        }

        public static bool HasRated(int appointmentId) => Feedbacks.Any(f => f.AppointmentId == appointmentId);
        public static bool HasBeenRated(int appointmentId) => Feedbacks.Any(f => f.AppointmentId == appointmentId);

        public static double GetDoctorAverageRating(int doctorId) 
        {
            var f = GetDoctorFeedback(doctorId);
            return f.Any() ? f.Average(x => x.Rating) : 0;
        }

        public static Patient FindPatientByPhone(string phone) => Patients.FirstOrDefault(p => p.Phone == phone);

        public static void MarkNotificationAsRead(int notificationId)
        {
            var n = Notifications.FirstOrDefault(x => x.Id == notificationId);
            if (n != null) n.IsRead = true;
        }

        public static void LogAudit(string user, string action, string module = "Security") {
            AuditLogs.Add(new AuditLogEntry { Timestamp = DateTime.Now, Username = user ?? "System", Action = action, Module = module });
        }

        public static void SaveAllData() { 
            SaveUsers();
            SaveDoctors();
            SavePatients();
            SaveAppointments();
            SaveFeedbacks();
            SaveMedicalRecords();
            SavePrescriptions();
            SaveLabTests();
        }

        public static void SaveUsers() { 
            using (var uow = CreateUnitOfWork())
            {
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

        public static void BackupData() { LastBackupTime = DateTime.Now; }
    }
}
