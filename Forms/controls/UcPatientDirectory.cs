using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;
using System.Collections.Generic;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcPatientDirectory : UserControl
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;
        private string sortColumn = "Name";
        private bool sortAscending = true;
        private bool isUpdatingSort = false;

        public UcPatientDirectory()
        {
            InitializeComponent();
            SetupGridColumns();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                try { cmbPatientGender.SelectedIndex = 0; } catch { }
                try { cmbStatusFilter.SelectedIndex = 0; } catch { }
                try { cmbSortMethod.SelectedIndex = 0; } catch { }
                try { SendMessage(txtPatientSearch.Handle, EM_SETCUEBANNER, 0, "Search by name or phone..."); } catch { }
                try { RefreshPatients(); } catch { }
            }
        }

        private void SetupGridColumns()
        {
            patientsGrid.Columns.Clear();
            patientsGrid.Columns.Add("No", "#");
            patientsGrid.Columns.Add("ID", "Security ID");
            patientsGrid.Columns["ID"].Visible = false;
            patientsGrid.Columns.Add("Name", "Patient Full Name");
            patientsGrid.Columns.Add("Gender", "Gender");
            patientsGrid.Columns.Add("Phone", "Contact Number");
            patientsGrid.Columns.Add("LastVisit", "Recent Activity");
            patientsGrid.Columns.Add("Status", "Archive Status");

            patientsGrid.Columns[0].Width = 50;
            patientsGrid.Columns[2].Width = 250;
            patientsGrid.Columns[3].Width = 100;
            patientsGrid.Columns[4].Width = 180;
            patientsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            patientsGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            patientsGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            patientsGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 85, 105);
            patientsGrid.EnableHeadersVisualStyles = false;
            patientsGrid.RowTemplate.Height = 40;

            patientsGrid.CellFormatting += (s, e) => {
                if (patientsGrid.Columns[e.ColumnIndex].Name == "Status" && e.Value != null) {
                    string v = e.Value.ToString();
                    e.CellStyle.ForeColor = v == "Active" ? Color.FromArgb(16, 185, 129) : Color.FromArgb(100, 116, 139);
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
            };

            patientsGrid.ColumnHeaderMouseClick -= PatientsGrid_ColumnHeaderMouseClick;
            patientsGrid.ColumnHeaderMouseClick += PatientsGrid_ColumnHeaderMouseClick;
        }

        private void PatientsGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string col = patientsGrid.Columns[e.ColumnIndex].Name;
            if (col == "No" || col == "ID") return;
            if (sortColumn == col) sortAscending = !sortAscending;
            else { sortColumn = col; sortAscending = true; }
            RefreshPatients();
        }

        private void cmbSortMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingSort || !this.IsHandleCreated) return;
            switch (cmbSortMethod.SelectedIndex) {
                case 0: sortColumn = "Name"; sortAscending = true; break;
                case 1: sortColumn = "Name"; sortAscending = false; break;
                case 2: sortColumn = "Gender"; sortAscending = true; break;
                case 3: sortColumn = "LastVisit"; sortAscending = false; break;
                case 4: sortColumn = "Status"; sortAscending = false; break;
                case 5: sortColumn = "Status"; sortAscending = true; break;
            }
            RefreshPatients();
        }

        private void RefreshPatients() => FilterPatients(null, null);

        private void FilterPatients(object sender, EventArgs e)
        {
            if (patientsGrid == null || !this.IsHandleCreated) return;
            string gender = cmbPatientGender.SelectedItem?.ToString() ?? "All Genders";
            string search = txtPatientSearch.Text.ToLower();

            var query = DataManager.Patients.AsEnumerable();

            if (gender != "All Genders")
                query = query.Where(p => p.Gender == gender);
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => 
                    p.FullName.ToLower().Contains(search) || 
                    (p.Phone != null && p.Phone.Contains(search)) ||
                    p.Id.ToString() == search
                );
            }
            if (cmbStatusFilter != null && cmbStatusFilter.SelectedIndex > 0)
            {
                bool isActive = cmbStatusFilter.SelectedIndex == 1;
                query = query.Where(p => p.IsActive == isActive);
            }

            if (sortColumn == "Name") query = sortAscending ? query.OrderBy(p => p.FullName) : query.OrderByDescending(p => p.FullName);
            else if (sortColumn == "Gender") query = sortAscending ? query.OrderBy(p => p.Gender) : query.OrderByDescending(p => p.Gender);
            else if (sortColumn == "Status") query = sortAscending ? query.OrderBy(p => p.IsActive) : query.OrderByDescending(p => p.IsActive);

            var filtered = query.ToList();

            patientsGrid.Rows.Clear();
            int count = 1;
            foreach (var p in filtered)
            {
                var lastAppt = DataManager.Appointments.Where(a => a.PatientId == p.Id).OrderByDescending(a => a.AppointmentDate).FirstOrDefault();
                string lastAct = lastAppt != null ? lastAppt.AppointmentDate.ToString("yyyy-MM-dd") : "---";
                patientsGrid.Rows.Add(count++, p.Id, p.FullName, p.Gender, p.Phone ?? "---", lastAct, p.IsActive ? "Active" : "Archived");
            }

            foreach (DataGridViewColumn col in patientsGrid.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            if (patientsGrid.Columns[sortColumn] != null)
                patientsGrid.Columns[sortColumn].HeaderCell.SortGlyphDirection = sortAscending ? SortOrder.Ascending : SortOrder.Descending;

            if (cmbSortMethod != null) {
                isUpdatingSort = true;
                if (sortColumn == "Name") cmbSortMethod.SelectedIndex = sortAscending ? 0 : 1;
                else if (sortColumn == "Gender") cmbSortMethod.SelectedIndex = 2;
                else if (sortColumn == "Status") cmbSortMethod.SelectedIndex = sortAscending ? 5 : 4;
                isUpdatingSort = false;
            }

            lblResults.Text = $"Showing {filtered.Count} of {DataManager.Patients.Count} clinical records.";
        }

        private void btnTogglePatient_Click(object sender, EventArgs e)
        {
            if (patientsGrid.SelectedRows.Count == 0) return;
            int id = (int)patientsGrid.SelectedRows[0].Cells["ID"].Value;
            var patient = DataManager.Patients.FirstOrDefault(p => p.Id == id);
            if (patient != null) {
                patient.IsActive = !patient.IsActive;
                DataManager.LogAudit(DataManager.CurrentUser?.Username, $"{(patient.IsActive ? "Activated" : "Archived")} Patient {patient.FullName}");
                RefreshPatients();
            }
        }

        private void btnDeletePatient_Click(object sender, EventArgs e)
        {
            if (patientsGrid.SelectedRows.Count == 0) return;
            int id = (int)patientsGrid.SelectedRows[0].Cells["ID"].Value;
            var patient = DataManager.Patients.FirstOrDefault(p => p.Id == id);
            if (patient != null) {
                if (MessageBox.Show($"WARNING: Permanently delete patient '{patient.FullName}'?\n\nAll associated appointments will also be removed.", "Critical Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    DataManager.Appointments.RemoveAll(a => a.PatientId == patient.Id);
                    DataManager.Patients.Remove(patient);
                    DataManager.LogAudit(DataManager.CurrentUser?.Username, "Deleted patient record: " + patient.FullName);
                    DataManager.SaveAllData();
                    RefreshPatients();
                }
            }
        }

        private void btnEditPatient_Click(object sender, EventArgs e)
        {
            if (patientsGrid.SelectedRows.Count == 0) return;
            int id = (int)patientsGrid.SelectedRows[0].Cells["ID"].Value;
            var patient = DataManager.Patients.FirstOrDefault(p => p.Id == id);
            if (patient == null) return;

            Form dlg = new Form {
                Text = "Edit Patient Record",
                Size = new Size(460, 600),
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

            Label lblG = new Label { Text = "GENDER:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            ComboBox cmbGender = new ComboBox { Location = new Point(0, y + 22), Width = 380, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11) };
            cmbGender.Items.AddRange(new string[] { "Male", "Female", "Other" });
            cmbGender.SelectedItem = patient.Gender ?? "Male";
            dlgPanel.Controls.AddRange(new Control[] { lblG, cmbGender });
            y += 70;

            Label lblDob = new Label { Text = "DATE OF BIRTH:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            DateTimePicker dtp = new DateTimePicker { Location = new Point(0, y + 22), Width = 380, Format = DateTimePickerFormat.Short, Value = patient.DateOfBirth == default ? DateTime.Today.AddYears(-30) : patient.DateOfBirth };
            dlgPanel.Controls.AddRange(new Control[] { lblDob, dtp });
            y += 70;

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
                RefreshPatients();
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

        private void btnDetectDups_Click(object sender, EventArgs e)
        {
            var duplicates = DataManager.Patients
                .GroupBy(p => p.Phone)
                .Where(g => !string.IsNullOrEmpty(g.Key) && g.Count() > 1)
                .Select(g => new { Phone = g.Key, Count = g.Count(), Names = string.Join(", ", g.Select(p => p.FullName)) })
                .ToList();

            if (duplicates.Count == 0)
            {
                MessageBox.Show("No patients with duplicate phone numbers were detected.", "Duplicate Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string msg = "The following potential duplicates were found:\n\n" + 
                             string.Join("\n", duplicates.Select(d => $"Phone {d.Phone}: {d.Names} ({d.Count} records)"));
                MessageBox.Show(msg, "Potential Duplicates Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnBookAppt_Click(object sender, EventArgs e)
        {
            if (patientsGrid.SelectedRows.Count == 0) return;
            int id = (int)patientsGrid.SelectedRows[0].Cells["ID"].Value;
            
            // Navigate to Booking Form via Parent Dashboard
            Control parent = this.Parent;
            while (parent != null && !(parent is AdminDashboard)) parent = parent.Parent;

            if (parent is AdminDashboard admin)
            {
                var bookingForm = new UcBookingForm();
                bookingForm.PreselectPatient(id);
                admin.ShowControl(bookingForm);
            }
        }
    }
}
