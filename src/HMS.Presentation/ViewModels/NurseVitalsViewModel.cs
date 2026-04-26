using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using HMS.Core.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class NurseVitalsViewModel : ObservableObject
    {
        private string _searchText;

        public ObservableCollection<PatientVitalDisplay> FilteredVitals { get; } = new ObservableCollection<PatientVitalDisplay>();

        public string SearchText 
        { 
            get => _searchText; 
            set { SetProperty(ref _searchText, value); ApplyFilter(); } 
        }

        public ICommand UpdateVitalsCommand { get; }
        public ICommand RefreshCommand { get; }

        public NurseVitalsViewModel()
        {
            DataManager.EnsureLoaded();
            
            UpdateVitalsCommand = new RelayCommand<PatientVitalDisplay>(OpenUpdateDialog);
            RefreshCommand = new RelayCommand(LoadData);
            LoadData();
        }

        private void LoadData()
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            FilteredVitals.Clear();
            var vitals = DataManager.PatientVitals.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var kw = SearchText.ToLower();
                vitals = vitals.Where(v => 
                    (v.PatientName != null && v.PatientName.ToLower().Contains(kw)) || 
                    (v.RoomNumber != null && v.RoomNumber.ToLower().Contains(kw)));
            }

            foreach (var v in vitals.OrderBy(v => v.RoomNumber))
            {
                FilteredVitals.Add(new PatientVitalDisplay
                {
                    Id = v.Id,
                    PatientId = v.PatientId,
                    PatientName = v.PatientName,
                    RoomNumber = v.RoomNumber,
                    BP = v.BloodPressure,
                    Temperature = v.Temperature,
                    Pulse = v.Pulse,
                    SPO2 = v.SPO2,
                    LastUpdated = v.LastUpdated.ToString("hh:mm tt, MMM dd")
                });
            }
        }

        private async void OpenUpdateDialog(PatientVitalDisplay displayItem)
        {
            if (displayItem == null) return;

            // Find the actual persisted entity
            var entity = DataManager.PatientVitals.FirstOrDefault(v => v.Id == displayItem.Id);
            if (entity == null) return;

            var editModel = new VitalsEditModel
            {
                PatientName = entity.PatientName,
                RoomNumber = entity.RoomNumber,
                BP = entity.BloodPressure,
                Temperature = entity.Temperature,
                Pulse = entity.Pulse,
                SPO2 = entity.SPO2
            };

            var dialog = new UpdateVitalsDialog { DataContext = editModel };

            object result = null;
            try
            {
                result = await DialogHost.Show(dialog, "NurseDialogHost");
            }
            catch
            {
                // Fallback if dialog host not available
                return;
            }

            if ((result is bool saved && saved) || (result is string s && s == "True"))
            {
                // Update the persisted entity
                entity.BloodPressure = editModel.BP;
                entity.Temperature = editModel.Temperature;
                entity.Pulse = editModel.Pulse;
                entity.SPO2 = editModel.SPO2;
                entity.LastUpdated = DateTime.Now;
                entity.UpdatedBy = CurrentSession.Instance.LoggedInNurse?.FullName ?? "Nurse";

                // Save to database
                try
                {
                    DataManager.SavePatientVitals();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Save vitals failed: {ex.Message}");
                }

                // Refresh the UI list
                ApplyFilter();
            }
        }
    }

    /// <summary>
    /// Display model with INotifyPropertyChanged so the DataGrid updates when values change.
    /// </summary>
    public class PatientVitalDisplay : ObservableObject
    {
        private int _id;
        private int _patientId;
        private string _patientName;
        private string _roomNumber;
        private string _bp;
        private string _temperature;
        private string _pulse;
        private string _spo2;
        private string _lastUpdated;

        public int Id { get => _id; set => SetProperty(ref _id, value); }
        public int PatientId { get => _patientId; set => SetProperty(ref _patientId, value); }
        public string PatientName { get => _patientName; set => SetProperty(ref _patientName, value); }
        public string RoomNumber { get => _roomNumber; set => SetProperty(ref _roomNumber, value); }
        public string BP { get => _bp; set => SetProperty(ref _bp, value); }
        public string Temperature { get => _temperature; set => SetProperty(ref _temperature, value); }
        public string Pulse { get => _pulse; set => SetProperty(ref _pulse, value); }
        public string SPO2 { get => _spo2; set => SetProperty(ref _spo2, value); }
        public string LastUpdated { get => _lastUpdated; set => SetProperty(ref _lastUpdated, value); }
    }

    public class VitalsEditModel : ObservableObject
    {
        private string _patientName;
        private string _roomNumber;
        private string _bp;
        private string _temperature;
        private string _pulse;
        private string _spo2;

        public string PatientName { get => _patientName; set => SetProperty(ref _patientName, value); }
        public string RoomNumber { get => _roomNumber; set => SetProperty(ref _roomNumber, value); }
        public string BP { get => _bp; set => SetProperty(ref _bp, value); }
        public string Temperature { get => _temperature; set => SetProperty(ref _temperature, value); }
        public string Pulse { get => _pulse; set => SetProperty(ref _pulse, value); }
        public string SPO2 { get => _spo2; set => SetProperty(ref _spo2, value); }
    }
}
