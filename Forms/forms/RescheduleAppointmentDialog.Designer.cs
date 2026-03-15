namespace ClinicAppointmentSystem
{
    partial class RescheduleAppointmentDialog
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblNewDate;
        private System.Windows.Forms.DateTimePicker dtpNewDate;
        private System.Windows.Forms.Label lblNewTime;
        private System.Windows.Forms.ListView lstTimeSlots;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.Button btnReschedule;
        private System.Windows.Forms.Button btnCancel;

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
            this.panelContent = new System.Windows.Forms.Panel();
            this.lblNewDate = new System.Windows.Forms.Label();
            this.dtpNewDate = new System.Windows.Forms.DateTimePicker();
            this.lblNewTime = new System.Windows.Forms.Label();
            this.lstTimeSlots = new System.Windows.Forms.ListView();
            this.colTime = new System.Windows.Forms.ColumnHeader();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.btnReschedule = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.panelHeader.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Reschedule Appointment";
            this.ClientSize = new System.Drawing.Size(500, 450);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.BackColor = System.Drawing.Color.White;

            // panelHeader
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(0, 191, 255);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.btnClose);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(500, 60);
            this.panelHeader.TabIndex = 0;

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 30);
            this.lblTitle.Text = "Reschedule Appointment";

            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(450, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.Text = "✖";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);

            // panelContent
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelContent.Controls.Add(this.lblNewDate);
            this.panelContent.Controls.Add(this.dtpNewDate);
            this.panelContent.Controls.Add(this.lblNewTime);
            this.panelContent.Controls.Add(this.lstTimeSlots);
            this.panelContent.Controls.Add(this.btnReschedule);
            this.panelContent.Controls.Add(this.btnCancel);
            this.panelContent.Location = new System.Drawing.Point(20, 80);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(460, 350);
            this.panelContent.TabIndex = 1;

            // lblNewDate
            this.lblNewDate.AutoSize = true;
            this.lblNewDate.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNewDate.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblNewDate.Location = new System.Drawing.Point(30, 25);
            this.lblNewDate.Name = "lblNewDate";
            this.lblNewDate.Size = new System.Drawing.Size(80, 20);
            this.lblNewDate.Text = "New Date:";

            // dtpNewDate
            this.dtpNewDate.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpNewDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNewDate.Location = new System.Drawing.Point(30, 50);
            this.dtpNewDate.Name = "dtpNewDate";
            this.dtpNewDate.Size = new System.Drawing.Size(400, 27);
            this.dtpNewDate.TabIndex = 0;

            // lblNewTime
            this.lblNewTime.AutoSize = true;
            this.lblNewTime.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNewTime.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblNewTime.Location = new System.Drawing.Point(30, 95);
            this.lblNewTime.Name = "lblNewTime";
            this.lblNewTime.Size = new System.Drawing.Size(82, 20);
            this.lblNewTime.Text = "New Time:";

            // lstTimeSlots
            this.lstTimeSlots.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colTime,
                this.colStatus
            });
            this.lstTimeSlots.FullRowSelect = true;
            this.lstTimeSlots.GridLines = true;
            this.lstTimeSlots.Location = new System.Drawing.Point(30, 120);
            this.lstTimeSlots.Name = "lstTimeSlots";
            this.lstTimeSlots.Size = new System.Drawing.Size(400, 120);
            this.lstTimeSlots.TabIndex = 1;
            this.lstTimeSlots.UseCompatibleStateImageBehavior = false;
            this.lstTimeSlots.View = System.Windows.Forms.View.Details;

            this.colTime.Text = "Time";
            this.colTime.Width = 150;
            this.colStatus.Text = "Status";
            this.colStatus.Width = 230;

            // btnReschedule
            this.btnReschedule.BackColor = System.Drawing.Color.FromArgb(0, 191, 255);
            this.btnReschedule.FlatAppearance.BorderSize = 0;
            this.btnReschedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReschedule.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnReschedule.ForeColor = System.Drawing.Color.White;
            this.btnReschedule.Location = new System.Drawing.Point(80, 270);
            this.btnReschedule.Name = "btnReschedule";
            this.btnReschedule.Size = new System.Drawing.Size(140, 40);
            this.btnReschedule.TabIndex = 2;
            this.btnReschedule.Text = "Reschedule";
            this.btnReschedule.UseVisualStyleBackColor = false;
            this.btnReschedule.Click += new System.EventHandler(this.btnReschedule_Click);

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(240, 270);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(140, 40);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // Add controls to form
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelHeader);

            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}