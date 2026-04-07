using System;
using System.Windows;
using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Services;

namespace ClinicAppointmentSystem.Views
{
    public partial class PatientEntryWindow : Window
    {
        public PatientEntryWindow()
        {
            InitializeComponent();
            DataContext = new ClinicAppointmentSystem.ViewModels.PatientEntryViewModel(this);
        }

        // Supporting Edit mode if needed later
        public PatientEntryWindow(Patient patient) : this()
        {
            var vm = DataContext as ClinicAppointmentSystem.ViewModels.PatientEntryViewModel;
            if (vm != null) vm.LoadPatient(patient);
        }
    }
}
