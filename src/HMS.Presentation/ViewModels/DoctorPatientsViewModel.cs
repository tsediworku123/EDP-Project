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
    public class DoctorPatientsViewModel : ObservableObject
    {
        public ObservableCollection<Patient> Patients { get; } = new ObservableCollection<Patient>();

        public ICommand ViewPatientDetailsCommand { get; }

        public DoctorPatientsViewModel()
        {
            DataManager.EnsureLoaded();
            
            // Just displaying the first set of patients for simplicity in this view
            var patientsList = DataManager.Patients.Take(10).ToList();
            foreach (var patient in patientsList)
            {
                Patients.Add(patient);
            }

            ViewPatientDetailsCommand = new RelayCommand<Patient>((patient) => 
            {
                if (patient != null)
                {
                    int age = (int)((DateTime.Now - patient.DateOfBirth).TotalDays / 365.25);
                    string diagnosisDetails = $"Patient ID: {patient.PatientCode}\n" +
                                              $"Full Name: {patient.FullName}\n" +
                                              $"Age: {age}\n" +
                                              $"Gender: {patient.Gender}\n" +
                                              $"Blood Group: {patient.BloodGroup}\n" + 
                                              $"Phone: {patient.Phone}\n" +
                                              $"Email: {patient.Email}\n" +
                                              $"Address: {patient.Address}\n" +
                                              $"--------------------------------\n" +
                                              $"Allergies/Conditions: {patient.AllergiesOrChronicConditions ?? "None"}\n" +
                                              $"Current Meds: {patient.CurrentMedications ?? "None"}";

                    MessageBox.Show(diagnosisDetails, $"Patient File - {patient.FullName}", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });
        }
    }
}
