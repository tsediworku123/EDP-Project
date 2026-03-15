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
        private System.Windows.Forms.ListView lvPatients;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colFullName;
        private System.Windows.Forms.ColumnHeader colPhone;
        private System.Windows.Forms.ColumnHeader colDOB;
        private System.Windows.Forms.ColumnHeader colGender;
        private System.Windows.Forms.ColumnHeader colAddress;
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
            this.lvPatients = new System.Windows.Forms.ListView();
            this.colId = new System.Windows.Forms.ColumnHeader();
            this.colFullName = new System.Windows.Forms.ColumnHeader();
            this.colPhone = new System.Windows.Forms.ColumnHeader();
            this.colDOB = new System.Windows.Forms.ColumnHeader();
            this.colGender = new System.Windows.Forms.ColumnHeader();
            this.colAddress = new System.Windows.Forms.ColumnHeader();
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
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.BackColor = System.Drawing.Color.FromArgb(240, 248, 255);

            // panelHeader
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(0, 191, 255);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 80;
            this.panelHeader.TabIndex = 0;

            this.lblTitle.Text = "Patient Management";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Size = new System.Drawing.Size(400, 45);

            this.lblSubtitle.Text = "Total Patients: 0";
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(230, 240, 255);
            this.lblSubtitle.Location = new System.Drawing.Point(20, 55);
            this.lblSubtitle.Size = new System.Drawing.Size(300, 25);

            this.txtSearch.Location = new System.Drawing.Point(800, 25);
            this.txtSearch.Size = new System.Drawing.Size(200, 27);
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 11F);

            this.btnSearch.Text = "Search";
            this.btnSearch.Location = new System.Drawing.Point(1010, 23);
            this.btnSearch.Size = new System.Drawing.Size(80, 30);
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            this.btnClear.Text = "Clear";
            this.btnClear.Location = new System.Drawing.Point(1100, 23);
            this.btnClear.Size = new System.Drawing.Size(80, 30);
            this.btnClear.BackColor = System.Drawing.Color.White;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.lblSubtitle);
            this.panelHeader.Controls.Add(this.txtSearch);
            this.panelHeader.Controls.Add(this.btnSearch);
            this.panelHeader.Controls.Add(this.btnClear);

            // lvPatients
            this.lvPatients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colId,
                this.colFullName,
                this.colPhone,
                this.colDOB,
                this.colGender,
                this.colAddress
            });
            this.lvPatients.FullRowSelect = true;
            this.lvPatients.GridLines = true;
            this.lvPatients.Location = new System.Drawing.Point(20, 100);
            this.lvPatients.Name = "lvPatients";
            this.lvPatients.Size = new System.Drawing.Size(700, 500);
            this.lvPatients.TabIndex = 1;
            this.lvPatients.UseCompatibleStateImageBehavior = false;
            this.lvPatients.View = System.Windows.Forms.View.Details;
            this.lvPatients.SelectedIndexChanged += new System.EventHandler(this.lvPatients_SelectedIndexChanged);

            this.colId.Text = "ID";
            this.colId.Width = 40;
            this.colFullName.Text = "Full Name";
            this.colFullName.Width = 150;
            this.colPhone.Text = "Phone";
            this.colPhone.Width = 120;
            this.colDOB.Text = "DOB";
            this.colDOB.Width = 100;
            this.colGender.Text = "Gender";
            this.colGender.Width = 80;
            this.colAddress.Text = "Address";
            this.colAddress.Width = 180;

            // panelForm
            this.panelForm.BackColor = System.Drawing.Color.FromArgb(250, 250, 255);
            this.panelForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelForm.Location = new System.Drawing.Point(740, 100);
            this.panelForm.Size = new System.Drawing.Size(440, 500);
            this.panelForm.TabIndex = 2;

            this.lblFormTitle.Text = "Patient Details";
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.FromArgb(0, 105, 148);
            this.lblFormTitle.Location = new System.Drawing.Point(15, 15);
            this.lblFormTitle.Size = new System.Drawing.Size(200, 25);

            // Full Name
            this.lblFullName.Text = "Full Name:";
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblFullName.Location = new System.Drawing.Point(30, 60);
            this.lblFullName.Size = new System.Drawing.Size(100, 25);

            this.txtFullName.Location = new System.Drawing.Point(140, 60);
            this.txtFullName.Size = new System.Drawing.Size(280, 27);
            this.txtFullName.Font = new System.Drawing.Font("Segoe UI", 11F);

            // Phone
            this.lblPhone.Text = "Phone:";
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPhone.Location = new System.Drawing.Point(30, 100);
            this.lblPhone.Size = new System.Drawing.Size(100, 25);

            this.txtPhone.Location = new System.Drawing.Point(140, 100);
            this.txtPhone.Size = new System.Drawing.Size(280, 27);
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 11F);

            // Address
            this.lblAddress.Text = "Address:";
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAddress.Location = new System.Drawing.Point(30, 140);
            this.lblAddress.Size = new System.Drawing.Size(100, 25);

            this.txtAddress.Location = new System.Drawing.Point(140, 140);
            this.txtAddress.Size = new System.Drawing.Size(280, 27);
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 11F);

            // DOB
            this.lblDOB.Text = "Date of Birth:";
            this.lblDOB.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDOB.Location = new System.Drawing.Point(30, 180);
            this.lblDOB.Size = new System.Drawing.Size(100, 25);

            this.dtpDOB.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDOB.Location = new System.Drawing.Point(140, 180);
            this.dtpDOB.Size = new System.Drawing.Size(150, 27);
            this.dtpDOB.Font = new System.Drawing.Font("Segoe UI", 11F);

            // Gender
            this.lblGender.Text = "Gender:";
            this.lblGender.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblGender.Location = new System.Drawing.Point(30, 220);
            this.lblGender.Size = new System.Drawing.Size(100, 25);

            this.rbtnMale.Text = "Male";
            this.rbtnMale.Location = new System.Drawing.Point(140, 220);
            this.rbtnMale.Size = new System.Drawing.Size(60, 25);
            this.rbtnMale.Font = new System.Drawing.Font("Segoe UI", 11F);

            this.rbtnFemale.Text = "Female";
            this.rbtnFemale.Location = new System.Drawing.Point(220, 220);
            this.rbtnFemale.Size = new System.Drawing.Size(80, 25);
            this.rbtnFemale.Font = new System.Drawing.Font("Segoe UI", 11F);

            // Buttons
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(140, 280);
            this.btnAdd.Size = new System.Drawing.Size(90, 40);
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(241, 196, 15);
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(240, 280);
            this.btnEdit.Size = new System.Drawing.Size(90, 40);
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Enabled = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            this.btnSave.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(140, 340);
            this.btnSave.Size = new System.Drawing.Size(90, 40);
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(240, 340);
            this.btnDelete.Size = new System.Drawing.Size(90, 40);
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Enabled = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.panelForm.Controls.Add(this.lblFormTitle);
            this.panelForm.Controls.Add(this.lblFullName);
            this.panelForm.Controls.Add(this.txtFullName);
            this.panelForm.Controls.Add(this.lblPhone);
            this.panelForm.Controls.Add(this.txtPhone);
            this.panelForm.Controls.Add(this.lblAddress);
            this.panelForm.Controls.Add(this.txtAddress);
            this.panelForm.Controls.Add(this.lblDOB);
            this.panelForm.Controls.Add(this.dtpDOB);
            this.panelForm.Controls.Add(this.lblGender);
            this.panelForm.Controls.Add(this.rbtnMale);
            this.panelForm.Controls.Add(this.rbtnFemale);
            this.panelForm.Controls.Add(this.btnAdd);
            this.panelForm.Controls.Add(this.btnEdit);
            this.panelForm.Controls.Add(this.btnSave);
            this.panelForm.Controls.Add(this.btnDelete);

            // statusStrip
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblStatus
            });
            this.statusStrip.Location = new System.Drawing.Point(0, 678);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1200, 22);

            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";

            // Add controls
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.lvPatients);
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