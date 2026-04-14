using HMS.Core.AppLogic.Services;
using HMS.Core.Domain.Entities;
using HMS.Core.Common.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class MedicalHistoryViewModel : ObservableObject
    {
        public ObservableCollection<MedicalRecord> Records { get; } = new ObservableCollection<MedicalRecord>();
        public string PatientName { get; set; }

        public MedicalHistoryViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            DataManager.EnsureLoaded();
            var patient = DataManager.GetCurrentPatient();
            if (patient != null)
            {
                PatientName = patient.FullName;
                var records = DataManager.GetPatientMedicalRecords(patient.Id).OrderByDescending(r => r.Date);
                foreach (var r in records) Records.Add(r);
            }

            // ADD SAMPLE DATA IF EMPTY
            if (Records.Count == 0)
            {
                Records.Add(new MedicalRecord {
                    Title = "General Health Screening", Date = System.DateTime.Now.AddDays(-15), Diagnosis = "Stable", Treatment = "Follow-up in 6 months", DoctorName = "Dr. Smith", FilePath = "ALPHA-SCR-2024.pdf"
                });
                Records.Add(new MedicalRecord {
                    Title = "Influenza Vaccination", Date = System.DateTime.Now.AddMonths(-2), Diagnosis = "Routine Immunization", Treatment = "Completed", DoctorName = "Nursing Staff", FilePath = "VAX-FLU-2024.pdf"
                });
                Records.Add(new MedicalRecord {
                    Title = "Clinical Consultation", Date = System.DateTime.Now.AddMonths(-5), Diagnosis = "Knee Discomfort", Treatment = "Physiotherapy recommended", DoctorName = "Dr. Adams", FilePath = "ALPHA-CON-2023-B.pdf"
                });
            }
        }
    }
}
