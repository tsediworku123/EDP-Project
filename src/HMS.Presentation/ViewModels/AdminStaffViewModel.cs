using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using HMS.Core.Views;

namespace HMS.Core.ViewModels
{
    public class StaffMemberDto
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public object OriginalEntity { get; set; }
    }

    public class AdminStaffViewModel : ObservableObject
    {
        private ObservableCollection<StaffMemberDto> _staffMembers;
        public ObservableCollection<StaffMemberDto> StaffMembers
        {
            get => _staffMembers;
            set => SetProperty(ref _staffMembers, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    FilterStaff();
            }
        }

        private string _selectedRoleFilter = "All Roles";
        public string SelectedRoleFilter
        {
            get => _selectedRoleFilter;
            set
            {
                if (SetProperty(ref _selectedRoleFilter, value))
                    FilterStaff();
            }
        }

        public List<string> RoleFilters { get; } = new List<string>
        {
            "All Roles",
            "Doctor",
            "Nurse",
            "Pharmacist",
            "Lab Technician",
            "Billing Staff"
        };

        public int TotalActiveStaff => StaffMembers?.Count(s => s.IsActive) ?? 0;
        public int TotalStaffCount => StaffMembers?.Count ?? 0;

        public ICommand RefreshCommand { get; }
        public ICommand AddStaffCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand DeleteStaffCommand { get; }

        public AdminStaffViewModel()
        {
            DataManager.EnsureLoaded();
            RefreshData();

            RefreshCommand = new RelayCommand(RefreshData);
            AddStaffCommand = new RelayCommand(AddStaff);
            ViewDetailsCommand = new RelayCommand<StaffMemberDto>(ViewDetails);
            DeleteStaffCommand = new RelayCommand<StaffMemberDto>(DeleteStaff);
        }

        private List<StaffMemberDto> GetAllStaff()
        {
            var list = new List<StaffMemberDto>();

            if (DataManager.Doctors != null)
                list.AddRange(DataManager.Doctors.Select(d => new StaffMemberDto { Id = d.Id, Role = "Doctor", FullName = d.FullName, Specialization = d.Specialization, Phone = d.PhoneNumber, Email = d.Email, IsActive = d.IsActive, OriginalEntity = d }));

            if (DataManager.Nurses != null)
                list.AddRange(DataManager.Nurses.Select(n => new StaffMemberDto { Id = n.Id, Role = "Nurse", FullName = n.FullName, Specialization = n.Specialization, Phone = n.Phone, Email = n.Email, IsActive = n.IsActive, OriginalEntity = n }));

            if (DataManager.Pharmacists != null)
                list.AddRange(DataManager.Pharmacists.Select(p => new StaffMemberDto { Id = p.Id, Role = "Pharmacist", FullName = p.FullName, Specialization = "Pharmacy", Phone = p.Phone, Email = p.Email, IsActive = p.IsActive, OriginalEntity = p }));

            if (DataManager.LabTechnicians != null)
                list.AddRange(DataManager.LabTechnicians.Select(l => new StaffMemberDto { Id = l.Id, Role = "Lab Technician", FullName = l.FullName, Specialization = l.Specialization ?? "Laboratory", Phone = l.Phone, Email = l.Email, IsActive = l.IsActive, OriginalEntity = l }));

            if (DataManager.BillingStaff != null)
                list.AddRange(DataManager.BillingStaff.Select(b => new StaffMemberDto { Id = b.Id, Role = "Billing Staff", FullName = b.FullName, Specialization = b.Position ?? "Billing", Phone = b.Phone, Email = b.Email, IsActive = b.IsActive, OriginalEntity = b }));

            return list;
        }

        private void RefreshData()
        {
            FilterStaff();
        }

        private void FilterStaff()
        {
            var all = GetAllStaff();
            var query = all.AsEnumerable();

            if (SelectedRoleFilter != "All Roles")
            {
                query = query.Where(s => s.Role == SelectedRoleFilter);
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lowerSearch = SearchText.ToLower();
                query = query.Where(s => 
                    (s.FullName != null && s.FullName.ToLower().Contains(lowerSearch)) ||
                    (s.Specialization != null && s.Specialization.ToLower().Contains(lowerSearch)) ||
                    (s.Phone != null && s.Phone.ToLower().Contains(lowerSearch))
                );
            }

            StaffMembers = new ObservableCollection<StaffMemberDto>(query);
            OnPropertyChanged(nameof(TotalActiveStaff));
            OnPropertyChanged(nameof(TotalStaffCount));
        }

        private async void AddStaff()
        {
            // We will use a unified AddStaffDialog
            var view = new AddStaffDialog();
            var result = await DialogHost.Show(view, "MainDialogHost");
            if (result != null && result.ToString() == "True")
            {
                RefreshData();
            }
        }

        private async void ViewDetails(StaffMemberDto staff)
        {
            if (staff == null) return;
            // Show details dialog
            // For now, let's just show a message box or a simple details view
            // We can implement a more complex details view later
            MessageBox.Show($"Staff Member: {staff.FullName}\nRole: {staff.Role}\nDept: {staff.Specialization}\nEmail: {staff.Email}\nPhone: {staff.Phone}", "Staff Details", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteStaff(StaffMemberDto staff)
        {
            if (staff == null) return;

            var result = MessageBox.Show($"Are you sure you want to remove {staff.FullName} ({staff.Role}) from the system?", "Confirm Deletion", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            
            if (result == MessageBoxResult.OK)
            {
                if (staff.OriginalEntity is Doctor d) DataManager.Doctors.Remove(d);
                else if (staff.OriginalEntity is Nurse n) DataManager.Nurses.Remove(n);
                else if (staff.OriginalEntity is Pharmacist p) DataManager.Pharmacists.Remove(p);
                else if (staff.OriginalEntity is LabTechnician l) DataManager.LabTechnicians.Remove(l);
                else if (staff.OriginalEntity is BillingStaff b) DataManager.BillingStaff.Remove(b);

                DataManager.SaveAllData();
                RefreshData();
            }
        }
    }
}
