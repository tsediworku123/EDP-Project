using System.Windows.Forms;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcAppointmentOversight
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
            this.btnCancelAppt = new System.Windows.Forms.Button();
            this.btnChangeStatus = new System.Windows.Forms.Button();
            this.btnReassign = new System.Windows.Forms.Button();
            this.filterPanel = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpApptDateFilter = new System.Windows.Forms.DateTimePicker();
            this.lblDoc = new System.Windows.Forms.Label();
            this.cmbApptDoctorFilter = new System.Windows.Forms.ComboBox();
            this.lblStat = new System.Windows.Forms.Label();
            this.cmbApptStatusFilter = new System.Windows.Forms.ComboBox();
            this.lblResults = new System.Windows.Forms.Label();
            this.apptGrid = new System.Windows.Forms.DataGridView();
            this.filterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apptGrid)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Text = "APPOINTMENT OVERSIGHT";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Size = new System.Drawing.Size(400, 35);

            // btnCancelAppt
            this.btnCancelAppt.Text = " Cancel Appt";
            this.btnCancelAppt.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnCancelAppt.ForeColor = System.Drawing.Color.White;
            this.btnCancelAppt.FlatStyle = FlatStyle.Flat;
            this.btnCancelAppt.FlatAppearance.BorderSize = 0;
            this.btnCancelAppt.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelAppt.Location = new System.Drawing.Point(440, 25);
            this.btnCancelAppt.Size = new System.Drawing.Size(150, 35);
            this.btnCancelAppt.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnCancelAppt.Click += new System.EventHandler(this.btnCancelAppt_Click);

            // btnChangeStatus
            this.btnChangeStatus.Text = " Update Status";
            this.btnChangeStatus.BackColor = System.Drawing.Color.FromArgb(243, 156, 18);
            this.btnChangeStatus.ForeColor = System.Drawing.Color.White;
            this.btnChangeStatus.FlatStyle = FlatStyle.Flat;
            this.btnChangeStatus.FlatAppearance.BorderSize = 0;
            this.btnChangeStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnChangeStatus.Location = new System.Drawing.Point(600, 25);
            this.btnChangeStatus.Size = new System.Drawing.Size(150, 35);
            this.btnChangeStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnChangeStatus.Click += new System.EventHandler(this.btnChangeStatus_Click);

            // btnReassign
            this.btnReassign.Text = " Reassign Dr.";
            this.btnReassign.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnReassign.ForeColor = System.Drawing.Color.White;
            this.btnReassign.FlatStyle = FlatStyle.Flat;
            this.btnReassign.FlatAppearance.BorderSize = 0;
            this.btnReassign.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReassign.Location = new System.Drawing.Point(760, 25);
            this.btnReassign.Size = new System.Drawing.Size(150, 35);
            this.btnReassign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnReassign.Click += new System.EventHandler(this.btnReassign_Click);

            // filterPanel
            this.filterPanel.BackColor = System.Drawing.Color.White;
            this.filterPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.filterPanel.Padding = new System.Windows.Forms.Padding(10);
            this.filterPanel.Location = new System.Drawing.Point(20, 80);
            this.filterPanel.Size = new System.Drawing.Size(890, 60);
            this.filterPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.filterPanel.Controls.Add(this.lblDate);
            this.filterPanel.Controls.Add(this.dtpApptDateFilter);
            this.filterPanel.Controls.Add(this.lblDoc);
            this.filterPanel.Controls.Add(this.cmbApptDoctorFilter);
            this.filterPanel.Controls.Add(this.lblStat);
            this.filterPanel.Controls.Add(this.cmbApptStatusFilter);

            // lblDate
            this.lblDate.Text = "Date:";
            this.lblDate.Location = new System.Drawing.Point(15, 20);
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            // dtpApptDateFilter
            this.dtpApptDateFilter.Location = new System.Drawing.Point(65, 17);
            this.dtpApptDateFilter.Width = 120;
            this.dtpApptDateFilter.Format = DateTimePickerFormat.Short;
            this.dtpApptDateFilter.ValueChanged += new System.EventHandler(this.FilterAppointments);

            // lblDoc
            this.lblDoc.Text = "Doctor:";
            this.lblDoc.Location = new System.Drawing.Point(205, 20);
            this.lblDoc.AutoSize = true;
            this.lblDoc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            // cmbApptDoctorFilter
            this.cmbApptDoctorFilter.Location = new System.Drawing.Point(270, 17);
            this.cmbApptDoctorFilter.Width = 180;
            this.cmbApptDoctorFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbApptDoctorFilter.SelectedIndexChanged += new System.EventHandler(this.FilterAppointments);

            // lblStat
            this.lblStat.Text = "Status:";
            this.lblStat.Location = new System.Drawing.Point(470, 20);
            this.lblStat.AutoSize = true;
            this.lblStat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            // cmbApptStatusFilter
            this.cmbApptStatusFilter.Location = new System.Drawing.Point(530, 17);
            this.cmbApptStatusFilter.Width = 120;
            this.cmbApptStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbApptStatusFilter.Items.AddRange(new string[] { "All", "Scheduled", "Completed", "Cancelled", "No-show" });
            this.cmbApptStatusFilter.SelectedIndexChanged += new System.EventHandler(this.FilterAppointments);

            // lblResults
            this.lblResults.Text = "Showing results...";
            this.lblResults.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblResults.Location = new System.Drawing.Point(20, 142);
            this.lblResults.Size = new System.Drawing.Size(300, 20);

            // apptGrid
            this.apptGrid.BackgroundColor = System.Drawing.Color.White;
            this.apptGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.apptGrid.RowHeadersVisible = false;
            this.apptGrid.AllowUserToAddRows = false;
            this.apptGrid.ReadOnly = true;
            this.apptGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.apptGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.apptGrid.Location = new System.Drawing.Point(20, 160);
            this.apptGrid.Size = new System.Drawing.Size(890, 365);
            this.apptGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // UcAppointmentOversight
            this.BackColor = System.Drawing.Color.White;
            this.Size = new System.Drawing.Size(930, 550);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnCancelAppt);
            this.Controls.Add(this.btnChangeStatus);
            this.Controls.Add(this.btnReassign);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.apptGrid);
            this.filterPanel.ResumeLayout(false);
            this.filterPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apptGrid)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCancelAppt;
        private System.Windows.Forms.Button btnChangeStatus;
        private System.Windows.Forms.Button btnReassign;
        private System.Windows.Forms.Panel filterPanel;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpApptDateFilter;
        private System.Windows.Forms.Label lblDoc;
        private System.Windows.Forms.ComboBox cmbApptDoctorFilter;
        private System.Windows.Forms.Label lblStat;
        private System.Windows.Forms.ComboBox cmbApptStatusFilter;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.DataGridView apptGrid;
    }
}
