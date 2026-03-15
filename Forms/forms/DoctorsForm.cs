using ClinicAppointmentSystem.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ClinicAppointmentSystem
{
    public partial class DoctorsForm : Form
    {
        private int editingId = -1;

        public DoctorsForm()
        {
            InitializeComponent();
            LoadDoctors();
        }

        private void LoadDoctors()
        {
            dgvDoctors.Rows.Clear();
            foreach (var doctor in DataManager.Doctors)
            {
                dgvDoctors.Rows.Add(
                    doctor.Id,
                    doctor.FullName,
                    doctor.Specialization,
                    doctor.Gender,
                    doctor.Phone
                );
            }
            UpdateStatus($"Total Doctors: {DataManager.Doctors.Count}");
        }

        private void UpdateStatus(string message)
        {
            if (lblStatus != null)
                lblStatus.Text = message;
        }

        private void ClearFields()
        {
            editingId = -1;
            txtName.Text = "";
            txtSpecialization.Text = "";
            cmbGender.SelectedIndex = -1;
            txtPhone.Text = "";
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void dgvDoctors_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDoctors.SelectedRows.Count > 0)
            {
                var row = dgvDoctors.SelectedRows[0];
                editingId = Convert.ToInt32(row.Cells["colId"].Value);
                txtName.Text = row.Cells["colName"].Value?.ToString() ?? "";
                txtSpecialization.Text = row.Cells["colSpecialization"].Value?.ToString() ?? "";
                cmbGender.SelectedItem = row.Cells["colGender"].Value?.ToString();
                txtPhone.Text = row.Cells["colPhone"].Value?.ToString() ?? "";

                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearFields();
            txtName.Focus();
            UpdateStatus("Adding new doctor...");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (editingId == -1)
            {
                MessageBox.Show("Please select a doctor to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtName.Focus();
            UpdateStatus($"Editing doctor ID: {editingId}");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtSpecialization.Text) ||
                cmbGender.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please fill all fields!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (editingId != -1)
                {
                    var doctor = DataManager.Doctors.FirstOrDefault(d => d.Id == editingId);
                    if (doctor != null)
                    {
                        doctor.FullName = txtName.Text.Trim();
                        doctor.Specialization = txtSpecialization.Text.Trim();
                        doctor.Gender = cmbGender.SelectedItem.ToString();
                        doctor.Phone = txtPhone.Text.Trim();
                        UpdateStatus("Doctor updated successfully!");
                        MessageBox.Show("Doctor updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    var doctor = new Doctor
                    {
                        Id = DataManager.Doctors.Count > 0 ? DataManager.Doctors.Max(d => d.Id) + 1 : 1,
                        FullName = txtName.Text.Trim(),
                        Specialization = txtSpecialization.Text.Trim(),
                        Gender = cmbGender.SelectedItem.ToString(),
                        Phone = txtPhone.Text.Trim()
                    };
                    DataManager.Doctors.Add(doctor);
                    UpdateStatus("Doctor added successfully!");
                    MessageBox.Show("Doctor added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                DataManager.SaveDoctors();
                LoadDoctors();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (editingId == -1)
            {
                MessageBox.Show("Please select a doctor to delete!", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this doctor?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var doctor = DataManager.Doctors.FirstOrDefault(d => d.Id == editingId);
                if (doctor != null)
                {
                    DataManager.Doctors.Remove(doctor);
                    DataManager.SaveDoctors();
                    LoadDoctors();
                    ClearFields();
                    UpdateStatus("Doctor deleted successfully!");
                    MessageBox.Show("Doctor deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string term = txtSearch.Text.ToLower();
            if (string.IsNullOrEmpty(term))
            {
                LoadDoctors();
                return;
            }

            dgvDoctors.Rows.Clear();
            foreach (var d in DataManager.Doctors.Where(d =>
                d.FullName.ToLower().Contains(term) ||
                d.Specialization.ToLower().Contains(term)))
            {
                dgvDoctors.Rows.Add(d.Id, d.FullName, d.Specialization, d.Gender, d.Phone);
            }
            UpdateStatus($"Found {dgvDoctors.Rows.Count} matching doctors");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadDoctors();
            ClearFields();
        }
    }
}