using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;

namespace HMS.Core.ViewModels
{
    public class NurseAdmissionsViewModel : ObservableObject
    {
        public ObservableCollection<AdmissionItem> Admissions { get; } = new ObservableCollection<AdmissionItem>();

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); LoadAdmissions(); }
        }

        public int TotalAdmissionsCount => Admissions.Count;
        public ICommand RefreshCommand { get; }

        public NurseAdmissionsViewModel()
        {
            DataManager.EnsureLoaded();
            RefreshCommand = new RelayCommand(LoadAdmissions);
            LoadAdmissions();
        }

        private void LoadAdmissions()
        {
            Admissions.Clear();

            // Build admission items from patients + appointments
            var items = DataManager.Patients
                .Select(p => new AdmissionItem
                {
                    PatientId   = p.Id,
                    PatientName = p.FullName,
                    PatientCode = p.PatientCode,
                    Ward        = "General Ward",
                    AdmitDate   = DateTime.Now.AddDays(-p.Id).ToString("MMM dd, yyyy"),
                    Status      = p.Id % 2 == 0 ? "Admitted" : "Discharged"
                })
                .ToList();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var s = SearchText.ToLower();
                items = items.Where(i => i.PatientName.ToLower().Contains(s) ||
                                          i.Status.ToLower().Contains(s)).ToList();
            }

            foreach (var item in items)
                Admissions.Add(item);

            OnPropertyChanged(nameof(TotalAdmissionsCount));
        }
    }

    public class AdmissionItem
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientCode { get; set; }
        public string Ward { get; set; }
        public string AdmitDate { get; set; }
        public string Status { get; set; }
    }
}
