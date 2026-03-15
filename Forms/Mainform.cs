using ClinicPatientSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicPatientSystem
{
    public partial class Mainform : Form
    {
        // Temporary storage for patients (NO database yet)
        public static List<Patient> patients = new List<Patient>();

        public Mainform()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            AddPatientForm form = new AddPatientForm();
            form.ShowDialog();
        }

        private void btnViewPatients_Click(object sender, EventArgs e)
        {
            PatientListForm form = new PatientListForm();
            form.ShowDialog();
        }

        private void btnSearchPatient_Click(object sender, EventArgs e)
        {
            SearchPatientForm searchForm = new SearchPatientForm();
            searchForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
