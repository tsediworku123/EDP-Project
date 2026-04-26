using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class NurseVitalsViewModel : ObservableObject
    {
        private string _searchText;
        private PatientVitalItem _selectedPatient;

        public ObservableCollection<PatientVitalItem> PatientVitals { get; } = new ObservableCollection<PatientVitalItem>();
        public ObservableCollection<PatientVitalItem> FilteredVitals { get; } = new ObservableCollection<PatientVitalItem>();

        public string SearchText 
        { 
            get => _searchText; 
            set { SetProperty(ref _searchText, value); ApplyFilter(); } 
        }

        public PatientVitalItem SelectedPatient 
        { 
            get => _selectedPatient; 
            set => SetProperty(ref _selectedPatient, value); 
        }

        public ICommand UpdateVitalsCommand { get; }
        public ICommand RefreshCommand { get; }

        public NurseVitalsViewModel()
        {
            DataManager.EnsureLoaded();
            LoadData();
            
            UpdateVitalsCommand = new RelayCommand(OpenUpdateDialog);
            RefreshCommand = new RelayCommand(LoadData);
        }

        private void LoadData()
        {
            PatientVitals.Clear();
            foreach (var p in DataManager.Patients.Take(15)) // Just taking some for demo
            {
                PatientVitals.Add(new PatientVitalItem
                {
                    PatientId = p.Id,
                    PatientName = p.FullName,
                    RoomNumber = "Room " + (200 + p.Id % 50),
                    BP = "120/80",
                    Temperature = "36.8°C",
                    Pulse = "72 bpm",
                    SPO2 = "98%",
                    LastUpdated = DateTime.Now.AddHours(-2).ToString("hh:mm tt")
                });
            }
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            FilteredVitals.Clear();
            var query = PatientVitals.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var kw = SearchText.ToLower();
                query = query.Where(v => v.PatientName.ToLower().Contains(kw) || v.RoomNumber.ToLower().Contains(kw));
            }
            foreach (var v in query) FilteredVitals.Add(v);
        }

        private void OpenUpdateDialog()
        {
            // Logic to open a dialog to enter new vitals
        }
    }

    public class PatientVitalItem
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string RoomNumber { get; set; }
        public string BP { get; set; }
        public string Temperature { get; set; }
        public string Pulse { get; set; }
        public string SPO2 { get; set; }
        public string LastUpdated { get; set; }
    }
}
