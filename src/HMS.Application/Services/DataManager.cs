using HMS.Core.Domain.Entities;
using HMS.Core.Infrastructure.Repositories.Json;
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
        private static bool isInitialized = false;

        public static User CurrentUser { get; set; }
        public static TimeSpan ClinicOpStart = new TimeSpan(0, 0, 0);
        public static TimeSpan ClinicOpEnd = new TimeSpan(23, 59, 59);
        public static int DefaultSlotDuration = 30;
        public static bool AllowDoubleBooking = false;
        public static DateTime LastBackupTime { get; set; } = DateTime.MinValue;

        private static readonly JsonDataService _jsonService = new JsonDataService();

        static DataManager()
        {
            EnsureLoaded();
        }

        public static void EnsureLoaded()
        {
            if (isInitialized) return;
            isInitialized = true;

            if (Users == null || Users.Count == 0) Users = _jsonService.LoadUsers();
            if (Doctors == null || Doctors.Count == 0) Doctors = _jsonService.LoadDoctors();
            if (Patients == null || Patients.Count == 0) Patients = _jsonService.LoadPatients();
            if (Appointments == null || Appointments.Count == 0) Appointments = _jsonService.LoadAppointments();
            if (Feedbacks == null || Feedbacks.Count == 0) Feedbacks = _jsonService.LoadFeedbacks();
            if (MedicalRecords == null || MedicalRecords.Count == 0) MedicalRecords = _jsonService.LoadMedicalRecords();
            
            var dynamicDepts = _jsonService.LoadDepartments();
            if (dynamicDepts != null && dynamicDepts.Any()) Departments = dynamicDepts;

            if (Doctors.Count < 5) SeedClinicalData();
        }

        public static void SeedClinicalData()
        {
            Doctors.Clear();

            var clinicalStaff = new List<Doctor> {
                new Doctor { Id = 1, FullName = "Dr. Elena Rodriguez", Specialization = "Cardiology", Department = "Cardiology", IsActive = true, WorkingDays = "Mon,Tue,Wed,Thu,Fri", WorkingHoursStart = new TimeSpan(8,0,0), WorkingHoursEnd = new TimeSpan(16,0,0) },
                new Doctor { Id = 2, FullName = "Dr. Marcus Thorne", Specialization = "Orthopedics", Department = "Orthopedics", IsActive = true, WorkingDays = "Mon,Tue,Wed,Fri", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(17,0,0) },
                new Doctor { Id = 3, FullName = "Dr. Sarah Jenkins", Specialization = "Pediatrics", Department = "Pediatrics", IsActive = true, WorkingDays = "Mon,Wed,Fri", WorkingHoursStart = new TimeSpan(10,0,0), WorkingHoursEnd = new TimeSpan(16,0,0) },
                new Doctor { Id = 4, FullName = "Dr. Robert Chen", Specialization = "Neurology", Department = "Neurology", IsActive = true, WorkingDays = "Tue,Thu,Sat", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(17,0,0) },
                new Doctor { Id = 5, FullName = "Dr. Aisha Patel", Specialization = "Dermatology", Department = "Dermatology", IsActive = true, WorkingDays = "Mon,Tue,Thu,Fri", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(17,30,0) },
                new Doctor { Id = 6, FullName = "Dr. Thomas Miller", Specialization = "General Medicine", Department = "General Medicine", IsActive = true, WorkingDays = "Mon,Wed,Fri", WorkingHoursStart = new TimeSpan(8,30,0), WorkingHoursEnd = new TimeSpan(16,30,0) },
                new Doctor { Id = 7, FullName = "Dr. Linda Zhao", Specialization = "Internal Med", Department = "General Medicine", IsActive = true, WorkingDays = "Tue,Wed,Fri", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(17,0,0) },
                new Doctor { Id = 8, FullName = "Dr. Kevin Brooks", Specialization = "Psychiatry", Department = "Psychiatry", IsActive = true, WorkingDays = "Mon,Thu,Sat", WorkingHoursStart = new TimeSpan(10,0,0), WorkingHoursEnd = new TimeSpan(18,0,0) },
                new Doctor { Id = 10, FullName = "Dr. Sophia Walsh", Specialization = "Gynecology", Department = "Gynecology", IsActive = true, WorkingDays = "Mon,Tue,Wed,Fri", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(15,0,0) },
                new Doctor { Id = 11, FullName = "Dr. David Kim", Specialization = "Dental Care", Department = "Dental", IsActive = true, WorkingDays = "Mon,Wed,Fri", WorkingHoursStart = new TimeSpan(8,0,0), WorkingHoursEnd = new TimeSpan(17,0,0) },
                new Doctor { Id = 12, FullName = "Dr. Angela Hope", Specialization = "ENT Surgery", Department = "ENT", IsActive = true, WorkingDays = "Tue,Thu,Sat", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(17,0,0) },
                new Doctor { Id = 13, FullName = "Dr. James Carter", Specialization = "General Medicine", Department = "General Medicine", IsActive = true, WorkingDays = "Mon,Tue,Wed,Thu,Fri", WorkingHoursStart = new TimeSpan(9,0,0), WorkingHoursEnd = new TimeSpan(18,0,0) }
            };

            foreach(var d in clinicalStaff) if(!Doctors.Any(x => x.Id == d.Id)) Doctors.Add(d);
            SaveDoctors();

            var currentPat = Patients.FirstOrDefault();
            if (currentPat != null && !Appointments.Any(a => a.PatientId == currentPat.Id))
            {
                Appointments.Add(new Appointment { Id = 101, PatientId = currentPat.Id, DoctorId = 2, AppointmentDate = DateTime.Now.AddDays(-10), Status = "Completed", Reason = "Initial consultation for migraine.", Diagnosis = "Stress-induced Migraine", Recommendation = "Reduced screen time." });
                Appointments.Add(new Appointment { Id = 102, PatientId = currentPat.Id, DoctorId = 5, AppointmentDate = DateTime.Now.AddDays(-5), Status = "Completed", Reason = "Follow-up blood work.", Diagnosis = "Vitamin D Deficiency", Recommendation = "Take Vitamin D supplement." });
                Appointments.Add(new Appointment { Id = 103, PatientId = currentPat.Id, DoctorId = 6, AppointmentDate = DateTime.Now.AddDays(-1), Status = "Completed", Reason = "Cardio checkup.", PatientRating = 0 });
                Appointments.Add(new Appointment { Id = 104, PatientId = currentPat.Id, DoctorId = 1, AppointmentDate = DateTime.Now.AddDays(3), Status = "Scheduled", Reason = "Annual checkup." });
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
            if (patient.Id == 0) patient.Id = Patients.Any() ? Patients.Max(p => p.Id) + 1 : 1;
            if (string.IsNullOrEmpty(patient.PatientCode)) patient.PatientCode = $"PAT-{patient.Id:D5}";
            Patients.Add(patient);
            Users.Add(new User { Id = Users.Any() ? Users.Max(u => u.Id) + 1 : 1, Username = patient.Phone, Password = PasswordHasher.HashPassword(patient.Password), Role = "Patient", PatientId = patient.Id, IsActive = true });
            SavePatients(); SaveUsers();
        }

        public static void AddFeedback(Feedback f) { f.Id = Feedbacks.Any() ? Feedbacks.Max(x => x.Id) + 1 : 1; Feedbacks.Add(f); SaveFeedbacks(); }

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
            AuditLogs.Add(new AuditLogEntry { Id = AuditLogs.Count + 1, Timestamp = DateTime.Now, Username = user ?? "System", Action = action, Module = module });
        }

        public static void SaveAllData() { SaveUsers(); SaveDoctors(); SavePatients(); SaveAppointments(); SaveFeedbacks(); SaveMedicalRecords(); SaveDepartments(); }
        public static void SaveUsers() { _jsonService.SaveUsers(Users); }
        public static void SaveDoctors() { _jsonService.SaveDoctors(Doctors); }
        public static void SavePatients() { _jsonService.SavePatients(Patients); }
        public static void SaveAppointments() { _jsonService.SaveAppointments(Appointments); }
        public static void SaveFeedbacks() { _jsonService.SaveFeedbacks(Feedbacks); }
        public static void SaveMedicalRecords() { _jsonService.SaveMedicalRecords(MedicalRecords); }
        public static void SaveDepartments() { _jsonService.SaveDepartments(Departments); }

        public static void BackupData() { LastBackupTime = DateTime.Now; }
    }
}
