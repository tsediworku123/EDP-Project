using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class PatientForm : Form
    {
        private int editingId = -1;

        public PatientForm()
        {
            InitializeComponent();
            LoadPatients();
        }

        private void LoadPatients()
        {
            lvPatients.Items.Clear();
            foreach (var patient in DataManager.Patients)
            {
                ListViewItem item = new ListViewItem(patient.Id.ToString());
                item.SubItems.Add(patient.FullName);
                item.SubItems.Add(patient.Phone);
                item.SubItems.Add(patient.DateOfBirth.ToShortDateString());
                item.SubItems.Add(patient.Gender);
                item.SubItems.Add(patient.Address);
                item.Tag = patient;
                lvPatients.Items.Add(item);
            }
            UpdateStatus($"Total Patients: {DataManager.Patients.Count}");
        }

        private void UpdateStatus(string message)
        {
            if (lblStatus != null)
                lblStatus.Text = message;
        }

        private void ClearForm()
        {
            editingId = -1;
            txtFullName.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            dtpDOB.Value = DateTime.Now.AddYears(-20);
            rbtnMale.Checked = false;
            rbtnFemale.Checked = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void lvPatients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPatients.SelectedItems.Count > 0)
            {
                ListViewItem item = lvPatients.SelectedItems[0];
                editingId = int.Parse(item.SubItems[0].Text);
                txtFullName.Text = item.SubItems[1].Text;
                txtPhone.Text = item.SubItems[2].Text;
                dtpDOB.Value = DateTime.Parse(item.SubItems[3].Text);

                string gender = item.SubItems[4].Text;
                rbtnMale.Checked = (gender == "Male");
                rbtnFemale.Checked = (gender == "Female");

                txtAddress.Text = item.SubItems[5].Text;

                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            editingId = -1;
            txtFullName.Focus();
            UpdateStatus("Adding new patient...");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (editingId == -1)
            {
                MessageBox.Show("Please select a patient to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtFullName.Focus();
            UpdateStatus($"Editing patient ID: {editingId}");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Please enter full name!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter phone number!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            if (!long.TryParse(txtPhone.Text, out _))
            {
                MessageBox.Show("Phone must contain only numbers!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            if (!rbtnMale.Checked && !rbtnFemale.Checked)
            {
                MessageBox.Show("Please select gender!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (editingId == -1)
                {
                    Patient newPatient = new Patient
                    {
                        Id = DataManager.Patients.Count > 0 ? DataManager.Patients.Max(p => p.Id) + 1 : 1,
                        FullName = txtFullName.Text.Trim(),
                        Phone = txtPhone.Text.Trim(),
                        Address = txtAddress.Text.Trim(),
                        DateOfBirth = dtpDOB.Value,
                        Gender = rbtnMale.Checked ? "Male" : "Female"
                    };
                    DataManager.Patients.Add(newPatient);
                    UpdateStatus("Patient added successfully!");
                    MessageBox.Show("Patient added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var patient = DataManager.Patients.FirstOrDefault(p => p.Id == editingId);
                    if (patient != null)
                    {
                        patient.FullName = txtFullName.Text.Trim();
                        patient.Phone = txtPhone.Text.Trim();
                        patient.Address = txtAddress.Text.Trim();
                        patient.DateOfBirth = dtpDOB.Value;
                        patient.Gender = rbtnMale.Checked ? "Male" : "Female";
                        UpdateStatus($"Patient ID {editingId} updated successfully!");
                        MessageBox.Show("Patient updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                DataManager.SavePatients();
                LoadPatients();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving patient: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (editingId == -1)
            {
                MessageBox.Show("Please select a patient to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this patient?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var patient = DataManager.Patients.FirstOrDefault(p => p.Id == editingId);
                if (patient != null)
                {
                    DataManager.Patients.Remove(patient);
                    DataManager.SavePatients();
                    LoadPatients();
                    ClearForm();
                    UpdateStatus("Patient deleted successfully!");
                    MessageBox.Show("Patient deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadPatients();
                return;
            }

            lvPatients.Items.Clear();
            foreach (var patient in DataManager.Patients)
            {
                if (patient.FullName.ToLower().Contains(searchTerm) ||
                    patient.Phone.Contains(searchTerm))
                {
                    ListViewItem item = new ListViewItem(patient.Id.ToString());
                    item.SubItems.Add(patient.FullName);
                    item.SubItems.Add(patient.Phone);
                    item.SubItems.Add(patient.DateOfBirth.ToShortDateString());
                    item.SubItems.Add(patient.Gender);
                    item.SubItems.Add(patient.Address);
                    lvPatients.Items.Add(item);
                }
            }
            UpdateStatus($"Found {lvPatients.Items.Count} matching patients");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadPatients();
            ClearForm();
        }
    }
}