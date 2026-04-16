using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class AdminReportsViewModel : ObservableObject
    {
        private DateTime _startDate = DateTime.Today.AddDays(-30);
        private DateTime _endDate = DateTime.Today;
        private int _totalPatients;
        private int _totalDoctors;
        private int _completedAppointments;
        private string _totalRevenue;

        public DateTime StartDate { get => _startDate; set => SetProperty(ref _startDate, value); }
        public DateTime EndDate { get => _endDate; set => SetProperty(ref _endDate, value); }
        
        public int TotalPatients { get => _totalPatients; set => SetProperty(ref _totalPatients, value); }
        public int TotalDoctors { get => _totalDoctors; set => SetProperty(ref _totalDoctors, value); }
        public int CompletedAppointments { get => _completedAppointments; set => SetProperty(ref _completedAppointments, value); }
        public string TotalRevenue { get => _totalRevenue; set => SetProperty(ref _totalRevenue, value); }

        public ICommand ExportPatientsCommand { get; }
        public ICommand ExportAppointmentsCommand { get; }
        public ICommand ExportFinancialCommand { get; }
        public ICommand RefreshStatsCommand { get; }

        public AdminReportsViewModel()
        {
            ExportPatientsCommand = new RelayCommand(ExportPatients);
            ExportAppointmentsCommand = new RelayCommand(ExportAppointments);
            ExportFinancialCommand = new RelayCommand(ExportFinancial);
            RefreshStatsCommand = new RelayCommand(LoadStats);
            
            LoadStats();
        }

        private void LoadStats()
        {
            DataManager.EnsureLoaded();
            TotalPatients = DataManager.Patients.Count;
            TotalDoctors = DataManager.Doctors.Count;
            
            var completed = DataManager.Appointments.Where(a => a.Status == "Completed");
            CompletedAppointments = completed.Count();
            
            decimal revenue = completed.Where(a => a.IsPaid).Sum(a => a.ConsultationFee);
            TotalRevenue = revenue.ToString("C0"); // Currency format
        }

        private void ExportPatients()
        {
            var patients = DataManager.Patients;
            var csv = new StringBuilder();
            csv.AppendLine("ID,Full Name,Email,Gender,Phone,Blood Group");

            foreach (var p in patients)
            {
                csv.AppendLine($"{p.Id},\"{p.FullName}\",{p.Email},{p.Gender},{p.Phone},{p.BloodGroup}");
            }

            SaveCsv(csv.ToString(), "PatientsReport");
        }

        private void ExportAppointments()
        {
            var appointments = DataManager.Appointments
                .Where(a => a.AppointmentDate.Date >= StartDate.Date && a.AppointmentDate.Date <= EndDate.Date)
                .OrderBy(a => a.AppointmentDate);

            var csv = new StringBuilder();
            csv.AppendLine("ID,PatientID,DoctorID,Date,Status,Type,Reason,Paid");

            foreach (var a in appointments)
            {
                csv.AppendLine($"{a.Id},{a.PatientId},{a.DoctorId},{a.AppointmentDate:yyyy-MM-dd HH:mm},{a.Status},{a.AppointmentType},\"{a.Reason}\",{a.IsPaid}");
            }

            SaveCsv(csv.ToString(), "AppointmentsReport");
        }

        private void ExportFinancial()
        {
            var completedPaid = DataManager.Appointments
                .Where(a => a.Status == "Completed" && a.IsPaid && 
                        a.AppointmentDate.Date >= StartDate.Date && a.AppointmentDate.Date <= EndDate.Date);

            var csv = new StringBuilder();
            csv.AppendLine("AppointmentID,Date,PatientID,Fee,Payment Status");

            foreach (var a in completedPaid)
            {
                csv.AppendLine($"{a.Id},{a.AppointmentDate:yyyy-MM-dd},{a.PatientId},{a.ConsultationFee},Paid");
            }
            
            csv.AppendLine($",,,TOTAL,{completedPaid.Sum(a => a.ConsultationFee)}");

            SaveCsv(csv.ToString(), "FinancialSummary");
        }

        private void SaveCsv(string content, string defaultName)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                FileName = $"{defaultName}_{DateTime.Now:yyyyMMdd_HHmm}.csv"
            };

            if (sfd.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(sfd.FileName, content);
                    MessageBox.Show("Report exported successfully!", "Export Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to export: {ex.Message}", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
