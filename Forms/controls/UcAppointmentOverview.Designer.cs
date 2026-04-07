using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem
{
    partial class UcAppointmentOverview
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
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.l1 = new System.Windows.Forms.Label();
            this.cmbDoctor = new System.Windows.Forms.ComboBox();
            this.l2 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.l3 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.btnClear = new System.Windows.Forms.Button();
            this.pGridWrap = new System.Windows.Forms.Panel();
            this.dgvAll = new System.Windows.Forms.DataGridView();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPatient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDoctor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblCount = new System.Windows.Forms.Label();
            
            this.pnlFilters.SuspendLayout();
            this.pGridWrap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAll)).BeginInit();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(25, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(534, 30);
            this.lblTitle.Text = "CENTRAL APPOINTMENT OVERLOOK && OVERSIGHT";
            
            // pnlFilters
            this.pnlFilters.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            this.pnlFilters.BackColor = System.Drawing.Color.White;
            this.pnlFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilters.Controls.Add(this.btnClear);
            this.pnlFilters.Controls.Add(this.dtpDate);
            this.pnlFilters.Controls.Add(this.l3);
            this.pnlFilters.Controls.Add(this.cmbStatus);
            this.pnlFilters.Controls.Add(this.l2);
            this.pnlFilters.Controls.Add(this.cmbDoctor);
            this.pnlFilters.Controls.Add(this.l1);
            this.pnlFilters.Location = new System.Drawing.Point(25, 75);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(1000, 55);
            
            // l1
            this.l1.AutoSize = true;
            this.l1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.l1.Location = new System.Drawing.Point(15, 18);
            this.l1.Name = "l1";
            this.l1.Size = new System.Drawing.Size(83, 13);
            this.l1.Text = "FILTER BY DR:";
            
            // cmbDoctor
            this.cmbDoctor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDoctor.Location = new System.Drawing.Point(110, 15);
            this.cmbDoctor.Name = "cmbDoctor";
            this.cmbDoctor.Size = new System.Drawing.Size(180, 21);
            this.cmbDoctor.SelectedIndexChanged += new System.EventHandler(this.cmbDoctor_SelectedIndexChanged);
            
            // l2
            this.l2.AutoSize = true;
            this.l2.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.l2.Location = new System.Drawing.Point(310, 18);
            this.l2.Name = "l2";
            this.l2.Size = new System.Drawing.Size(51, 13);
            this.l2.Text = "STATUS:";
            
            // cmbStatus
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Items.AddRange(new object[] {
            "All Statuses",
            "Scheduled",
            "In Progress",
            "Completed",
            "Cancelled",
            "No-Show"});
            this.cmbStatus.Location = new System.Drawing.Point(380, 15);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(140, 21);
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);
            
            // l3
            this.l3.AutoSize = true;
            this.l3.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.l3.Location = new System.Drawing.Point(540, 18);
            this.l3.Name = "l3";
            this.l3.Size = new System.Drawing.Size(39, 13);
            this.l3.Text = "DATE:";
            
            // dtpDate
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(590, 15);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(150, 20);
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            
            // btnClear
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(760, 13);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 30);
            this.btnClear.Text = "RESET";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            
            // pGridWrap
            this.pGridWrap.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            this.pGridWrap.BackColor = System.Drawing.Color.White;
            this.pGridWrap.Controls.Add(this.dgvAll);
            this.pGridWrap.Location = new System.Drawing.Point(25, 150);
            this.pGridWrap.Name = "pGridWrap";
            this.pGridWrap.Size = new System.Drawing.Size(1000, 520);
            
            // dgvAll
            this.dgvAll.AllowUserToAddRows = false;
            this.dgvAll.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAll.BackgroundColor = System.Drawing.Color.White;
            this.dgvAll.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAll.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.dgvAll.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvAll.EnableHeadersVisualStyles = false;
            this.dgvAll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAll.ColumnHeadersHeight = 40;
            this.dgvAll.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDate,
            this.colTime,
            this.colPatient,
            this.colDoctor,
            this.colReason,
            this.colStatus});
            this.dgvAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAll.Location = new System.Drawing.Point(0, 0);
            this.dgvAll.Name = "dgvAll";
            this.dgvAll.ReadOnly = true;
            this.dgvAll.RowHeadersVisible = false;
            this.dgvAll.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAll.Size = new System.Drawing.Size(1000, 520);
            
            // colDate
            this.colDate.HeaderText = "Sch. Date";
            this.colDate.Name = "Date";
            
            // colTime
            this.colTime.HeaderText = "Time";
            this.colTime.Name = "Time";
            
            // colPatient
            this.colPatient.HeaderText = "Patient Name";
            this.colPatient.Name = "Patient";
            
            // colDoctor
            this.colDoctor.HeaderText = "Doctor Info";
            this.colDoctor.Name = "Doctor";
            
            // colReason
            this.colReason.HeaderText = "Reason/Complaint";
            this.colReason.Name = "Reason";
            
            // colStatus
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "Status";
            
            // lblCount
            this.lblCount.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblCount.Location = new System.Drawing.Point(25, 675);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(91, 15);
            this.lblCount.Text = "Found 0 records";
            
            // UcAppointmentOverview
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.pGridWrap);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.lblTitle);
            this.Name = "UcAppointmentOverview";
            this.Size = new System.Drawing.Size(1050, 720);
            
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.pGridWrap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAll)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label l1;
        private System.Windows.Forms.ComboBox cmbDoctor;
        private System.Windows.Forms.Label l2;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label l3;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel pGridWrap;
        private System.Windows.Forms.DataGridView dgvAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDoctor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReason;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.Label lblCount;
    }
}
