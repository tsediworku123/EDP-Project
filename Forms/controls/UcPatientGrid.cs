using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcPatientGrid : UserControl
    {
        private bool _isAdminMode = false;

        public bool IsAdminMode {
            get => _isAdminMode;
            set {
                _isAdminMode = value;
                UpdateModeUI();
            }
        }

        public UcPatientGrid()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 250, 252);
            RefreshData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) { RefreshData(); }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new RegisterPatientForm();
            form.ShowDialog();
            RefreshData();
        }

        private void dgvPatients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string col = dgvPatients.Columns[e.ColumnIndex].Name;
            int id = (int)dgvPatients.Rows[e.RowIndex].Cells["Id"].Value;
            if (col == "ActionEdit") EditPatient(id);
            else if (col == "ActionBook") BookPatient(id);
            else if (col == "ActionDelete") DeletePatient(id);
        }

        private void UpdateModeUI()
        {
            // Hide Add button for Admin as requested (Patients register themselves)
            if (btnAdd != null) btnAdd.Visible = !_isAdminMode;

            if (dgvPatients.Columns.Contains("ActionBook"))
                dgvPatients.Columns["ActionBook"].Visible = !_isAdminMode;

            // Strict Role Check for Delete
            if (dgvPatients.Columns.Contains("ActionDelete"))
            {
                var user = DataManager.CurrentUser;
                dgvPatients.Columns["ActionDelete"].Visible = (user?.Role == "Admin");
            }
        }

        public void RefreshData()
        {
            dgvPatients.Rows.Clear();
            string filter = txtSearch.Text.ToLower();
            
            var list = DataManager.Patients.Where(p => 
                string.IsNullOrEmpty(filter) || 
                p.FullName.ToLower().Contains(filter) || 
                (p.Phone != null && p.Phone.Contains(filter))
            );

            foreach (var p in list)
            {
                int age = DateTime.Today.Year - p.DateOfBirth.Year;
                string appt = DataManager.Appointments.Where(a => a.PatientId == p.Id).OrderByDescending(a => a.AppointmentDate).FirstOrDefault()?.AppointmentDate.ToString("yyyy-MM-dd") ?? "None";
                dgvPatients.Rows.Add(p.Id, p.FullName, p.Phone, $"{age}/{p.Gender.Substring(0,1)}", appt, p.IsActive ? "Active" : "Archived");
            }
        }

        private void EditPatient(int id) 
        { 
            var patient = DataManager.Patients.FirstOrDefault(p => p.Id == id);
            if (patient == null) return;

            Form dlg = new Form {
                Text = "Edit Patient Record",
                Size = new Size(460, 600), // Increased size
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                BackColor = Color.White,
                MaximizeBox = false
            };

            Panel dlgPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30) };
            dlg.Controls.Add(dlgPanel);
            int y = 10;

            TextBox txtName  = AddEditField(dlgPanel, "FULL NAME:", patient.FullName, ref y);
            TextBox txtPhone = AddEditField(dlgPanel, "PHONE NUMBER:", patient.Phone ?? "", ref y);
            TextBox txtAddr  = AddEditField(dlgPanel, "ADDRESS:", patient.Address ?? "", ref y);

            // Gender
            Label lblG = new Label { Text = "GENDER:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            ComboBox cmbGender = new ComboBox { Location = new Point(0, y + 22), Width = 380, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11) };
            cmbGender.Items.AddRange(new string[] { "Male", "Female", "Other" });
            cmbGender.SelectedItem = patient.Gender ?? "Male";
            dlgPanel.Controls.AddRange(new Control[] { lblG, cmbGender });
            y += 70;

            // DOB
            Label lblDob = new Label { Text = "DATE OF BIRTH:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            DateTimePicker dtp = new DateTimePicker { Location = new Point(0, y + 22), Width = 380, Format = DateTimePickerFormat.Short, Value = patient.DateOfBirth == default ? DateTime.Today.AddYears(-30) : patient.DateOfBirth };
            dlgPanel.Controls.AddRange(new Control[] { lblDob, dtp });
            y += 70;

            // Optional: Medical Notes / Allergies
            Label lblAllergies = new Label { Text = "ALLERGIES & NOTES:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            TextBox txtNotes = new TextBox { Location = new Point(0, y + 22), Width = 380, Height = 60, Multiline = true, Font = new Font("Segoe UI", 11), Text = patient.MedicalNotes ?? "" };
            dlgPanel.Controls.AddRange(new Control[] { lblAllergies, txtNotes });
            y += 90;

            Button btnSave = new Button {
                Text = "SAVE CHANGES",
                Location = new Point(0, y),
                Size = new Size(380, 45),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s2, e2) => {
                if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Name is required.", "Validation"); return; }
                patient.FullName = txtName.Text.Trim();
                patient.Phone = txtPhone.Text.Trim();
                patient.Address = txtAddr.Text.Trim();
                patient.Gender = cmbGender.SelectedItem?.ToString() ?? "Unknown";
                patient.DateOfBirth = dtp.Value;
                patient.MedicalNotes = txtNotes.Text.Trim();
                DataManager.SavePatients();
                dlg.DialogResult = DialogResult.OK;
            };
            dlgPanel.Controls.Add(btnSave);

            if (dlg.ShowDialog() == DialogResult.OK) {
                RefreshData();
                MessageBox.Show("Patient record updated successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private TextBox AddEditField(Panel p, string label, string value, ref int y)
        {
            Label lbl = new Label { Text = label, Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            TextBox txt = new TextBox { Location = new Point(0, y + 22), Width = 380, Font = new Font("Segoe UI", 11), Text = value };
            p.Controls.AddRange(new Control[] { lbl, txt });
            y += 70;
            return txt;
        }


        private void DeletePatient(int id)
        {
            var user = DataManager.CurrentUser;
            if (user?.Role != "Admin")
            {
                MessageBox.Show("Access Denied: Only Administrators can delete patient records.", "Security", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var patient = DataManager.Patients.FirstOrDefault(p => p.Id == id);
            if (patient != null)
            {
                var msg = $"Are you sure you want to PERMANENTLY DELETE '{patient.FullName}'?\nThis action cannot be undone.";
                if (MessageBox.Show(msg, "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DataManager.Patients.Remove(patient);
                    DataManager.SavePatients();
                    RefreshData();
                }
            }
        }
        private void BookPatient(int id) 
        { 
            var patient = DataManager.Patients.FirstOrDefault(p => p.Id == id);
            if (patient == null) return;
            
            if (this.ParentForm is AdminDashboard admin)
            {
                var bookingForm = new UcBookingForm();
                bookingForm.PreselectPatient(id);
                admin.ShowControl(bookingForm);
            }
        }
    }
}
