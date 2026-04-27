using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using HMS.Core.Common.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class PatientPrescriptionsViewModel : ObservableObject
    {
        public ObservableCollection<PatientPrescriptionListItem> Prescriptions { get; } = new ObservableCollection<PatientPrescriptionListItem>();

        public PatientPrescriptionsViewModel()
        {
            LoadPrescriptions();
        }

        private void LoadPrescriptions()
        {
            DataManager.EnsureLoaded();
            var patient = DataManager.GetCurrentPatient();
            if (patient == null) return;

            var allPrescriptions = DataManager.GetPatientPrescriptions(patient.Id)
                .OrderByDescending(p => p.PrescribedDate)
                .ToList();

            Prescriptions.Clear();
            foreach (var p in allPrescriptions)
            {
                var doctor = DataManager.GetDoctorById(p.DoctorId);
                var items = DataManager.GetItemsForPrescription(p.Id);
                
                string medicinesSummary = "No medications specified";
                if (items != null && items.Count > 0)
                {
                    medicinesSummary = string.Join(", ", items.Select(i => i.MedicineName));
                }

                Prescriptions.Add(new PatientPrescriptionListItem
                {
                    PrescriptionId = p.Id,
                    DoctorName = doctor != null ? $"Dr. {doctor.FullName}" : "Unknown Doctor",
                    PrescriptionDate = p.PrescribedDate,
                    Status = p.Status,
                    MedicinesSummary = medicinesSummary,
                    Notes = p.Notes
                });
            }
        }
    }
    public class PatientPrescriptionListItem
    {
        public int PrescriptionId { get; set; }
        public string DoctorName { get; set; }
        public System.DateTime PrescriptionDate { get; set; }
        public string Status { get; set; }
        public string MedicinesSummary { get; set; }
        public string Notes { get; set; }
    }
}
