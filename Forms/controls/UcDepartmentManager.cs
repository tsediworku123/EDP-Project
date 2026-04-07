using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClinicAppointmentSystem.Controls
{
    public class UcDepartmentManager : UserControl
    {
        private ListBox lstDepts;
        private TextBox txtDeptName;
        private Button btnAdd, btnRemove, btnSave;

        public UcDepartmentManager()
        {
            InitializeComponent();
            LoadDepartments();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(800, 600);
            this.BackColor = Color.White;

            Label lblTitle = new Label { 
                Text = "DEPARTMENT MANAGEMENT", 
                Font = new Font("Segoe UI", 16, FontStyle.Bold), 
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(30, 30), AutoSize = true 
            };

            Label lblHint = new Label { 
                Text = "Manage clinical departments and specializations used across the system.", 
                Font = new Font("Segoe UI", 10), 
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(30, 70), AutoSize = true 
            };

            lstDepts = new ListBox { 
                Location = new Point(30, 120), 
                Size = new Size(300, 400), 
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lAdd = new Label { Text = "Department Name:", Location = new Point(360, 120), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            txtDeptName = new TextBox { Location = new Point(360, 145), Size = new Size(300, 30), Font = new Font("Segoe UI", 11) };

            btnAdd = new Button { 
                Text = "ADD DEPARTMENT", Location = new Point(360, 190), Size = new Size(140, 40), 
                BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat 
            };
            btnAdd.Click += BtnAdd_Click;

            btnRemove = new Button { 
                Text = "REMOVE SELECTED", Location = new Point(360, 240), Size = new Size(140, 40), 
                BackColor = Color.FromArgb(239, 68, 68), ForeColor = Color.White, FlatStyle = FlatStyle.Flat 
            };
            btnRemove.Click += BtnRemove_Click;

            btnSave = new Button { 
                Text = "SAVE CHANGES", Location = new Point(360, 480), Size = new Size(300, 50), 
                BackColor = Color.FromArgb(59, 130, 246), ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;

            this.Controls.AddRange(new Control[] { lblTitle, lblHint, lstDepts, lAdd, txtDeptName, btnAdd, btnRemove, btnSave });
        }

        private void LoadDepartments()
        {
            lstDepts.Items.Clear();
            foreach (var dept in DataManager.Departments)
                lstDepts.Items.Add(dept);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string name = txtDeptName.Text.Trim();
            if (string.IsNullOrEmpty(name)) return;
            if (lstDepts.Items.Contains(name)) { MessageBox.Show("Department already exists."); return; }
            lstDepts.Items.Add(name);
            txtDeptName.Clear();
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (lstDepts.SelectedItem != null)
                lstDepts.Items.Remove(lstDepts.SelectedItem);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataManager.Departments.Clear();
            foreach (var item in lstDepts.Items)
                DataManager.Departments.Add(item.ToString());
            
            DataManager.SaveDepartments();
            MessageBox.Show("Departments updated successfully!", "Admin Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
