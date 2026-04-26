using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class PharmacistPrescriptionsViewModel : ObservableObject
    {
        public ObservableCollection<PrescriptionListItem> Prescriptions { get; } = new ObservableCollection<PrescriptionListItem>();
        
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                LoadPrescriptions();
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand DispenseCommand { get; }

        public PharmacistPrescriptionsViewModel()
        {
            DataManager.EnsureLoaded();
            
            RefreshCommand = new RelayCommand(LoadPrescriptions);
            DispenseCommand = new RelayCommand<PrescriptionListItem>(DispensePrescription);

            LoadPrescriptions();
        }

        private void LoadPrescriptions()
        {
            Prescriptions.Clear();
            
            var query = DataManager.Prescriptions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lowerSearch = SearchText.ToLower();
                // Find patient ids that match
                var matchedPatientIds = DataManager.Patients
                    .Where(p => p.FullName.ToLower().Contains(lowerSearch) || p.PatientCode.ToLower().Contains(lowerSearch))
                    .Select(p => p.Id)
                    .ToList();

                query = query.Where(p => matchedPatientIds.Contains(p.PatientId) || p.Status.ToLower().Contains(lowerSearch));
            }

            foreach (var pres in query.OrderByDescending(p => p.PrescribedDate))
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == pres.PatientId);
                var doctor = DataManager.Doctors.FirstOrDefault(d => d.Id == pres.DoctorId);

                Prescriptions.Add(new PrescriptionListItem
                {
                    Id = pres.Id,
                    PatientName = patient?.FullName ?? "Unknown",
                    DoctorName = doctor?.FullName ?? "Unknown",
                    Date = pres.PrescribedDate.ToString("MMM dd, yyyy"),
                    Status = pres.Status,
                    ItemsCount = pres.Items.Count
                });
            }
        }

        private void DispensePrescription(PrescriptionListItem item)
        {
            if (item == null) return;
            
            var pres = DataManager.Prescriptions.FirstOrDefault(p => p.Id == item.Id);
            if (pres != null && pres.Status == "Pending")
            {
                pres.Status = "Dispensed";
                
                // Deduct from inventory (dummy logic or actual logic if inventory items match)
                foreach(var pItem in pres.Items)
                {
                    var invItem = DataManager.Inventory.FirstOrDefault(i => i.Name.ToLower() == pItem.MedicineName.ToLower());
                    if (invItem != null && invItem.StockQuantity >= pItem.Quantity)
                    {
                        invItem.StockQuantity -= pItem.Quantity;
                    }
                }
                
                LoadPrescriptions(); // Refresh
                System.Windows.MessageBox.Show("Prescription marked as Dispensed.", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
        }
    }

    public class PrescriptionListItem
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public int ItemsCount { get; set; }
        
        public bool IsPending => Status == "Pending";
    }
}
