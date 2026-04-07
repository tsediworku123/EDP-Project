using System.Windows.Forms;
using System.Drawing;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcPatientDirectory
    {
        private System.ComponentModel.IContainer components = null;

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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.pnlTopTools = new System.Windows.Forms.Panel();
            this.txtPatientSearch = new System.Windows.Forms.TextBox();
            this.cmbPatientGender = new System.Windows.Forms.ComboBox();
            this.cmbStatusFilter = new System.Windows.Forms.ComboBox();
            this.cmbSortMethod = new System.Windows.Forms.ComboBox();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this.patientsGrid = new System.Windows.Forms.DataGridView();
            this.pGridWrap = new System.Windows.Forms.Panel();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnDeletePatient = new System.Windows.Forms.Button();
            this.btnEditPatient = new System.Windows.Forms.Button();
            this.btnBookAppt = new System.Windows.Forms.Button();
            this.btnDetectDups = new System.Windows.Forms.Button();
            this.btnTogglePatient = new System.Windows.Forms.Button();
            this.lblResults = new System.Windows.Forms.Label();
            this.pnlTopTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.patientsGrid)).BeginInit();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.Text = "CENTRAL PATIENT REGISTRY & ARCHIVE";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Size = new System.Drawing.Size(600, 35);

            // btnExport
            this.btnExport.Text = " EXPORT";
            this.btnExport.BackColor = System.Drawing.Color.White;
            this.btnExport.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnExport.FlatStyle = FlatStyle.Flat;
            this.btnExport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(900, 20);
            this.btnExport.Size = new System.Drawing.Size(130, 35);
            this.btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // pnlTopTools
            this.pnlTopTools.BackColor = System.Drawing.Color.White;
            this.pnlTopTools.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTopTools.Controls.Add(this.cmbSortMethod);
            this.pnlTopTools.Controls.Add(this.cmbStatusFilter);
            this.pnlTopTools.Controls.Add(this.txtPatientSearch);
            this.pnlTopTools.Controls.Add(this.cmbPatientGender);
            this.pnlTopTools.Controls.Add(this.lblSearchIcon);
            this.pnlTopTools.Location = new System.Drawing.Point(20, 75);
            this.pnlTopTools.Size = new System.Drawing.Size(1010, 50);
            this.pnlTopTools.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // lblSearchIcon
            this.lblSearchIcon.Text = "🔍";
            this.lblSearchIcon.Location = new System.Drawing.Point(15, 15);
            this.lblSearchIcon.Size = new System.Drawing.Size(25, 25);

            // txtPatientSearch
            this.txtPatientSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPatientSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPatientSearch.Location = new System.Drawing.Point(45, 15);
            this.txtPatientSearch.Size = new System.Drawing.Size(320, 20);
            this.txtPatientSearch.TextChanged += new System.EventHandler(this.FilterPatients);

            // cmbPatientGender
            this.cmbPatientGender.Location = new System.Drawing.Point(380, 12);
            this.cmbPatientGender.Width = 150;
            this.cmbPatientGender.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPatientGender.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPatientGender.Items.AddRange(new string[] { "All Genders", "Male", "Female" });
            this.cmbPatientGender.SelectedIndexChanged += new System.EventHandler(this.FilterPatients);

            // cmbStatusFilter
            this.cmbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbStatusFilter.FormattingEnabled = true;
            this.cmbStatusFilter.Items.AddRange(new object[] { "All Status", "Active", "Archived" });
            this.cmbStatusFilter.Location = new System.Drawing.Point(540, 12);
            this.cmbStatusFilter.Name = "cmbStatusFilter";
            this.cmbStatusFilter.Size = new System.Drawing.Size(140, 25);
            this.cmbStatusFilter.SelectedIndexChanged += new System.EventHandler(this.FilterPatients);

            // cmbSortMethod
            this.cmbSortMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSortMethod.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbSortMethod.FormattingEnabled = true;
            this.cmbSortMethod.Items.AddRange(new object[] {
            "Sort: Name A-Z",
            "Sort: Name Z-A",
            "Sort: Gender A-Z",
            "Sort: Recent Visit",
            "Sort: Status (Active first)",
            "Sort: Status (Archived first)"});
            this.cmbSortMethod.Location = new System.Drawing.Point(690, 12);
            this.cmbSortMethod.Name = "cmbSortMethod";
            this.cmbSortMethod.Size = new System.Drawing.Size(210, 25);
            this.cmbSortMethod.SelectedIndexChanged += new System.EventHandler(this.cmbSortMethod_SelectedIndexChanged);

            // patientsGrid
            this.patientsGrid.BackgroundColor = System.Drawing.Color.White;
            this.patientsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.patientsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patientsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.patientsGrid.ColumnHeadersHeight = 40;
            this.patientsGrid.ReadOnly = true;
            this.patientsGrid.RowHeadersVisible = false;
            this.patientsGrid.AllowUserToAddRows = false;
            this.patientsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // pGridWrap
            this.pGridWrap.Location = new System.Drawing.Point(20, 140);
            this.pGridWrap.Size = new System.Drawing.Size(1010, 480);
            this.pGridWrap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pGridWrap.Controls.Add(this.patientsGrid);

            // pnlActions - buttons added rightmost-first so they stack left from right edge
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Height = 60;
            this.pnlActions.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlActions.Padding = new System.Windows.Forms.Padding(0, 10, 10, 10);
            this.pnlActions.Controls.Add(this.btnDeletePatient);
            this.pnlActions.Controls.Add(this.btnEditPatient);
            this.pnlActions.Controls.Add(this.btnBookAppt);
            this.pnlActions.Controls.Add(this.btnTogglePatient);
            this.pnlActions.Controls.Add(this.btnDetectDups);
            this.pnlActions.Controls.Add(this.lblResults);

            // btnDeletePatient (rightmost)
            this.btnDeletePatient.Text = "  DELETE";
            this.btnDeletePatient.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnDeletePatient.ForeColor = System.Drawing.Color.White;
            this.btnDeletePatient.FlatStyle = FlatStyle.Flat;
            this.btnDeletePatient.FlatAppearance.BorderSize = 0;
            this.btnDeletePatient.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDeletePatient.Width = 120;
            this.btnDeletePatient.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeletePatient.Click += new System.EventHandler(this.btnDeletePatient_Click);

            // btnTogglePatient
            this.btnTogglePatient.Text = "  TOGGLE ARCHIVE";
            this.btnTogglePatient.BackColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.btnTogglePatient.ForeColor = System.Drawing.Color.White;
            this.btnTogglePatient.FlatStyle = FlatStyle.Flat;
            this.btnTogglePatient.FlatAppearance.BorderSize = 0;
            this.btnTogglePatient.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnTogglePatient.Width = 140;
            this.btnTogglePatient.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTogglePatient.Click += new System.EventHandler(this.btnTogglePatient_Click);
            
            // btnEditPatient
            this.btnEditPatient.Text = "  EDIT";
            this.btnEditPatient.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnEditPatient.ForeColor = System.Drawing.Color.White;
            this.btnEditPatient.FlatStyle = FlatStyle.Flat;
            this.btnEditPatient.FlatAppearance.BorderSize = 0;
            this.btnEditPatient.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEditPatient.Width = 100;
            this.btnEditPatient.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditPatient.Click += new System.EventHandler(this.btnEditPatient_Click);

            // btnBookAppt
            this.btnBookAppt.Text = "  BOOK APPT";
            this.btnBookAppt.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnBookAppt.ForeColor = System.Drawing.Color.White;
            this.btnBookAppt.FlatStyle = FlatStyle.Flat;
            this.btnBookAppt.FlatAppearance.BorderSize = 0;
            this.btnBookAppt.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBookAppt.Width = 120;
            this.btnBookAppt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBookAppt.Click += new System.EventHandler(this.btnBookAppt_Click);

            // btnDetectDups
            this.btnDetectDups.Text = "  DUPLICATE SCAN";
            this.btnDetectDups.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnDetectDups.ForeColor = System.Drawing.Color.White;
            this.btnDetectDups.FlatStyle = FlatStyle.Flat;
            this.btnDetectDups.FlatAppearance.BorderSize = 0;
            this.btnDetectDups.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDetectDups.Width = 165;
            this.btnDetectDups.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDetectDups.Click += new System.EventHandler(this.btnDetectDups_Click);

            // lblResults
            this.lblResults.Text = "Loading patient registry...";
            this.lblResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResults.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblResults.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);

            // UcPatientDirectory
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.pGridWrap);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlTopTools);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lblTitle);
            this.Size = new System.Drawing.Size(1050, 700);
            this.pnlTopTools.ResumeLayout(false);
            this.pnlTopTools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.patientsGrid)).EndInit();
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel pnlTopTools;
        private System.Windows.Forms.TextBox txtPatientSearch;
        private System.Windows.Forms.ComboBox cmbPatientGender;
        private System.Windows.Forms.ComboBox cmbStatusFilter;
        private System.Windows.Forms.ComboBox cmbSortMethod;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.DataGridView patientsGrid;
        private System.Windows.Forms.Panel pGridWrap;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnTogglePatient;
        private System.Windows.Forms.Button btnEditPatient;
        private System.Windows.Forms.Button btnBookAppt;
        private System.Windows.Forms.Button btnDeletePatient;
        private System.Windows.Forms.Button btnDetectDups;
        private System.Windows.Forms.Label lblResults;
    }
}
