using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcAppointmentGrid
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.cmbDoctor = new System.Windows.Forms.ComboBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.apptGrid = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPatient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDoctor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActionCheckIn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lblResults = new System.Windows.Forms.Label();
            
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apptGrid)).BeginInit();
            this.SuspendLayout();
            
            // pnlTop
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.cmbStatus);
            this.pnlTop.Controls.Add(this.cmbDoctor);
            this.pnlTop.Controls.Add(this.dtpDate);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 100;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(217, 21);
            this.lblTitle.Text = "APPOINTMENT OVERSIGHT";
            
            // dtpDate
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(20, 55);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(150, 20);
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            
            // cmbDoctor
            this.cmbDoctor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDoctor.Location = new System.Drawing.Point(180, 55);
            this.cmbDoctor.Name = "cmbDoctor";
            this.cmbDoctor.Size = new System.Drawing.Size(150, 21);
            this.cmbDoctor.SelectedIndexChanged += new System.EventHandler(this.cmbDoctor_SelectedIndexChanged);
            
            // cmbStatus
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Items.AddRange(new object[] {
            "All Statuses",
            "Scheduled",
            "Completed",
            "Cancelled",
            "No-Show"});
            this.cmbStatus.Location = new System.Drawing.Point(340, 55);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(150, 21);
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);
            
            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(500, 55);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(250, 20);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            
            // apptGrid
            this.apptGrid.AllowUserToAddRows = false;
            this.apptGrid.BackgroundColor = System.Drawing.Color.White;
            this.apptGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.apptGrid.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.apptGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.apptGrid.EnableHeadersVisualStyles = false;
            this.apptGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.apptGrid.ColumnHeadersHeight = 42;
            this.apptGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.apptGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colTime,
            this.colPatient,
            this.colDoctor,
            this.colReason,
            this.colStatus,
            this.ActionCheckIn});
            this.apptGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.apptGrid.Location = new System.Drawing.Point(0, 100);
            this.apptGrid.Name = "apptGrid";
            this.apptGrid.ReadOnly = true;
            this.apptGrid.RowHeadersVisible = false;
            this.apptGrid.RowTemplate.Height = 40;
            this.apptGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.apptGrid.Size = new System.Drawing.Size(800, 470);
            
            // colId
            this.colId.HeaderText = "ID";
            this.colId.Name = "Id";
            this.colId.Visible = false;
            
            // colTime
            this.colTime.HeaderText = "Time";
            this.colTime.Name = "Time";
            
            // colPatient
            this.colPatient.HeaderText = "Patient Name";
            this.colPatient.Name = "Patient";
            
            // colDoctor
            this.colDoctor.HeaderText = "Doctor";
            this.colDoctor.Name = "Doctor";
            
            // colReason
            this.colReason.HeaderText = "Reason";
            this.colReason.Name = "Reason";
            
            // colStatus
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "Status";
            
            // ActionCheckIn
            this.ActionCheckIn.HeaderText = "Check-in";
            this.ActionCheckIn.Name = "ActionCheckIn";
            this.ActionCheckIn.Text = "";
            this.ActionCheckIn.UseColumnTextForButtonValue = true;
            this.ActionCheckIn.Width = 60;
            
            // lblResults
            this.lblResults.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblResults.Location = new System.Drawing.Point(0, 570);
            this.lblResults.Name = "lblResults";
            this.lblResults.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblResults.Size = new System.Drawing.Size(800, 30);
            this.lblResults.Text = "Found 0 appointments";
            this.lblResults.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // UcAppointmentGrid
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.apptGrid);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.pnlTop);
            this.Name = "UcAppointmentGrid";
            this.Size = new System.Drawing.Size(800, 600);
            
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apptGrid)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.ComboBox cmbDoctor;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView apptGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDoctor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReason;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewButtonColumn ActionCheckIn;
        private System.Windows.Forms.Label lblResults;
    }
}
