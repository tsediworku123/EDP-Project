using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
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
            set { SetProperty(ref _searchText, value); LoadPrescriptions(); }
        }

        public ICommand RefreshCommand { get; }
        public ICommand DispenseCommand { get; }
        public ICommand ViewCommand { get; }
        public ICommand PrintExternalCommand { get; }

        public PharmacistPrescriptionsViewModel()
        {
            DataManager.EnsureLoaded();
            RefreshCommand = new RelayCommand(LoadPrescriptions);
            DispenseCommand = new RelayCommand<PrescriptionListItem>(DispensePrescription);
            ViewCommand = new RelayCommand<PrescriptionListItem>(ViewPrescription);
            PrintExternalCommand = new RelayCommand<PrescriptionListItem>(PrintForExternalPharmacy);
            LoadPrescriptions();
        }

        private void LoadPrescriptions()
        {
            Prescriptions.Clear();
            DataManager.ReloadInventory();

            var query = DataManager.Prescriptions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lower = SearchText.ToLower();
                var matchedPatientIds = DataManager.Patients
                    .Where(p => (p.FullName != null && p.FullName.ToLower().Contains(lower))
                             || (p.PatientCode != null && p.PatientCode.ToLower().Contains(lower)))
                    .Select(p => p.Id).ToList();

                query = query.Where(p => matchedPatientIds.Contains(p.PatientId)
                                      || (p.Status != null && p.Status.ToLower().Contains(lower)));
            }

            foreach (var pres in query.OrderByDescending(p => p.PrescribedDate))
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == pres.PatientId);
                var doctor = DataManager.Doctors.FirstOrDefault(d => d.Id == pres.DoctorId);

                // Check drug availability for each item in the prescription
                int totalItems = pres.Items.Count;
                int availableItems = 0;
                int outOfStockItems = 0;

                foreach (var item in pres.Items)
                {
                    var inv = DataManager.Inventory.FirstOrDefault(i =>
                        i.Name != null && i.Name.ToLower() == item.MedicineName?.ToLower());

                    if (inv != null && inv.StockQuantity >= item.Quantity)
                        availableItems++;
                    else
                        outOfStockItems++;
                }

                string availStatus = "Available";
                if (outOfStockItems == totalItems) availStatus = "OutOfStock";
                else if (outOfStockItems > 0) availStatus = "Partial";

                string availLabel = availStatus == "Available" ? "All Available"
                                  : availStatus == "Partial"   ? $"{outOfStockItems}/{totalItems} Missing"
                                  : "Out of Stock";

                Prescriptions.Add(new PrescriptionListItem
                {
                    Id = pres.Id,
                    PatientName = patient?.FullName ?? "Unknown",
                    CardNo = patient?.PatientCode ?? "",
                    DoctorName = doctor?.FullName ?? "Unknown",
                    Date = pres.PrescribedDate.ToString("MMM dd, yyyy"),
                    Status = pres.Status,
                    ItemsCount = pres.Items.Count,
                    AvailabilityStatus = availStatus,
                    AvailabilityLabel = availLabel
                });
            }
        }

        private void DispensePrescription(PrescriptionListItem item)
        {
            if (item == null) return;

            var pres = DataManager.Prescriptions.FirstOrDefault(p => p.Id == item.Id);
            if (pres == null || pres.Status != "Pending") return;

            // Build missing items list
            var missingItems = new System.Text.StringBuilder();
            bool anyMissing = false;

            foreach (var pItem in pres.Items)
            {
                var inv = DataManager.Inventory.FirstOrDefault(i =>
                    i.Name != null && i.Name.ToLower() == pItem.MedicineName?.ToLower());

                if (inv == null || inv.StockQuantity < pItem.Quantity)
                {
                    anyMissing = true;
                    int available = inv?.StockQuantity ?? 0;
                    missingItems.AppendLine($"  • {pItem.MedicineName}  (Need: {pItem.Quantity}, Have: {available})");
                }
            }

            if (anyMissing)
            {
                var result = System.Windows.MessageBox.Show(
                    $"The following drugs are insufficient in stock:\n\n{missingItems}\n" +
                    "Would you like to print a prescription for the patient to obtain from another pharmacy?",
                    "Insufficient Stock",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (result == System.Windows.MessageBoxResult.Yes)
                    ViewPrescription(item);

                return;
            }

            // All available — dispense and deduct
            pres.Status = "Dispensed";
            foreach (var pItem in pres.Items)
            {
                var inv = DataManager.Inventory.FirstOrDefault(i =>
                    i.Name != null && i.Name.ToLower() == pItem.MedicineName?.ToLower());
                if (inv != null) inv.StockQuantity -= pItem.Quantity;
            }

            DataManager.SaveAllData();
            LoadPrescriptions();
            System.Windows.MessageBox.Show(
                $"Prescription for {item.PatientName} dispensed successfully.",
                "Dispensed", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private async void ViewPrescription(PrescriptionListItem item)
        {
            if (item == null) return;
            var pres = DataManager.Prescriptions.FirstOrDefault(p => p.Id == item.Id);
            if (pres != null)
            {
                var vm = new PrintPrescriptionViewModel(pres);
                var dialog = new HMS.Core.Views.PrintPrescriptionDialog { DataContext = vm };
                await MaterialDesignThemes.Wpf.DialogHost.Show(dialog, "PharmacistDialogHost");
            }
        }

        private async void PrintForExternalPharmacy(PrescriptionListItem item)
        {
            if (item == null) return;
            var pres = DataManager.Prescriptions.FirstOrDefault(p => p.Id == item.Id);
            if (pres != null)
            {
                var vm = new PrintPrescriptionViewModel(pres);
                var dialog = new HMS.Core.Views.PrintPrescriptionDialog { DataContext = vm };
                await MaterialDesignThemes.Wpf.DialogHost.Show(dialog, "PharmacistDialogHost");
            }
        }
    }

    public class PrescriptionListItem
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string CardNo { get; set; }
        public string DoctorName { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public int ItemsCount { get; set; }
        public string AvailabilityStatus { get; set; } = "Available"; // Available, Partial, OutOfStock
        public string AvailabilityLabel { get; set; } = "All Available";
        public bool IsPending => Status == "Pending";
    }
}
