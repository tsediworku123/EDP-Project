using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using HMS.Core.AppLogic.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class PrintPrescriptionItemDto
    {
        public string DrugDetails { get; set; }
        public string Price { get; set; }
    }

    public class PrintPrescriptionViewModel : ObservableObject
    {
        public string PatientName { get; set; }
        public string Sex { get; set; }
        public string Age { get; set; }
        public string Weight { get; set; }
        public string CardNo { get; set; }
        
        public string Region { get; set; }
        public string Town { get; set; }
        public string Woreda { get; set; }
        public string Kebele { get; set; }
        public string HouseNo { get; set; }
        public string TelNo { get; set; }
        
        public bool IsInpatient { get; set; }
        public bool IsOutpatient { get; set; }
        
        public string Diagnosis { get; set; }
        
        public ObservableCollection<PrintPrescriptionItemDto> PrescriptionItems { get; set; } = new ObservableCollection<PrintPrescriptionItemDto>();
        public string TotalPrice { get; set; }

        public string PrescriberName { get; set; }
        public string PrescriberQual { get; set; }
        public string PrescriberReg { get; set; }
        public string PrescriberDate { get; set; }

        public string DispenserName { get; set; }
        public string DispenserQual { get; set; }
        public string DispenserReg { get; set; }
        public string DispenserDate { get; set; }

        public PrintPrescriptionViewModel(Prescription prescription)
        {
            if (prescription == null) return;

            var patient = DataManager.Patients.FirstOrDefault(p => p.Id == prescription.PatientId);
            var doctor = DataManager.Doctors.FirstOrDefault(d => d.Id == prescription.DoctorId);
            var pharmacist = CurrentSession.Instance.LoggedInPharmacist;

            if (patient != null)
            {
                PatientName = patient.FullName;
                Sex = patient.Gender;
                Age = patient.DateOfBirth != default ? (System.DateTime.Now.Year - patient.DateOfBirth.Year).ToString() : "";
                Weight = ""; // Not available in Patient entity
                CardNo = patient.PatientCode;
                Region = ""; // Not available in Patient entity
                Town = patient.Address; // Fallback to Address
                Woreda = "";
                Kebele = "";
                HouseNo = "";
                TelNo = patient.Phone;
                
                IsOutpatient = true; // Defaulting to Outpatient
            }

            var appt = DataManager.Appointments.OrderByDescending(a => a.AppointmentDate).FirstOrDefault(a => a.PatientId == prescription.PatientId);
            if (appt != null)
            {
                Diagnosis = appt.Diagnosis;
            }

            decimal total = 0;

            foreach (var item in prescription.Items)
            {
                var invItem = DataManager.Inventory.FirstOrDefault(i => i.Name.ToLower() == item.MedicineName.ToLower());
                decimal itemPrice = 0;
                string priceDisplay = "";

                if (invItem != null)
                {
                    itemPrice = invItem.SellingUnitPrice * item.Quantity;
                    total += itemPrice;
                    priceDisplay = itemPrice.ToString("C");
                }

                PrescriptionItems.Add(new PrintPrescriptionItemDto
                {
                    DrugDetails = $"{item.MedicineName}, {item.Dosage}, Qty: {item.Quantity}\n{item.Instructions}",
                    Price = priceDisplay
                });
            }

            TotalPrice = total > 0 ? total.ToString("C") : "";

            if (doctor != null)
            {
                PrescriberName = doctor.FullName;
                PrescriberQual = doctor.Specialization;
                PrescriberDate = prescription.PrescribedDate.ToString("dd/MM/yyyy");
            }

            if (pharmacist != null)
            {
                DispenserName = pharmacist.FullName;
                DispenserQual = "Pharmacist";
                DispenserReg = pharmacist.LicenseNumber;
                DispenserDate = System.DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
    }
}
