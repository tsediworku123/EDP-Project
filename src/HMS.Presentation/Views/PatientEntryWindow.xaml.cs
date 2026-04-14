using System;
using System.Windows;
using HMS.Core.Domain.Entities;
using HMS.Core.Infrastructure.Repositories.Json;

namespace HMS.Core.Views
{
    public partial class PatientEntryWindow : Window
    {
        public PatientEntryWindow()
        {
            InitializeComponent();
            DataContext = new HMS.Core.ViewModels.PatientEntryViewModel(this);
        }

        // Supporting Edit mode if needed later
        public PatientEntryWindow(Patient patient) : this()
        {
            var vm = DataContext as HMS.Core.ViewModels.PatientEntryViewModel;
            if (vm != null) vm.LoadPatient(patient);
        }
    }
}
