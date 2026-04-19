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

        private static readonly IUnitOfWork _unitOfWork = new UnitOfWork(DatabaseFactory.CreateContext());

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
            Users = _unitOfWork.Users.GetAll().ToList();
            Doctors = _unitOfWork.Doctors.GetAll().ToList();
            Patients = _unitOfWork.Patients.GetAll().ToList();
            Appointments = _unitOfWork.Appointments.GetAll().ToList();
            Feedbacks = _unitOfWork.Feedbacks.GetAll().ToList();
            MedicalRecords = _unitOfWork.MedicalRecords.GetAll().ToList();
            Prescriptions = _unitOfWork.Prescriptions.GetAll().ToList();
            LabTests = _unitOfWork.LabTests.GetAll().ToList();
            Notifications = _unitOfWork.Notifications.GetAll().ToList();
            AuditLogs = _unitOfWork.AuditLogs.GetAll().ToList();
            
            // Departments are currently hardcoded in list, but we could load from DB if there was a table
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
            // Identity columns handle ID generation
            if (string.IsNullOrEmpty(patient.PatientCode)) 
            {
                 // We can't know the ID yet, so we use a temp code or update after save
                 patient.PatientCode = $"PAT-PENDING"; 
            }
            Patients.Add(patient);
            
            // Note: In a real app, we would save the patient first to get the ID, 
            // then create the user with that ID.
            Users.Add(new User { 
                Username = patient.Phone, 
                Password = PasswordHasher.HashPassword(patient.Password), 
                Role = "Patient", 
                IsActive = true 
            });
            
            SavePatients(); 
            SaveUsers();
            
            // Update patient code with the generated ID
            if (patient.PatientCode == "PAT-PENDING") {
                patient.PatientCode = $"PAT-{patient.Id:D5}";
                SavePatients();
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

        public static void SaveAllData() { _unitOfWork.Complete(); }
        public static void SaveUsers() { 
            foreach(var u in Users) {
                if(u.Id == 0) _unitOfWork.Users.Add(u);
                else _unitOfWork.Users.Update(u);
            }
            _unitOfWork.Complete(); 
        }
        public static void SaveDoctors() { 
            foreach(var d in Doctors) {
                if(d.Id == 0) _unitOfWork.Doctors.Add(d);
                else _unitOfWork.Doctors.Update(d);
            }
            _unitOfWork.Complete(); 
        }
        public static void SavePatients() { 
             foreach(var p in Patients) {
                if(p.Id == 0) _unitOfWork.Patients.Add(p);
                else _unitOfWork.Patients.Update(p);
            }
            _unitOfWork.Complete(); 
        }
        public static void SaveAppointments() { 
            foreach(var a in Appointments) {
                if(a.Id == 0) _unitOfWork.Appointments.Add(a);
                else _unitOfWork.Appointments.Update(a);
            }
            _unitOfWork.Complete(); 
        }
        public static void SaveFeedbacks() { 
            foreach(var f in Feedbacks) {
                if(f.Id == 0) _unitOfWork.Feedbacks.Add(f);
                else _unitOfWork.Feedbacks.Update(f);
            }
            _unitOfWork.Complete(); 
        }
        public static void SaveMedicalRecords() { 
            foreach(var m in MedicalRecords) {
                if(m.Id == 0) _unitOfWork.MedicalRecords.Add(m);
                else _unitOfWork.MedicalRecords.Update(m);
            }
            _unitOfWork.Complete(); 
        }
        public static void SaveDepartments() { /* Not implemented in DB yet */ }
        public static void SavePrescriptions() { 
            foreach(var p in Prescriptions) {
                if(p.Id == 0) _unitOfWork.Prescriptions.Add(p);
                else _unitOfWork.Prescriptions.Update(p);
            }
            _unitOfWork.Complete(); 
        }
        public static void SaveLabTests() { 
            foreach(var l in LabTests) {
                if(l.Id == 0) _unitOfWork.LabTests.Add(l);
                else _unitOfWork.LabTests.Update(l);
            }
            _unitOfWork.Complete(); 
        }

        public static void BackupData() { LastBackupTime = DateTime.Now; }
    }
}
