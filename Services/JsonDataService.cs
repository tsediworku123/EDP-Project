using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Services
{
    public class JsonDataService
    {
        private static readonly string DataDirectory = GetDataDirectory();
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();

        private static string GetDataDirectory()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dir = new DirectoryInfo(baseDir);
            while (dir != null && dir.Exists)
            {
                string candidate = Path.Combine(dir.FullName, "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = dir.Parent;
            }
            return Path.Combine(baseDir, "Data");
        }
        
        public JsonDataService()
        {
            if (!Directory.Exists(DataDirectory))
                Directory.CreateDirectory(DataDirectory);
        }

        private string GetFilePath(string fileName) => Path.Combine(DataDirectory, fileName);

        public List<T> LoadData<T>(string fileName)
        {
            string path = GetFilePath(fileName);
            if (!File.Exists(path)) return new List<T>();

            try
            {
                string json = File.ReadAllText(path);
                return _serializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading {fileName}: {ex.Message}");
                return new List<T>();
            }
        }

        public void SaveData<T>(List<T> data, string fileName)
        {
            string path = GetFilePath(fileName);
            try
            {
                string json = _serializer.Serialize(data);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving {fileName}: {ex.Message}");
            }
        }

        // Specific helpers for the requested files
        public List<User> LoadUsers() => LoadData<User>("users.json");
        public void SaveUsers(List<User> users) => SaveData(users, "users.json");

        public List<Doctor> LoadDoctors() => LoadData<Doctor>("doctors.json");
        public void SaveDoctors(List<Doctor> doctors) => SaveData(doctors, "doctors.json");

        public List<Patient> LoadPatients() => LoadData<Patient>("patients.json");
        public void SavePatients(List<Patient> patients) => SaveData(patients, "patients.json");

        public List<Appointment> LoadAppointments() => LoadData<Appointment>("appointments.json");
        public void SaveAppointments(List<Appointment> appointments) => SaveData(appointments, "appointments.json");

        public List<Feedback> LoadFeedbacks() => LoadData<Feedback>("feedbacks.json");
        public void SaveFeedbacks(List<Feedback> feedbacks) => SaveData(feedbacks, "feedbacks.json");

        public List<MedicalRecord> LoadMedicalRecords() => LoadData<MedicalRecord>("medical_records.json");
        public void SaveMedicalRecords(List<MedicalRecord> records) => SaveData(records, "medical_records.json");

        public List<string> LoadDepartments() => LoadData<string>("departments.json");
        public void SaveDepartments(List<string> departments) => SaveData(departments, "departments.json");
    }
}
