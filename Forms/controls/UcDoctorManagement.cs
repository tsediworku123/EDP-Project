using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcDoctorManagement : UserControl
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;
        private string sortColumn = "Name";
        private bool sortAscending = true;
        private bool isUpdatingSort = false;

        public UcDoctorManagement()
        {
            InitializeComponent();

            // Ensure the register button is on top and visible
            btnAddDoctor.BringToFront();
            if (btnAddDoctor.Location.X > this.Width - 300) btnAddDoctor.Location = new Point(this.Width - 300, 15);
            SetupGridColumns();
            LoadSpecialties();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime) {
                try { SendMessage(txtDoctorSearch.Handle, EM_SETCUEBANNER, 0, "Search by name or specialty..."); } catch { }
                try { RefreshDoctors(); } catch { }
            }


        }

        private void SetupGridColumns()
        {
            doctorsGrid.Columns.Clear();
            doctorsGrid.Columns.Add("No", "#");
            doctorsGrid.Columns.Add("ID", "Security ID");
            doctorsGrid.Columns["ID"].Visible = false;
            doctorsGrid.Columns.Add("Name", "Physician Name");
            doctorsGrid.Columns.Add("Spec", "Medical Specialty");
            doctorsGrid.Columns.Add("Email", "Professional Email");
            doctorsGrid.Columns.Add("Phone", "Emergency Contact");
            doctorsGrid.Columns.Add("Status", "Operational Status");

            doctorsGrid.Columns["No"].Width = 60;
            doctorsGrid.Columns["Name"].FillWeight = 110;
            doctorsGrid.Columns["Spec"].FillWeight = 90;
            doctorsGrid.Columns["Email"].FillWeight = 100;
            doctorsGrid.Columns["Status"].Width = 120;

            doctorsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            doctorsGrid.ColumnHeadersHeight = 45;
            doctorsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            doctorsGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            doctorsGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            doctorsGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 85, 105);
            doctorsGrid.EnableHeadersVisualStyles = false;
            doctorsGrid.RowTemplate.Height = 40;
            
            doctorsGrid.CellFormatting += (s, e) => {
                if (doctorsGrid.Columns[e.ColumnIndex].Name == "Status" && e.Value != null) {
                    string v = e.Value.ToString();
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
            };
            doctorsGrid.ColumnHeaderMouseClick -= DoctorsGrid_ColumnHeaderMouseClick;
            doctorsGrid.ColumnHeaderMouseClick += DoctorsGrid_ColumnHeaderMouseClick;
        }

        private void LoadSpecialties()
        {
            cmbDoctorSpec.Items.Clear();
            cmbDoctorSpec.Items.Add("All Specialties");
            var specs = DataManager.Doctors.Select(d => d.Specialty).Distinct().ToList();
            foreach (var s in specs) cmbDoctorSpec.Items.Add(s);
            cmbDoctorSpec.SelectedIndex = 0;
        }

        private void RefreshDoctors() => FilterDoctors(null, null);

        private void FilterDoctors(object sender, EventArgs e)
        {
            if (doctorsGrid == null || !this.IsHandleCreated) return;
            string spec = cmbDoctorSpec.SelectedItem?.ToString() ?? "All Specialties";
            string search = txtDoctorSearch.Text.ToLower();

            var query = DataManager.Doctors.AsEnumerable();

            if (spec != "All Specialties") {
                query = query.Where(d => d.Specialty == spec);
            }
            if (!string.IsNullOrEmpty(search)) {
                query = query.Where(d => d.FullName.ToLower().Contains(search) || d.Specialty.ToLower().Contains(search));
            }
            if (cmbStatusFilter != null && cmbStatusFilter.SelectedIndex > 0) {
                bool isActive = cmbStatusFilter.SelectedIndex == 1; // 1: Active, 2: Leave
                query = query.Where(d => d.IsActive == isActive);
            }

            if (sortColumn == "Name") query = sortAscending ? query.OrderBy(d => d.FullName) : query.OrderByDescending(d => d.FullName);
            else if (sortColumn == "Spec") query = sortAscending ? query.OrderBy(d => d.Specialty) : query.OrderByDescending(d => d.Specialty);
            else if (sortColumn == "Status") query = sortAscending ? query.OrderBy(d => d.IsActive) : query.OrderByDescending(d => d.IsActive);

            var filtered = query.ToList();

            doctorsGrid.Rows.Clear();
            int count = 1;
            foreach (var d in filtered)
            {
                doctorsGrid.Rows.Add(count++, d.Id, d.FullName, d.Specialty, d.Email ?? "---", d.Phone ?? "---", d.IsActive ? "Active Duty" : "On Leave");
            }
            
            foreach (DataGridViewColumn col in doctorsGrid.Columns) {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            if (doctorsGrid.Columns[sortColumn] != null) {
                doctorsGrid.Columns[sortColumn].HeaderCell.SortGlyphDirection = sortAscending ? SortOrder.Ascending : SortOrder.Descending;
            }

            if (cmbSortMethod != null) {
                isUpdatingSort = true;
                if (sortColumn == "Name") cmbSortMethod.SelectedIndex = sortAscending ? 0 : 1;
                else if (sortColumn == "Spec") cmbSortMethod.SelectedIndex = sortAscending ? 2 : 3;
                else if (sortColumn == "Status") cmbSortMethod.SelectedIndex = sortAscending ? 5 : 4;
                isUpdatingSort = false;
            }

            lblResults.Text = $"Showing {filtered.Count} of {DataManager.Doctors.Count} total medical staff members.";
        }

        private void btnAddDoctor_Click(object sender, EventArgs e)
        {
            Form dlg = new Form {
                Text = "Register New Physician",
                Size = new Size(540, 850),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                BackColor = Color.White,
                MaximizeBox = false,
                AutoScroll = true
            };

            Panel p = new Panel { AutoScroll = true, Dock = DockStyle.Fill, Padding = new Padding(25, 15, 25, 40) };
            dlg.Controls.Add(p);
            int y = 5;
            int fw = 430;

            // ── DOCTOR PROFILE SECTION ──────────────────────────────────
            AddSectionLabel(p, "DOCTOR PROFILE", ref y);
            TextBox txtName = AddField(p, "Full Name *", ref y, fw);

            // Gender
            Label lGender = new Label { Text = "GENDER *", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            ComboBox cmbGender = new ComboBox { Location = new Point(0, y + 20), Width = fw, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbGender.Items.AddRange(new[] { "Male", "Female", "Other" });
            cmbGender.SelectedIndex = 0;
            p.Controls.AddRange(new Control[] { lGender, cmbGender });
            y += 55;

            // Date of Birth
            Label lDob = new Label { Text = "DATE OF BIRTH *", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            DateTimePicker dtpDob = new DateTimePicker { Location = new Point(0, y + 20), Width = fw, Font = new Font("Segoe UI", 10), Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddYears(-30) };
            p.Controls.AddRange(new Control[] { lDob, dtpDob });
            y += 55;

            TextBox txtAddress = AddField(p, "Home Address", ref y, fw);
            
            // Specialty dropdown
            Label lSpec = new Label { Text = "SPECIALTY *", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            ComboBox cmbSpec = new ComboBox { Location = new Point(0, y + 20), Width = fw, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbSpec.Items.AddRange(new[] { "General Medicine", "Pediatrics", "Gynecology", "Cardiology", "Dermatology", "Orthopedics", "ENT", "Ophthalmology", "Dentistry", "Other" });
            cmbSpec.SelectedIndex = 0;
            p.Controls.AddRange(new Control[] { lSpec, cmbSpec });
            y += 55;

            TextBox txtPhone = AddField(p, "Phone Number", ref y, fw);
            txtPhone.MaxLength = 10;
            txtPhone.KeyPress += (s, ke) => {
                if (!char.IsControl(ke.KeyChar) && !char.IsDigit(ke.KeyChar)) ke.Handled = true;
            };

            // Assigned Shift
            Label lShift = new Label { Text = "ASSIGNED SHIFT (24/7)", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            ComboBox cmbShift = new ComboBox { Location = new Point(0, y + 20), Width = fw, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbShift.Items.AddRange(new[] { "Morning (08:00 - 16:00, Break 12-1)", "Afternoon (16:00 - 00:00, Break 20-21)", "Night (00:00 - 08:00, Break 04-05)" });
            cmbShift.SelectedIndex = 0;
            p.Controls.AddRange(new Control[] { lShift, cmbShift });
            y += 55;

            // Working Days checkboxes
            Label lDays = new Label { Text = "WORKING DAYS", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            p.Controls.Add(lDays); y += 20;
            string[] dayNames = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            CheckBox[] dayChecks = new CheckBox[7];
            for (int i = 0; i < 7; i++) {
                dayChecks[i] = new CheckBox { Text = dayNames[i], Location = new Point(i * 60, y), AutoSize = true, Checked = i < 5, Font = new Font("Segoe UI", 8) };
                p.Controls.Add(dayChecks[i]);
            }
            y += 30;

            // Slot + Buffer
            Label lSlot = new Label { Text = "SLOT DURATION (MIN)", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            NumericUpDown nSlot = new NumericUpDown { Location = new Point(0, y + 20), Width = 100, Minimum = 5, Maximum = 60, Value = 20, Font = new Font("Segoe UI", 10) };
            Label lBuf = new Label { Text = "BUFFER TIME (MIN)", Location = new Point(220, y), AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            NumericUpDown nBuf = new NumericUpDown { Location = new Point(220, y + 20), Width = 100, Minimum = 0, Maximum = 30, Value = 5, Font = new Font("Segoe UI", 10) };
            p.Controls.AddRange(new Control[] { lSlot, nSlot, lBuf, nBuf });
            y += 55;

            // ── LOGIN ACCOUNT SECTION ───────────────────────────────────
            AddSectionLabel(p, "LOGIN ACCOUNT (Temporary)", ref y);
            TextBox txtUser = AddField(p, "Username *", ref y, fw);
            
            // Password with Generate button
            Label lPass = new Label { Text = "PASSWORD *", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            TextBox txtPass = new TextBox { Location = new Point(0, y + 20), Width = fw - 110, Font = new Font("Segoe UI", 10), PasswordChar = '*' };
            Button btnGen = new Button { Text = "GENERATE", Location = new Point(fw - 100, y + 19), Size = new Size(100, 26), BackColor = Color.FromArgb(241, 245, 249), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 8, FontStyle.Bold) };
            btnGen.Click += (s2, e2) => {
                string pStr = new Random().Next(1000, 9999).ToString() + "!" + (char)('A' + new Random().Next(26));
                txtPass.Text = pStr;
                txtPass.PasswordChar = '\0'; // Show temp pass briefly
                MessageBox.Show("Temporary Password Generated: " + pStr + "\n\nPlease provide this to the physician for their first login.", "Security");
                txtPass.PasswordChar = '*';
            };
            p.Controls.AddRange(new Control[] { lPass, txtPass, btnGen });
            y += 55;

            TextBox txtConf = AddField(p, "Confirm Password *", ref y, fw); txtConf.PasswordChar = '*';
            TextBox txtEmail = AddField(p, "Email (optional)", ref y, fw);

            // ── SAVE BUTTON ─────────────────────────────────────────────
            Button btnSave = new Button {
                Text = "SAVE DOCTOR",
                Location = new Point(0, y + 5),
                Size = new Size(fw, 48),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s2, e2) => {
                // Validation
                if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Full Name is required.", "Validation"); return; }
                if (txtPhone.Text.Length > 0 && txtPhone.Text.Length != 10) { MessageBox.Show("Phone number must be exactly 10 digits.", "Validation"); return; }
                if (string.IsNullOrWhiteSpace(txtUser.Text) || txtUser.Text.Length < 3) { MessageBox.Show("Username is required (min 3 chars).", "Validation"); return; }
                if (string.IsNullOrWhiteSpace(txtPass.Text) || txtPass.Text.Length < 3) { MessageBox.Show("Password is required (min 3 chars).", "Validation"); return; }
                if (txtPass.Text != txtConf.Text) { MessageBox.Show("Passwords do not match.", "Validation"); return; }
                if (DataManager.Users.Any(u => u.Username.Equals(txtUser.Text, StringComparison.OrdinalIgnoreCase))) {
                    MessageBox.Show("Username already exists.", "Validation"); return;
                }

                // Build working days string
                var days = new System.Collections.Generic.List<string>();
                for (int i = 0; i < 7; i++) if (dayChecks[i].Checked) days.Add(dayNames[i]);

                // Add Doctor
                int newId = DataManager.Doctors.Count > 0 ? DataManager.Doctors.Max(d => d.Id) + 1 : 1;
                
                string selShift = cmbShift.SelectedItem.ToString();
                TimeSpan wStart, wEnd, bStart, bEnd;
                string shiftName = "Morning";
                if(selShift.StartsWith("Morning")) {
                    shiftName = "Morning"; wStart = new TimeSpan(8,0,0); wEnd = new TimeSpan(16,0,0); bStart = new TimeSpan(12,0,0); bEnd = new TimeSpan(13,0,0);
                } else if(selShift.StartsWith("Afternoon")) {
                    shiftName = "Afternoon"; wStart = new TimeSpan(16,0,0); wEnd = new TimeSpan(23,59,59); bStart = new TimeSpan(20,0,0); bEnd = new TimeSpan(21,0,0);
                } else {
                    shiftName = "Night"; wStart = new TimeSpan(0,0,0); wEnd = new TimeSpan(8,0,0); bStart = new TimeSpan(4,0,0); bEnd = new TimeSpan(5,0,0);
                }

                DataManager.Doctors.Add(new Doctor {
                    Id = newId,
                    FullName = txtName.Text.Trim(),
                    Specialty = cmbSpec.SelectedItem.ToString(),
                    Gender = cmbGender.SelectedItem.ToString(),
                    DateOfBirth = dtpDob.Value,
                    Address = txtAddress.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    SlotDurationMinutes = (int)nSlot.Value,
                    BufferMinutes = (int)nBuf.Value,
                    IsActive = true,
                    WorkingDays = string.Join(", ", days),
                    AssignedShift = shiftName,
                    WorkingHoursStart = wStart,
                    WorkingHoursEnd = wEnd,
                    BreakTimeStart = bStart,
                    BreakTimeEnd = bEnd
                });

                // Add Login Account
                int userId = DataManager.Users.Count > 0 ? DataManager.Users.Max(u => u.Id) + 1 : 1;
                DataManager.Users.Add(new User {
                    Id = userId,
                    Username = txtUser.Text.Trim(),
                    Password = Utils.PasswordHasher.HashPassword(txtPass.Text),
                    Email = txtEmail.Text.Trim(),
                    Role = "Doctor",
                    DoctorId = newId, // Critical: Missing Link!
                    IsActive = true
                });

                DataManager.SaveAllData();
                DataManager.LogAudit(DataManager.CurrentUser?.Username, "Registered physician + account: " + txtName.Text);
                dlg.DialogResult = DialogResult.OK;
            };
            p.Controls.Add(btnSave);
            
            // Bottom Spacer for scroll area
            p.Controls.Add(new Label { Text = "", Location = new Point(0, y + 60), Height = 50 });

            if (dlg.ShowDialog() == DialogResult.OK) {
                LoadSpecialties();
                RefreshDoctors();
                MessageBox.Show("Doctor registered and login account created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AddSectionLabel(Panel p, string text, ref int y)
        {
            Panel line = new Panel { Location = new Point(0, y), Size = new Size(430, 1), BackColor = Color.FromArgb(226, 232, 240) };
            Label lbl = new Label { Text = text, Location = new Point(0, y + 5), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(59, 130, 246) };
            p.Controls.AddRange(new Control[] { line, lbl });
            y += 30;
        }

        private TextBox AddField(Panel p, string label, ref int y, int width = 380)
        {
            Label lbl = new Label { Text = label.ToUpper(), Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            TextBox txt = new TextBox { Location = new Point(0, y + 20), Width = width, Font = new Font("Segoe UI", 10) };
            p.Controls.AddRange(new Control[] { lbl, txt });
            y += 52;
            return txt;
        }


        private void btnEditDoctor_Click(object sender, EventArgs e)
        {
            if (doctorsGrid.SelectedRows.Count == 0) return;
            int id = (int)doctorsGrid.SelectedRows[0].Cells["ID"].Value;
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == id);
            if (doc != null) MessageBox.Show("Administrative Shift Manager opened for " + doc.FullName, "Clinical Operations", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnToggleDoctor_Click(object sender, EventArgs e)
        {
            if (doctorsGrid.SelectedRows.Count == 0) return;
            int id = (int)doctorsGrid.SelectedRows[0].Cells["ID"].Value;
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == id);
            if (doc != null) {
                doc.IsActive = !doc.IsActive;
                DataManager.LogAudit(DataManager.CurrentUser?.Username, $"{(doc.IsActive ? "Activated" : "Deactivated")} Physician {doc.FullName}");
                RefreshDoctors();
            }
        }

        // Legacy WinForms Create dialog removed in favor of WPF AddNewDoctorWindow

        private void cmbSortMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingSort || !this.IsHandleCreated) return;
            switch (cmbSortMethod.SelectedIndex) {
                case 0: sortColumn = "Name"; sortAscending = true; break;
                case 1: sortColumn = "Name"; sortAscending = false; break;
                case 2: sortColumn = "Spec"; sortAscending = true; break;
                case 3: sortColumn = "Spec"; sortAscending = false; break;
                case 4: sortColumn = "Status"; sortAscending = false; break; 
                case 5: sortColumn = "Status"; sortAscending = true; break;  
            }
            RefreshDoctors();
        }

        private void DoctorsGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = doctorsGrid.Columns[e.ColumnIndex].Name;
            if (columnName == "No" || columnName == "ID") return;

            if (sortColumn == columnName) sortAscending = !sortAscending;
            else { sortColumn = columnName; sortAscending = true; }
            RefreshDoctors();
        }

        private void btnDeleteDoctor_Click(object sender, EventArgs e)
        {
            if (doctorsGrid.SelectedRows.Count == 0) return;
            int id = (int)doctorsGrid.SelectedRows[0].Cells["ID"].Value;
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == id);
            
            if (doc != null) {
                if (MessageBox.Show($"WARNING: Are you sure you want to permanently delete physician '{doc.FullName}'?\n\nThis will also revoke their clinical login credentials.", "Critical Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    DataManager.Doctors.Remove(doc);
                    var docUser = DataManager.Users.FirstOrDefault(u => u.DoctorId == doc.Id);
                    if (docUser != null) DataManager.Users.Remove(docUser);
                    
                    DataManager.LogAudit(DataManager.CurrentUser?.Username, "Deleted Physician and linked User account: " + doc.FullName);
                    DataManager.SaveAllData();
                    LoadSpecialties();
                    RefreshDoctors();
                }
            }
        }
    }
}
