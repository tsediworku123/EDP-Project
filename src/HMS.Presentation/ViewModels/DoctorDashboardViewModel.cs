using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Views; // Needed to instantiate Views
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class DoctorDashboardViewModel : ObservableObject
    {
        public ObservableCollection<object> TodayAppointments { get; } = new ObservableCollection<object>();
        public int TotalPatientCount => DataManager.Patients.Count;
        public int AppointmentsTodayCount { get; private set; }

        // Event to request a view change in the shell
        public Action<object, string> RequestViewChange { get; set; }

        // Core Doctor Functionality Commands
        public ICommand ViewPatientRecordsCommand { get; }
        public ICommand DiagnoseIllnessesCommand { get; }
        public ICommand PrescribeMedicationsCommand { get; }
        public ICommand RequestLabTestsCommand { get; }
        public ICommand ViewTestResultsCommand { get; }
        public ICommand ManageAppointmentsCommand { get; }
        public ICommand WriteDischargeSummariesCommand { get; }

        public DoctorDashboardViewModel()
        {
            LoadData();

            // Initialize Commands to navigate properly via the Action
            ViewPatientRecordsCommand = new RelayCommand(() => RequestViewChange?.Invoke(new DoctorRecordsView(), "MEDICAL RECORDS"));
            
            ManageAppointmentsCommand = new RelayCommand(() => RequestViewChange?.Invoke(new DoctorAppointmentsView(), "APPOINTMENTS"));
            DiagnoseIllnessesCommand = new RelayCommand(() => RequestViewChange?.Invoke(new DoctorDiagnoseView(), "DIAGNOSIS"));
            PrescribeMedicationsCommand = new RelayCommand(() => RequestViewChange?.Invoke(new DoctorPrescribeView(), "PRESCRIPTIONS"));
            RequestLabTestsCommand = new RelayCommand(() => RequestViewChange?.Invoke(new DoctorLabTestsView(), "REQUEST LABS"));
            ViewTestResultsCommand = new RelayCommand(() => RequestViewChange?.Invoke(new DoctorTestResultsView(), "TEST RESULTS"));
            WriteDischargeSummariesCommand = new RelayCommand(() => RequestViewChange?.Invoke(new DoctorDischargeView(), "DISCHARGE SUMMARY"));
        }

        private void LoadData()
        {
            DataManager.EnsureLoaded();
            var currentDoctor = CurrentSession.Instance.LoggedInDoctor;
            if (currentDoctor == null) return;

            var appointments = DataManager.Appointments
                .Where(a => a.DoctorId == currentDoctor.Id && a.AppointmentDate.Date == DateTime.Today)
                .OrderBy(a => a.AppointmentDate);

            AppointmentsTodayCount = appointments.Count();

            foreach (var app in appointments)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == app.PatientId);
                TodayAppointments.Add(new
                {
                    PatientName = patient?.FullName ?? "Unknown",
                    Reason = app.Reason,
                    Time = app.AppointmentDate.ToString("hh:mm tt")
                });
            }
        }
    }
}
