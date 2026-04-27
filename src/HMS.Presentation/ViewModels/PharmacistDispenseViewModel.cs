using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class PharmacistDispenseViewModel : ObservableObject
    {
        private readonly Prescription _prescription;
        private readonly Patient _patient;
        private readonly Doctor _doctor;
        private string _dispensingNotes;

        public Prescription Prescription => _prescription;
        public Patient Patient => _patient;
        public Doctor Doctor => _doctor;

        public ObservableCollection<DispenseItemViewModel> Items { get; } = new ObservableCollection<DispenseItemViewModel>();

        public string DispensingNotes
        {
            get => _dispensingNotes;
            set => SetProperty(ref _dispensingNotes, value);
        }

        public decimal GrandTotal => Items.Sum(i => i.TotalPrice);

        public ICommand DispenseCommand { get; }
        public ICommand CancelCommand { get; }

        public PharmacistDispenseViewModel(Prescription prescription)
        {
            _prescription = prescription;
            _patient = DataManager.Patients.FirstOrDefault(p => p.Id == prescription.PatientId);
            _doctor = DataManager.Doctors.FirstOrDefault(d => d.Id == prescription.DoctorId);

            LoadItems();

            DispenseCommand = new RelayCommand(ExecuteDispense);
            CancelCommand = new RelayCommand(() => MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(false, null));
        }

        private void LoadItems()
        {
            Items.Clear();
            foreach (var item in _prescription.Items)
            {
                var inv = DataManager.Inventory.FirstOrDefault(i =>
                    i.Name != null && i.Name.ToLower() == item.MedicineName?.ToLower());

                Items.Add(new DispenseItemViewModel(item, inv));
            }
        }

        private void ExecuteDispense()
        {
            // 1. Validate availability
            var outOfStock = Items.Where(i => !i.IsAvailable).ToList();
            if (outOfStock.Any())
            {
                var msg = "The following items are out of stock or insufficient:\n" +
                          string.Join("\n", outOfStock.Select(i => $" - {i.MedicineName}"));
                MessageBox.Show(msg, "Inventory Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Update stock and prescription items
            foreach (var item in Items)
            {
                if (item.InventorySource != null)
                {
                    item.InventorySource.StockQuantity -= item.Quantity;
                }
                
                item.Source.IsDispensed = true;
                item.Source.PharmacistInstructions = item.PharmacistInstructions;
                item.Source.UnitPrice = item.UnitPrice;
            }

            // 3. Update prescription status
            _prescription.Status = "Dispensed";
            _prescription.DispensedDate = DateTime.Now;
            _prescription.DispensedBy = CurrentSession.Instance.LoggedInPharmacist?.FullName ?? "Pharmacist";
            _prescription.DispensingNotes = DispensingNotes;

            // 4. Create Billing Entry
            CreateBill();

            // 5. Save and Close
            DataManager.SaveAllData();
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(true, null);
            
            MessageBox.Show($"Prescription for {_patient?.FullName} has been dispensed and billed.", 
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CreateBill()
        {
            var bill = new Bill
            {
                PatientId = _prescription.PatientId,
                BillDate = DateTime.Now,
                Status = "Unpaid",
                FinancialNotes = $"Pharmacy Billing for Prescription #{_prescription.Id}",
                Items = Items.Select(i => new BillItem
                {
                    Description = $"{i.MedicineName} ({i.Dosage})",
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    TotalPrice = i.TotalPrice,
                    ItemType = "Pharmacy"
                }).ToList()
            };
            bill.TotalAmount = bill.Items.Sum(bi => bi.TotalPrice);
            
            DataManager.Bills.Add(bill);
            DataManager.SaveBills();
        }
    }

    public class DispenseItemViewModel : ObservableObject
    {
        public PrescriptionItem Source { get; }
        public InventoryItem InventorySource { get; }

        public string MedicineName => Source.MedicineName;
        public string Dosage => Source.Dosage;
        public int Quantity => Source.Quantity;
        public string DoctorInstructions => Source.Instructions;
        
        private string _pharmacistInstructions;
        public string PharmacistInstructions
        {
            get => _pharmacistInstructions;
            set => SetProperty(ref _pharmacistInstructions, value);
        }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;

        public bool IsAvailable => InventorySource != null && InventorySource.StockQuantity >= Quantity;
        public string StockStatus => InventorySource == null ? "Not in Inventory" 
                                    : $"In Stock: {InventorySource.StockQuantity}";

        public DispenseItemViewModel(PrescriptionItem source, InventoryItem inventory)
        {
            Source = source;
            InventorySource = inventory;
            UnitPrice = inventory?.SellingUnitPrice ?? 0;
            PharmacistInstructions = "Take as directed by doctor.";
        }
    }
}
