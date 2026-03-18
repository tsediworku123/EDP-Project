namespace ClinicAppointmentSystem
{
    partial class MyAppointmentsForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controls
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ListView lvAppointments;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colDoctor;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colReason;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReschedule;
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
            this.btnClose = new System.Windows.Forms.Button();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lvAppointments = new System.Windows.Forms.ListView();
            this.colId = new System.Windows.Forms.ColumnHeader();
            this.colDoctor = new System.Windows.Forms.ColumnHeader();
            this.colDate = new System.Windows.Forms.ColumnHeader();
            this.colTime = new System.Windows.Forms.ColumnHeader();
            this.colReason = new System.Windows.Forms.ColumnHeader();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReschedule = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();

            this.panelHeader.SuspendLayout();
            this.panelFilter.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // Form Properties
            this.Text = "My Appointments";
            this.ClientSize = new System.Drawing.Size(1040, 610);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.BackColor = System.Drawing.Color.FromArgb(244, 247, 252);

            // panelHeader
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 80;

            this.lblTitle.Text = "Appointments";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(28, 40, 51);
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Size = new System.Drawing.Size(300, 50);

            // btnClose
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(850, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "✖";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // panelFilter
            this.panelFilter.BackColor = System.Drawing.Color.Transparent;
            this.panelFilter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panelFilter.Location = new System.Drawing.Point(340, 15);
            this.panelFilter.Size = new System.Drawing.Size(400, 55);
            this.panelHeader.Controls.Add(this.panelFilter);

            this.lblFilter.Text = "Status";
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFilter.ForeColor = System.Drawing.Color.Gray;
            this.lblFilter.Location = new System.Drawing.Point(0, 0);

            this.cmbFilter.Items.Clear();
            this.cmbFilter.Items.AddRange(new object[] { "All", "Upcoming", "Past", "Cancelled" });
            this.cmbFilter.SelectedIndex = 0;
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.Location = new System.Drawing.Point(0, 20);
            this.cmbFilter.Size = new System.Drawing.Size(150, 30);

            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(170, 18);
            this.btnRefresh.Size = new System.Drawing.Size(120, 35);
            this.btnRefresh.Text = "🔄 Refresh";

            // lvAppointments
            this.lvAppointments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colId, this.colDoctor, this.colDate, this.colTime, this.colReason, this.colStatus
            });
            this.lvAppointments.Location = new System.Drawing.Point(20, 100);
            this.lvAppointments.Size = new System.Drawing.Size(1000, 420);
            this.lvAppointments.FullRowSelect = true;
            this.lvAppointments.View = System.Windows.Forms.View.Details;
            this.lvAppointments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvAppointments.BackColor = System.Drawing.Color.White;
            this.lvAppointments.SelectedIndexChanged += new System.EventHandler(this.lvAppointments_SelectedIndexChanged);

            this.colId.Text = "ID"; this.colId.Width = 40;
            this.colDoctor.Text = "Doctor"; this.colDoctor.Width = 200;
            this.colDate.Text = "Date"; this.colDate.Width = 120;
            this.colTime.Text = "Time"; this.colTime.Width = 100;
            this.colReason.Text = "Reason"; this.colReason.Width = 300;
            this.colStatus.Text = "Status"; this.colStatus.Width = 120;

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(20, 535);
            this.btnCancel.Size = new System.Drawing.Size(180, 40);
            this.btnCancel.Text = "Cancel Appointment";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.btnReschedule.BackColor = System.Drawing.Color.FromArgb(241, 196, 15);
            this.btnReschedule.ForeColor = System.Drawing.Color.White;
            this.btnReschedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReschedule.Location = new System.Drawing.Point(210, 535);
            this.btnReschedule.Size = new System.Drawing.Size(150, 40);
            this.btnReschedule.Text = "Reschedule";
            this.btnReschedule.Click += new System.EventHandler(this.btnReschedule_Click);

            // statusStrip
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblStatus
            });
            this.statusStrip.Location = new System.Drawing.Point(0, 588);
            this.statusStrip.Size = new System.Drawing.Size(1040, 22);
            this.statusStrip.TabIndex = 5;

            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";

            // Add controls to form
            this.Controls.Add(this.btnReschedule);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lvAppointments);
            this.Controls.Add(this.panelFilter);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.statusStrip);

            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}