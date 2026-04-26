 using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HMS.Core.ViewModels
{
    public class CreateInvoiceViewModel : ObservableObject
    {
        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set => SetProperty(ref _selectedPatient, value);
        }

        public ObservableCollection<Patient> Patients => new ObservableCollection<Patient>(DataManager.Patients);

        public class BillItemWrapper : ObservableObject
        {
            public BillItem Item { get; }
            public Action OnChanged { get; }

            public BillItemWrapper(BillItem item, Action onChanged)
            {
                Item = item;
                OnChanged = onChanged;
            }

            public string Description
            {
                get => Item.Description;
                set { Item.Description = value; OnPropertyChanged(); }
            }

            public decimal UnitPrice
            {
                get => Item.UnitPrice;
                set 
                { 
                    Item.UnitPrice = value; 
                    Item.TotalPrice = Item.UnitPrice * Item.Quantity;
                    OnPropertyChanged(); 
                    OnPropertyChanged(nameof(TotalPrice));
                    OnChanged?.Invoke();
                }
            }

            public int Quantity
            {
                get => Item.Quantity;
                set 
                { 
                    Item.Quantity = value; 
                    Item.TotalPrice = Item.UnitPrice * Item.Quantity;
                    OnPropertyChanged(); 
                    OnPropertyChanged(nameof(TotalPrice));
                    OnChanged?.Invoke();
                }
            }

            public decimal TotalPrice => Item.TotalPrice;
        }

        private ObservableCollection<BillItemWrapper> _items = new ObservableCollection<BillItemWrapper>();
        public ObservableCollection<BillItemWrapper> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);

        public ICommand AddItemCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand SaveInvoiceCommand { get; }

        public CreateInvoiceViewModel()
        {
            AddItemCommand = new RelayCommand(ExecuteAddItem);
            RemoveItemCommand = new RelayCommand<BillItemWrapper>(ExecuteRemoveItem);
            SaveInvoiceCommand = new RelayCommand(ExecuteSaveInvoice, CanSaveInvoice);
            
            ExecuteAddItem();
        }

        private void ExecuteAddItem()
        {
            var item = new BillItem { Description = "New Service", UnitPrice = 0, Quantity = 1, TotalPrice = 0 };
            Items.Add(new BillItemWrapper(item, () => OnPropertyChanged(nameof(TotalAmount))));
            OnPropertyChanged(nameof(TotalAmount));
        }

        private void ExecuteRemoveItem(BillItemWrapper wrapper)
        {
            if (wrapper != null)
            {
                Items.Remove(wrapper);
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        private bool CanSaveInvoice() => SelectedPatient != null && Items.Any();

        private void ExecuteSaveInvoice()
        {
            var bill = new Bill
            {
                PatientId = SelectedPatient.Id,
                BillDate = DateTime.Now,
                InvoiceNumber = $"INV-{DateTime.Now:yyyyMMdd}-{DataManager.Bills.Count + 1:D3}",
                TotalAmount = TotalAmount,
                Status = "Unpaid",
                Items = Items.Select(i => i.Item).ToList()
            };

            DataManager.Bills.Add(bill);
            DataManager.SaveBills();

            MaterialDesignThemes.Wpf.DialogHost.Close("BillingDialogHost");
        }
    }
}
