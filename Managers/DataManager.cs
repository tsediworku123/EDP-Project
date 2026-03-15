using ClinicAppointmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicAppointmentSystem
{
    public static class DataManager
    {
        public static List<Doctor> Doctors { get; set; } = new List<Doctor>();
        public static List<Patient> Patients { get; set; } = new List<Patient>();
        public static List<User> Users { get; set; } = new List<User>();
        public static List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public static List<Feedback> Feedbacks { get; set; } = new List<Feedback>(); // NEW
        public static List<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>(); // NEW
        public static List<Notification> Notifications { get; set; } = new List<Notification>(); // NEW

        public static User CurrentUser { get; set; } // Track logged-in user

        static DataManager()
        {
            if (!Users.Any(u => u.Role == "Admin"))
            {
                Users.Add(new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin123",
                    Role = "Admin",
                    PatientId = 0,
                    Email = "admin@clinic.com"
                });
            }

            // Add sample patient user for testing
            if (!Users.Any(u => u.Role == "Patient"))
            {
                Users.Add(new User
                {
                    Id = 2,
                    Username = "john",
                    Password = "1234",
                    Role = "Patient",
                    PatientId = 1,
                    Email = "john@email.com"
                });
            }

            // Rest of your initialization...
        }

        private static void InitializeSampleData()
        {
            // Doctors
            if (!Doctors.Any())
            {
                Doctors.Add(new Doctor { Id = 1, FullName = "Dr. Sarah Johnson", Specialization = "Cardiology", Gender = "Female", Phone = "123-456-7890" });
                Doctors.Add(new Doctor { Id = 2, FullName = "Dr. Michael Chen", Specialization = "Neurology", Gender = "Male", Phone = "234-567-8901" });
                Doctors.Add(new Doctor { Id = 3, FullName = "Dr. Emily Williams", Specialization = "Pediatrics", Gender = "Female", Phone = "345-678-9012" });
                Doctors.Add(new Doctor { Id = 4, FullName = "Dr. James Brown", Specialization = "Dermatology", Gender = "Male", Phone = "456-789-0123" });
                Doctors.Add(new Doctor { Id = 5, FullName = "Dr. Lisa Garcia", Specialization = "Gynecology", Gender = "Female", Phone = "567-890-1234" });
            }

            // Patients
            if (!Patients.Any())
            {
                Patients.Add(new Patient { Id = 1, FullName = "John Doe", Phone = "111-222-3333", Gender = "Male", DateOfBirth = new System.DateTime(1985, 5, 15), Address = "123 Main St" });
                Patients.Add(new Patient { Id = 2, FullName = "Jane Smith", Phone = "444-555-6666", Gender = "Female", DateOfBirth = new System.DateTime(1990, 8, 22), Address = "456 Oak Ave" });
            }

            // Users
            if (!Users.Any(u => u.Role == "Admin"))
            {
                Users.Add(new User { Id = 1, Username = "admin", Password = "admin123", Role = "Admin", PatientId = 0 });
            }
            if (!Users.Any(u => u.Role == "Patient"))
            {
                Users.Add(new User { Id = 2, Username = "john", Password = "1234", Role = "Patient", PatientId = 1 });
                Users.Add(new User { Id = 3, Username = "jane", Password = "1234", Role = "Patient", PatientId = 2 });
            }

            // Appointments
            if (!Appointments.Any())
            {
                Appointments.Add(new Appointment { Id = 1, PatientId = 1, DoctorId = 1, AppointmentDate = System.DateTime.Now.AddDays(2), Reason = "Chest pain", Status = "Confirmed" });
                Appointments.Add(new Appointment { Id = 2, PatientId = 1, DoctorId = 3, AppointmentDate = System.DateTime.Now.AddDays(5), Reason = "Child checkup", Status = "Pending" });
                Appointments.Add(new Appointment { Id = 3, PatientId = 2, DoctorId = 2, AppointmentDate = System.DateTime.Now.AddDays(1), Reason = "Headaches", Status = "Confirmed" });
            }

            // Medical Records
            if (!MedicalRecords.Any())
            {
                MedicalRecords.Add(new MedicalRecord { Id = 1, PatientId = 1, DoctorId = 1, DoctorName = "Dr. Sarah Johnson", VisitDate = System.DateTime.Now.AddMonths(-1), Diagnosis = "Mild hypertension", Prescription = "Lisinopril 10mg daily", Notes = "Follow up in 3 months" });
                MedicalRecords.Add(new MedicalRecord { Id = 2, PatientId = 1, DoctorId = 3, DoctorName = "Dr. Emily Williams", VisitDate = System.DateTime.Now.AddMonths(-2), Diagnosis = "Seasonal allergies", Prescription = "Cetirizine 10mg as needed", Notes = "Avoid triggers" });
            }

            // Notifications
            Notifications = new List<Notification>();
        }

        // ========== USER METHODS ==========
        public static User AuthenticateUser(string username, string password)
        {
            var user = Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
                CurrentUser = user;
            return user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static Patient GetCurrentPatient()
        {
            if (CurrentUser == null || CurrentUser.Role != "Patient")
                return null;
            return Patients.FirstOrDefault(p => p.Id == CurrentUser.PatientId);
        }

        // ========== APPOINTMENT METHODS ==========
        public static List<Appointment> GetPatientAppointments(int patientId)
        {
            return Appointments.Where(a => a.PatientId == patientId)
                              .OrderByDescending(a => a.AppointmentDate)
                              .ToList();
        }

        public static void AddAppointment(Appointment appointment)
        {
            appointment.Id = Appointments.Any() ? Appointments.Max(a => a.Id) + 1 : 1;
            Appointments.Add(appointment);
            AddNotification(new Notification
            {
                PatientId = appointment.PatientId,
                Title = "Appointment Booked",
                Message = $"Your appointment with Dr. {Doctors.FirstOrDefault(d => d.Id == appointment.DoctorId)?.FullName} on {appointment.AppointmentDate:MMM dd, yyyy} is pending confirmation.",
                Type = "Info"
            });
        }

        public static void CancelAppointment(int appointmentId)
        {
            var apt = Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (apt != null)
            {
                apt.Status = "Cancelled";
                AddNotification(new Notification
                {
                    PatientId = apt.PatientId,
                    Title = "Appointment Cancelled",
                    Message = $"Your appointment on {apt.AppointmentDate:MMM dd, yyyy} has been cancelled.",
                    Type = "Warning"
                });
            }
        }

        public static void RescheduleAppointment(int appointmentId, DateTime newDate)
        {
            var apt = Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (apt != null)
            {
                apt.AppointmentDate = newDate;
                apt.Status = "Pending";
                AddNotification(new Notification
                {
                    PatientId = apt.PatientId,
                    Title = "Appointment Rescheduled",
                    Message = $"Your appointment has been rescheduled to {newDate:MMM dd, yyyy}.",
                    Type = "Info"
                });
            }
        }

        // ========== NOTIFICATION METHODS ==========
        public static void AddNotification(Notification notification)
        {
            notification.Id = Notifications.Any() ? Notifications.Max(n => n.Id) + 1 : 1;
            notification.CreatedAt = DateTime.Now;
            notification.IsRead = false;
            Notifications.Add(notification);
        }

        public static List<Notification> GetPatientNotifications(int patientId)
        {
            return Notifications.Where(n => n.PatientId == patientId)
                               .OrderByDescending(n => n.CreatedAt)
                               .ToList();
        }

        public static void MarkNotificationAsRead(int notificationId)
        {
            var notif = Notifications.FirstOrDefault(n => n.Id == notificationId);
            if (notif != null)
                notif.IsRead = true;
        }

        // ========== FEEDBACK METHODS ==========
        public static void AddFeedback(Feedback feedback)
        {
            feedback.Id = Feedbacks.Any() ? Feedbacks.Max(f => f.Id) + 1 : 1;
            feedback.FeedbackDate = DateTime.Now;
            Feedbacks.Add(feedback);
        }

        public static List<Feedback> GetDoctorFeedbacks(int doctorId)
        {
            return Feedbacks.Where(f => f.DoctorId == doctorId).ToList();
        }

        public static double GetDoctorAverageRating(int doctorId)
        {
            var ratings = Feedbacks.Where(f => f.DoctorId == doctorId).Select(f => f.Rating);
            return ratings.Any() ? ratings.Average() : 0;
        }

        // ========== MEDICAL RECORDS ==========
        public static List<MedicalRecord> GetPatientMedicalRecords(int patientId)
        {
            return MedicalRecords.Where(m => m.PatientId == patientId)
                                .OrderByDescending(m => m.VisitDate)
                                .ToList();
        }

        // ========== SAVE METHODS ==========
        public static void SaveAllData() { /* JSON serialization would go here */ }
        public static void SaveDoctors() { }
        public static void SavePatients() { }
        public static void SaveUsers() { }
        public static void SaveAppointments() { }
    }
}