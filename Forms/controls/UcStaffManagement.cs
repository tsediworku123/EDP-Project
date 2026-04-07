using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcStaffManagement : UserControl
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;
        private string sortColumn = "Username";
        private bool sortAscending = true;
        private bool isUpdatingSort = false;

        public UcStaffManagement()
        {
            InitializeComponent();
            SetupGridColumns();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                try { SendMessage(txtSearch.Handle, EM_SETCUEBANNER, 0, "Search by username, role, or email..."); } catch { }
                try { RefreshStaffGrid(); } catch { }
            }
        }

        private void SetupGridColumns()
        {
            staffGrid.Columns.Clear();
            staffGrid.Columns.Add("No", "#");
            staffGrid.Columns.Add("ID", "Security ID");
            staffGrid.Columns["ID"].Visible = false;
            staffGrid.Columns.Add("Username", "Official Username");
            staffGrid.Columns.Add("Role", "System Role");
            staffGrid.Columns.Add("Email", "Staff Email");
            staffGrid.Columns.Add("Status", "Security Status");

            staffGrid.Columns[0].Width = 50;
            staffGrid.Columns[1].Visible = false;
            staffGrid.Columns[2].Width = 200;
            staffGrid.Columns[3].Width = 150;
            staffGrid.Columns[4].Width = 250;
            staffGrid.Columns[5].Width = 150;
            staffGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            staffGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            staffGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            staffGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 85, 105);
            staffGrid.EnableHeadersVisualStyles = false;
            staffGrid.RowTemplate.Height = 40;
            staffGrid.ColumnHeaderMouseClick -= StaffGrid_ColumnHeaderMouseClick;
            staffGrid.ColumnHeaderMouseClick += StaffGrid_ColumnHeaderMouseClick;
        }

        private void StaffGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = staffGrid.Columns[e.ColumnIndex].Name;
            if (columnName == "No" || columnName == "ID") return;

            if (sortColumn == columnName) {
                sortAscending = !sortAscending;
            } else {
                sortColumn = columnName;
                sortAscending = true;
            }
            RefreshStaffGrid(txtSearch.Text);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshStaffGrid(txtSearch.Text);
        }

        private void cmbRoleFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.IsHandleCreated) {
                RefreshStaffGrid(txtSearch.Text);
            }
        }

        private void cmbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.IsHandleCreated) {
                RefreshStaffGrid(txtSearch.Text);
            }
        }

        private void cmbSortMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingSort || !this.IsHandleCreated) return;

            switch (cmbSortMethod.SelectedIndex)
            {
                case 0: sortColumn = "Username"; sortAscending = true; break;
                case 1: sortColumn = "Username"; sortAscending = false; break;
                case 2: sortColumn = "Role"; sortAscending = true; break;
                case 3: sortColumn = "Role"; sortAscending = false; break;
                case 4: sortColumn = "Status"; sortAscending = false; break; // Active first (true)
                case 5: sortColumn = "Status"; sortAscending = true; break;  // Locked first (false)
            }
            RefreshStaffGrid(txtSearch.Text);
        }

        public void RefreshStaffGrid(string filter = "")
        {
            staffGrid.Rows.Clear();
            int count = 1;
            
            var query = DataManager.Users.Where(u => u.Role != "Patient").AsEnumerable();
            if (!string.IsNullOrEmpty(filter)) {
                string f = filter.ToLower();
                query = query.Where(u => u.Username.ToLower().Contains(f) || 
                                       u.Role.ToLower().Contains(f) || 
                                       (u.Email != null && u.Email.ToLower().Contains(f)));
            }

            if (cmbRoleFilter != null && cmbRoleFilter.SelectedIndex > 0) {
                string roleFilter = cmbRoleFilter.SelectedItem.ToString();
                query = query.Where(u => u.Role.Equals(roleFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (cmbStatusFilter != null && cmbStatusFilter.SelectedIndex > 0) {
                bool isStatusActive = cmbStatusFilter.SelectedIndex == 1; // 1: Active, 2: Locked
                query = query.Where(u => u.IsActive == isStatusActive);
            }

            if (sortColumn == "Username")
            {
                query = sortAscending ? query.OrderBy(u => u.Username) : query.OrderByDescending(u => u.Username);
            }
            else if (sortColumn == "Role")
            {
                query = sortAscending ? query.OrderBy(u => u.Role) : query.OrderByDescending(u => u.Role);
            }
            else if (sortColumn == "Email")
            {
                query = sortAscending ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email);
            }
            else if (sortColumn == "Status")
            {
                query = sortAscending ? query.OrderBy(u => u.IsActive) : query.OrderByDescending(u => u.IsActive);
            }

            foreach (var user in query)
            {
                staffGrid.Rows.Add(count++, user.Id, user.Username, user.Role, user.Email ?? "---", user.IsActive ? "Active" : "Locked");
            }

            foreach (DataGridViewColumn col in staffGrid.Columns)
            {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            if (staffGrid.Columns[sortColumn] != null)
            {
                staffGrid.Columns[sortColumn].HeaderCell.SortGlyphDirection = sortAscending ? SortOrder.Ascending : SortOrder.Descending;
            }

            if (cmbSortMethod != null)
            {
                isUpdatingSort = true;
                if (sortColumn == "Username") cmbSortMethod.SelectedIndex = sortAscending ? 0 : 1;
                else if (sortColumn == "Role") cmbSortMethod.SelectedIndex = sortAscending ? 2 : 3;
                else if (sortColumn == "Status") cmbSortMethod.SelectedIndex = sortAscending ? 5 : 4;
                isUpdatingSort = false;
            }

            lblInfo.Text = $"Showing {query.Count()} system accounts | {DataManager.Users.Count(u => u.IsActive)} total active members";
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            ShowCreateUserDialog();
        }

        private void btnToggleU_Click(object sender, EventArgs e)
        {
            if (staffGrid.SelectedRows.Count == 0) return;
            int id = (int)staffGrid.SelectedRows[0].Cells["ID"].Value;
            var user = DataManager.Users.FirstOrDefault(u => u.Id == id);
            
            if (user != null) {
                if (user.Role == "Admin" && user.IsActive && DataManager.Users.Count(u => u.Role == "Admin" && u.IsActive) <= 1) {
                    MessageBox.Show("Security Violation: Cannot deactivate the last active administrator.", "System Guard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                user.IsActive = !user.IsActive;
                DataManager.LogAudit(DataManager.CurrentUser?.Username, $"{(user.IsActive ? "Activated" : "Deactivated")} User {user.Username}");
                RefreshStaffGrid(txtSearch.Text);
            }
        }



        private void staffGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (staffGrid.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                e.CellStyle.ForeColor = e.Value.ToString() == "Active" ? Color.FromArgb(16, 185, 129) : Color.FromArgb(239, 68, 68);
                e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (staffGrid.SelectedRows.Count == 0) return;
            int id = (int)staffGrid.SelectedRows[0].Cells["ID"].Value;
            var user = DataManager.Users.FirstOrDefault(u => u.Id == id);
            
            if (user != null) {
                if (user.Role == "Admin" && DataManager.Users.Count(u => u.Role == "Admin") <= 1) {
                    MessageBox.Show("Security Violation: Cannot delete the last administrator.", "System Guard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show($"Are you sure you want to permanently delete {user.Username}?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    DataManager.Users.Remove(user);
                    DataManager.LogAudit(DataManager.CurrentUser?.Username, "Deleted User " + user.Username);
                    RefreshStaffGrid();
                }
            }
        }

        private void ShowCreateUserDialog()
        {
            Form f = new Form { Text = "Onboard New System Member", Size = new Size(450, 550), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, BackColor = Color.White };
            Panel p = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30) }; f.Controls.Add(p);
            int y = 20;

            var txtU = AddUIField(p, "System Username:", ref y);
            var txtE = AddUIField(p, "Secure Email Address:", ref y);
            var txtP = AddUIField(p, "Initial Security Pass:", ref y);
            txtP.PasswordChar = '*';

            Label lblR = new Label { Text = "SYSTEM ROLE:", Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            ComboBox cmbR = new ComboBox { Location = new Point(0, y + 25), Width = 390, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11) };
            cmbR.Items.AddRange(new string[] { "Admin", "Receptionist" }); cmbR.SelectedIndex = 1;
            
            Label lblNote = new Label { Text = "* Note: For intensive physician profile management (specialties, shifts, etc.),\nplease use the Physician Management module.", Location = new Point(0, y + 55), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Italic), ForeColor = Color.FromArgb(100, 116, 139) };
            
            p.Controls.AddRange(new Control[] { lblR, cmbR, lblNote }); y += 110;

            Button btnS = new Button { 
                Text = "CREATE ACCOUNT", 
                Location = new Point(0, y), 
                Size = new Size(390, 50), 
                BackColor = Color.FromArgb(16, 185, 129), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = new Font("Segoe UI", 11, FontStyle.Bold), 
                Cursor = Cursors.Hand 
            };
            
            btnS.Click += (s, e) => {
                if (string.IsNullOrEmpty(txtU.Text) || txtU.Text.Length < 3) { MessageBox.Show("Invalid username (min 3 chars)."); return; }
                if (DataManager.Users.Any(u => u.Username.Equals(txtU.Text, StringComparison.OrdinalIgnoreCase))) {
                    MessageBox.Show("Username already exists."); return;
                }
                
                DataManager.Users.Add(new User {
                    Id = DataManager.Users.Max(x => x.Id) + 1,
                    Username = txtU.Text,
                    Email = txtE.Text,
                    Password = ClinicAppointmentSystem.Utils.PasswordHasher.HashPassword(txtP.Text),
                    Role = cmbR.SelectedItem.ToString(),
                    IsActive = true
                });
                DataManager.LogAudit(DataManager.CurrentUser?.Username, "Created Staff Account: " + txtU.Text);
                f.DialogResult = DialogResult.OK;
            };
            p.Controls.Add(btnS);

            if (f.ShowDialog() == DialogResult.OK) RefreshStaffGrid(txtSearch.Text);
        }

        private TextBox AddUIField(Panel p, string label, ref int y)
        {
            Label lbl = new Label { Text = label.ToUpper(), Location = new Point(0, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            TextBox txt = new TextBox { Location = new Point(0, y + 25), Width = 390, Font = new Font("Segoe UI", 11) };
            p.Controls.AddRange(new Control[] { lbl, txt }); y += 75;
            return txt;
        }
    }
}
