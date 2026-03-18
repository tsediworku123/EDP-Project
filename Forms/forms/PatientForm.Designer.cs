namespace ClinicAppointmentSystem
{
    partial class PatientForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controls
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgvPatients;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDOB;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cmbFilterGender;
        private System.Windows.Forms.Panel panelForm;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblDOB;
        private System.Windows.Forms.DateTimePicker dtpDOB;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.RadioButton rbtnMale;
        private System.Windows.Forms.RadioButton rbtnFemale;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvPatients = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDOB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cmbFilterGender = new System.Windows.Forms.ComboBox();
            this.panelForm = new System.Windows.Forms.Panel();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblDOB = new System.Windows.Forms.Label();
            this.dtpDOB = new System.Windows.Forms.DateTimePicker();
            this.lblGender = new System.Windows.Forms.Label();
            this.rbtnMale = new System.Windows.Forms.RadioButton();
            this.rbtnFemale = new System.Windows.Forms.RadioButton();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();

            this.panelHeader.SuspendLayout();
            this.panelForm.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Manage Patients";
            this.ClientSize = new System.Drawing.Size(1040, 610);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.BackColor = System.Drawing.Color.FromArgb(244, 247, 252);

            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 80;

            this.lblTitle.Text = "Patients";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(28, 40, 51);
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Size = new System.Drawing.Size(200, 50);

            this.lblFilter.Text = "Filter by Gender";
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFilter.ForeColor = System.Drawing.Color.Gray;
            this.lblFilter.Location = new System.Drawing.Point(300, 15);
            this.lblFilter.Size = new System.Drawing.Size(150, 20);

            this.cmbFilterGender.Items.AddRange(new object[] { "All", "Male", "Female" });
            this.cmbFilterGender.SelectedIndex = 0;
            this.cmbFilterGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterGender.Location = new System.Drawing.Point(300, 35);
            this.cmbFilterGender.Size = new System.Drawing.Size(150, 30);
            this.cmbFilterGender.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);

            this.txtSearch.Location = new System.Drawing.Point(500, 35);
            this.txtSearch.Size = new System.Drawing.Size(200, 30);
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Text = "Search by name...";

            this.btnSearch.Text = "Search";
            this.btnSearch.Location = new System.Drawing.Point(710, 33);
            this.btnSearch.Size = new System.Drawing.Size(80, 30);
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            this.btnClear.Text = "Clear";
            this.btnClear.Location = new System.Drawing.Point(800, 33);
            this.btnClear.Size = new System.Drawing.Size(80, 30);
            this.btnClear.BackColor = System.Drawing.Color.White;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.lblFilter);
            this.panelHeader.Controls.Add(this.cmbFilterGender);
            this.panelHeader.Controls.Add(this.txtSearch);
            this.panelHeader.Controls.Add(this.btnSearch);
            this.panelHeader.Controls.Add(this.btnClear);

            // dgvPatients
            this.dgvPatients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colId,
                this.colFullName,
                this.colPhone,
                this.colDOB,
                this.colGender,
                this.colAddress
            });
            this.dgvPatients.Location = new System.Drawing.Point(20, 95);
            this.dgvPatients.Size = new System.Drawing.Size(650, 480);
            this.dgvPatients.BackgroundColor = System.Drawing.Color.White;
            this.dgvPatients.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPatients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPatients.ReadOnly = true;
            this.dgvPatients.RowHeadersVisible = false;
            this.dgvPatients.AllowUserToAddRows = false;
            this.dgvPatients.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPatients.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvPatients.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(244, 247, 252);
            this.dgvPatients.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvPatients.EnableHeadersVisualStyles = false;
            this.dgvPatients.SelectionChanged += new System.EventHandler(this.dgvPatients_SelectionChanged);

            this.colId.HeaderText = "ID";
            this.colId.Width = 40;
            this.colFullName.HeaderText = "Full Name";
            this.colFullName.Width = 150;
            this.colPhone.HeaderText = "Phone";
            this.colPhone.Width = 100;
            this.colDOB.HeaderText = "DOB";
            this.colDOB.Width = 90;
            this.colGender.HeaderText = "Gender";
            this.colGender.Width = 70;
            this.colAddress.HeaderText = "Address";
            this.colAddress.Width = 150;

            // panelForm
            this.panelForm.BackColor = System.Drawing.Color.White;
            this.panelForm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panelForm.Location = new System.Drawing.Point(680, 95);
            this.panelForm.Size = new System.Drawing.Size(340, 480);
            this.panelForm.TabIndex = 2;

            this.lblFormTitle.Text = "Patient Details";
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.FromArgb(28, 40, 51);
            this.lblFormTitle.Location = new System.Drawing.Point(15, 15);
            this.lblFormTitle.Size = new System.Drawing.Size(200, 30);

            this.lblFullName.Text = "Full Name";
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFullName.Location = new System.Drawing.Point(15, 60);
            this.lblFullName.Size = new System.Drawing.Size(100, 20);

            this.txtFullName.Location = new System.Drawing.Point(15, 80);
            this.txtFullName.Size = new System.Drawing.Size(310, 25);

            this.lblPhone.Text = "Phone";
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPhone.Location = new System.Drawing.Point(15, 120);
            this.lblPhone.Size = new System.Drawing.Size(100, 20);

            this.txtPhone.Location = new System.Drawing.Point(15, 140);
            this.txtPhone.Size = new System.Drawing.Size(310, 25);

            this.lblAddress.Text = "Address";
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAddress.Location = new System.Drawing.Point(15, 180);
            this.lblAddress.Size = new System.Drawing.Size(100, 20);

            this.txtAddress.Location = new System.Drawing.Point(15, 200);
            this.txtAddress.Size = new System.Drawing.Size(310, 25);

            this.lblDOB.Text = "Date of Birth";
            this.lblDOB.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDOB.Location = new System.Drawing.Point(15, 240);
            this.lblDOB.Size = new System.Drawing.Size(100, 20);

            this.dtpDOB.Location = new System.Drawing.Point(15, 260);
            this.dtpDOB.Size = new System.Drawing.Size(310, 25);

            this.lblGender.Text = "Gender";
            this.lblGender.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblGender.Location = new System.Drawing.Point(15, 300);
            this.lblGender.Size = new System.Drawing.Size(100, 20);

            this.rbtnMale.Location = new System.Drawing.Point(15, 320);
            this.rbtnFemale.Location = new System.Drawing.Point(100, 320);

            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnAdd.Location = new System.Drawing.Point(15, 370);
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnSave.Location = new System.Drawing.Point(115, 370);
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(241, 196, 15);
            this.btnEdit.Location = new System.Drawing.Point(215, 370);
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnDelete.Location = new System.Drawing.Point(15, 420);
            this.btnDelete.Size = new System.Drawing.Size(290, 40);

            this.panelForm.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblFormTitle, lblFullName, txtFullName, lblPhone, txtPhone,
                lblAddress, txtAddress, lblDOB, dtpDOB, lblGender,
                rbtnMale, rbtnFemale, btnAdd, btnSave, btnEdit, btnDelete
            });

            // statusStrip
            this.statusStrip.Location = new System.Drawing.Point(0, 588);
            this.statusStrip.Size = new System.Drawing.Size(1040, 22);

            // Add controls
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.dgvPatients);
            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.statusStrip);

            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelForm.ResumeLayout(false);
            this.panelForm.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}