using System.Windows.Forms;
using System.Drawing;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcDoctorManagement
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnAddDoctor = new System.Windows.Forms.Button();
            this.pnlTopTools = new System.Windows.Forms.Panel();
            this.txtDoctorSearch = new System.Windows.Forms.TextBox();
            this.cmbDoctorSpec = new System.Windows.Forms.ComboBox();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this.doctorsGrid = new System.Windows.Forms.DataGridView();
            this.cmbStatusFilter = new System.Windows.Forms.ComboBox();
            this.cmbSortMethod = new System.Windows.Forms.ComboBox();
            this.btnDeleteDoctor = new System.Windows.Forms.Button();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnEditDoctor = new System.Windows.Forms.Button();
            this.btnToggleDoctor = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblResults = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlTopTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.doctorsGrid)).BeginInit();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            
            // pnlHeader
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 140;
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.btnAddDoctor);
            this.pnlHeader.Controls.Add(this.pnlTopTools);

            // lblTitle
            this.lblTitle.Text = "PHYSICIAN REGISTRY & SHIFT PLANNING [UPDATED]";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(20, 18);
            this.lblTitle.Size = new System.Drawing.Size(550, 35);

            // btnAddDoctor
            this.btnAddDoctor.Text = "  ✚  REGISTER NEW PHYSICIAN";
            this.btnAddDoctor.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnAddDoctor.ForeColor = System.Drawing.Color.White;
            this.btnAddDoctor.FlatStyle = FlatStyle.Flat;
            this.btnAddDoctor.FlatAppearance.BorderSize = 0;
            this.btnAddDoctor.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddDoctor.Location = new System.Drawing.Point(740, 15);
            this.btnAddDoctor.Size = new System.Drawing.Size(260, 40);
            this.btnAddDoctor.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnAddDoctor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddDoctor.Click += new System.EventHandler(this.btnAddDoctor_Click);

            // pnlTopTools
            this.pnlTopTools.BackColor = System.Drawing.Color.White;
            this.pnlTopTools.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTopTools.Location = new System.Drawing.Point(20, 75);
            this.pnlTopTools.Size = new System.Drawing.Size(1000, 50);
            this.pnlTopTools.Padding = new Padding(10, 8, 10, 8);
            this.pnlTopTools.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlTopTools.Controls.Add(this.txtDoctorSearch);
            this.pnlTopTools.Controls.Add(this.lblSearchIcon);
            this.pnlTopTools.Controls.Add(this.cmbDoctorSpec);
            this.pnlTopTools.Controls.Add(this.cmbStatusFilter);
            this.pnlTopTools.Controls.Add(this.cmbSortMethod);

            // txtDoctorSearch
            this.txtDoctorSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDoctorSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtDoctorSearch.Location = new System.Drawing.Point(45, 15);
            this.txtDoctorSearch.Size = new System.Drawing.Size(350, 20);
            this.txtDoctorSearch.TextChanged += new System.EventHandler(this.FilterDoctors);

            // cmbDoctorSpec
            this.cmbDoctorSpec.Location = new System.Drawing.Point(410, 12);
            this.cmbDoctorSpec.Width = 180;
            this.cmbDoctorSpec.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDoctorSpec.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbDoctorSpec.SelectedIndexChanged += new System.EventHandler(this.FilterDoctors);

            // cmbStatusFilter
            this.cmbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbStatusFilter.FormattingEnabled = true;
            this.cmbStatusFilter.Items.AddRange(new object[] {
            "All Status",
            "Active Duty",
            "On Leave"});
            this.cmbStatusFilter.Location = new System.Drawing.Point(600, 12);
            this.cmbStatusFilter.Name = "cmbStatusFilter";
            this.cmbStatusFilter.Size = new System.Drawing.Size(150, 25);
            this.cmbStatusFilter.SelectedIndex = 0;
            this.cmbStatusFilter.SelectedIndexChanged += new System.EventHandler(this.FilterDoctors);

            // cmbSortMethod
            this.cmbSortMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSortMethod.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbSortMethod.FormattingEnabled = true;
            this.cmbSortMethod.Items.AddRange(new object[] {
            "Sort: Name A-Z",
            "Sort: Name Z-A",
            "Sort: Specialty A-Z",
            "Sort: Specialty Z-A",
            "Sort: Status (Active first)",
            "Sort: Status (Leave first)"});
            this.cmbSortMethod.Location = new System.Drawing.Point(760, 12);
            this.cmbSortMethod.Name = "cmbSortMethod";
            this.cmbSortMethod.Size = new System.Drawing.Size(220, 25);
            this.cmbSortMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSortMethod.SelectedIndex = 0;
            this.cmbSortMethod.SelectedIndexChanged += new System.EventHandler(this.cmbSortMethod_SelectedIndexChanged);

            // lblSearchIcon
            this.lblSearchIcon.Text = "🔍";
            this.lblSearchIcon.Location = new System.Drawing.Point(15, 15);
            this.lblSearchIcon.Size = new System.Drawing.Size(25, 25);

            // doctorsGrid
            this.doctorsGrid.BackgroundColor = System.Drawing.Color.White;
            this.doctorsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.doctorsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doctorsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.doctorsGrid.ColumnHeadersHeight = 40;
            this.doctorsGrid.ReadOnly = true;
            this.doctorsGrid.RowHeadersVisible = false;
            this.doctorsGrid.AllowUserToAddRows = false;
            this.doctorsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Height = 60;
            this.pnlActions.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlActions.Padding = new System.Windows.Forms.Padding(0, 10, 10, 10);
            this.pnlActions.Controls.Add(this.lblResults);
            this.pnlActions.Controls.Add(this.btnToggleDoctor);
            this.pnlActions.Controls.Add(this.btnEditDoctor);
            this.pnlActions.Controls.Add(this.btnDeleteDoctor);

            // btnDeleteDoctor (rightmost)
            this.btnDeleteDoctor.Text = "  🗑  DELETE";
            this.btnDeleteDoctor.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnDeleteDoctor.ForeColor = System.Drawing.Color.White;
            this.btnDeleteDoctor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteDoctor.FlatAppearance.BorderSize = 0;
            this.btnDeleteDoctor.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDeleteDoctor.Width = 120;
            this.btnDeleteDoctor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeleteDoctor.Click += new System.EventHandler(this.btnDeleteDoctor_Click);

            // btnEditDoctor
            this.btnEditDoctor.Text = "  📋  SHIFT MANAGER";
            this.btnEditDoctor.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnEditDoctor.ForeColor = System.Drawing.Color.White;
            this.btnEditDoctor.FlatStyle = FlatStyle.Flat;
            this.btnEditDoctor.FlatAppearance.BorderSize = 0;
            this.btnEditDoctor.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEditDoctor.Width = 155;
            this.btnEditDoctor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditDoctor.Click += new System.EventHandler(this.btnEditDoctor_Click);

            // btnToggleDoctor
            this.btnToggleDoctor.Text = "  ⚡  TOGGLE STATUS";
            this.btnToggleDoctor.BackColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.btnToggleDoctor.ForeColor = System.Drawing.Color.White;
            this.btnToggleDoctor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleDoctor.FlatAppearance.BorderSize = 0;
            this.btnToggleDoctor.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnToggleDoctor.Width = 155;
            this.btnToggleDoctor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToggleDoctor.Click += new System.EventHandler(this.btnToggleDoctor_Click);

            // lblResults
            this.lblResults.Text = "Showing statistical summary of active providers.";
            this.lblResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResults.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblResults.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);


            // UcDoctorManagement
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.doctorsGrid);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlHeader);
            this.Size = new System.Drawing.Size(1040, 700);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlTopTools.ResumeLayout(false);
            this.pnlTopTools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.doctorsGrid)).EndInit();
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnAddDoctor;
        private System.Windows.Forms.Panel pnlTopTools;
        private System.Windows.Forms.TextBox txtDoctorSearch;
        private System.Windows.Forms.ComboBox cmbDoctorSpec;
        private System.Windows.Forms.ComboBox cmbStatusFilter;
        private System.Windows.Forms.ComboBox cmbSortMethod;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.DataGridView doctorsGrid;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnEditDoctor;
        private System.Windows.Forms.Button btnToggleDoctor;
        private System.Windows.Forms.Button btnDeleteDoctor;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblResults;
    }
}
