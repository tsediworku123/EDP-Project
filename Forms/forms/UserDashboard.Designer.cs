namespace ClinicAppointmentSystem
{
    partial class UserDashboard
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Panel panelWelcome;
        private System.Windows.Forms.Label lblPatientName;
        private System.Windows.Forms.PictureBox picPatientIcon;
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Panel cardNotifications;
        private System.Windows.Forms.Panel cardAppointments;
        private System.Windows.Forms.Panel cardRecords;
        private System.Windows.Forms.Label lblNotifications;
        private System.Windows.Forms.Label lblTotalAppointments;
        private System.Windows.Forms.Label lblMedicalRecords;
        private System.Windows.Forms.Label lblNotificationsTitle;
        private System.Windows.Forms.Label lblAppointmentsTitle;
        private System.Windows.Forms.Label lblRecordsTitle;
        private System.Windows.Forms.Panel panelUpcoming;
        private System.Windows.Forms.Label lblUpcomingTitle;
        private System.Windows.Forms.ListView lvUpcoming;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colDoctor;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.Panel panelActions;
        private System.Windows.Forms.Button btnViewDoctors;
        private System.Windows.Forms.Button btnBookAppointment;
        private System.Windows.Forms.Button btnMyAppointments;
        private System.Windows.Forms.Button btnMedicalHistory;
        private System.Windows.Forms.Button btnProfile;
        private System.Windows.Forms.Button btnNotifications;
        private System.Windows.Forms.Button btnFeedback;

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
            this.lblWelcome = new System.Windows.Forms.Label();
            this.panelWelcome = new System.Windows.Forms.Panel();
            this.lblPatientName = new System.Windows.Forms.Label();
            this.picPatientIcon = new System.Windows.Forms.PictureBox();
            this.panelStats = new System.Windows.Forms.Panel();
            this.cardNotifications = new System.Windows.Forms.Panel();
            this.cardAppointments = new System.Windows.Forms.Panel();
            this.cardRecords = new System.Windows.Forms.Panel();
            this.lblNotifications = new System.Windows.Forms.Label();
            this.lblTotalAppointments = new System.Windows.Forms.Label();
            this.lblMedicalRecords = new System.Windows.Forms.Label();
            this.lblNotificationsTitle = new System.Windows.Forms.Label();
            this.lblAppointmentsTitle = new System.Windows.Forms.Label();
            this.lblRecordsTitle = new System.Windows.Forms.Label();
            this.panelUpcoming = new System.Windows.Forms.Panel();
            this.lblUpcomingTitle = new System.Windows.Forms.Label();
            this.lvUpcoming = new System.Windows.Forms.ListView();
            this.colDate = new System.Windows.Forms.ColumnHeader();
            this.colDoctor = new System.Windows.Forms.ColumnHeader();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.panelActions = new System.Windows.Forms.Panel();
            this.btnViewDoctors = new System.Windows.Forms.Button();
            this.btnBookAppointment = new System.Windows.Forms.Button();
            this.btnMyAppointments = new System.Windows.Forms.Button();
            this.btnMedicalHistory = new System.Windows.Forms.Button();
            this.btnProfile = new System.Windows.Forms.Button();
            this.btnNotifications = new System.Windows.Forms.Button();
            this.btnFeedback = new System.Windows.Forms.Button();

            this.panelHeader.SuspendLayout();
            this.panelWelcome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPatientIcon)).BeginInit();
            this.panelStats.SuspendLayout();
            this.cardNotifications.SuspendLayout();
            this.cardAppointments.SuspendLayout();
            this.cardRecords.SuspendLayout();
            this.panelUpcoming.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Patient Dashboard";
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.BackColor = System.Drawing.Color.White;

            // panelHeader
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(0, 191, 255);
            this.panelHeader.Controls.Add(this.lblWelcome);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1000, 60);

            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(20, 15);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(200, 32);
            this.lblWelcome.Text = "Welcome, Patient!";

            // panelWelcome
            this.panelWelcome.BackColor = System.Drawing.Color.White;
            this.panelWelcome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelWelcome.Controls.Add(this.lblPatientName);
            this.panelWelcome.Controls.Add(this.picPatientIcon);
            this.panelWelcome.Location = new System.Drawing.Point(20, 80);
            this.panelWelcome.Name = "panelWelcome";
            this.panelWelcome.Size = new System.Drawing.Size(300, 100);

            this.picPatientIcon.BackColor = System.Drawing.Color.FromArgb(0, 191, 255);
            this.picPatientIcon.Location = new System.Drawing.Point(20, 20);
            this.picPatientIcon.Name = "picPatientIcon";
            this.picPatientIcon.Size = new System.Drawing.Size(60, 60);

            this.lblPatientName.AutoSize = true;
            this.lblPatientName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblPatientName.ForeColor = System.Drawing.Color.FromArgb(0, 105, 148);
            this.lblPatientName.Location = new System.Drawing.Point(100, 40);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Size = new System.Drawing.Size(94, 25);
            this.lblPatientName.Text = "Loading...";

            // panelStats
            this.panelStats.BackColor = System.Drawing.Color.Transparent;
            this.panelStats.Controls.Add(this.cardNotifications);
            this.panelStats.Controls.Add(this.cardAppointments);
            this.panelStats.Controls.Add(this.cardRecords);
            this.panelStats.Location = new System.Drawing.Point(340, 80);
            this.panelStats.Name = "panelStats";
            this.panelStats.Size = new System.Drawing.Size(640, 100);

            // cardNotifications
            this.cardNotifications.BackColor = System.Drawing.Color.White;
            this.cardNotifications.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardNotifications.Location = new System.Drawing.Point(0, 0);
            this.cardNotifications.Size = new System.Drawing.Size(200, 100);

            this.lblNotificationsTitle.Text = "Notifications";
            this.lblNotificationsTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNotificationsTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblNotificationsTitle.Location = new System.Drawing.Point(10, 15);
            this.lblNotificationsTitle.Size = new System.Drawing.Size(180, 20);
            this.cardNotifications.Controls.Add(this.lblNotificationsTitle);

            this.lblNotifications.Text = "0";
            this.lblNotifications.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblNotifications.ForeColor = System.Drawing.Color.FromArgb(0, 191, 255);
            this.lblNotifications.Location = new System.Drawing.Point(80, 40);
            this.lblNotifications.Size = new System.Drawing.Size(50, 45);
            this.lblNotifications.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cardNotifications.Controls.Add(this.lblNotifications);

            // cardAppointments
            this.cardAppointments.BackColor = System.Drawing.Color.White;
            this.cardAppointments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardAppointments.Location = new System.Drawing.Point(220, 0);
            this.cardAppointments.Size = new System.Drawing.Size(200, 100);

            this.lblAppointmentsTitle.Text = "Appointments";
            this.lblAppointmentsTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAppointmentsTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblAppointmentsTitle.Location = new System.Drawing.Point(10, 15);
            this.lblAppointmentsTitle.Size = new System.Drawing.Size(180, 20);
            this.cardAppointments.Controls.Add(this.lblAppointmentsTitle);

            this.lblTotalAppointments.Text = "0";
            this.lblTotalAppointments.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTotalAppointments.ForeColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.lblTotalAppointments.Location = new System.Drawing.Point(80, 40);
            this.lblTotalAppointments.Size = new System.Drawing.Size(50, 45);
            this.lblTotalAppointments.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cardAppointments.Controls.Add(this.lblTotalAppointments);

            // cardRecords
            this.cardRecords.BackColor = System.Drawing.Color.White;
            this.cardRecords.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardRecords.Location = new System.Drawing.Point(440, 0);
            this.cardRecords.Size = new System.Drawing.Size(200, 100);

            this.lblRecordsTitle.Text = "Medical Records";
            this.lblRecordsTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRecordsTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblRecordsTitle.Location = new System.Drawing.Point(10, 15);
            this.lblRecordsTitle.Size = new System.Drawing.Size(180, 20);
            this.cardRecords.Controls.Add(this.lblRecordsTitle);

            this.lblMedicalRecords.Text = "0";
            this.lblMedicalRecords.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblMedicalRecords.ForeColor = System.Drawing.Color.FromArgb(155, 89, 182);
            this.lblMedicalRecords.Location = new System.Drawing.Point(80, 40);
            this.lblMedicalRecords.Size = new System.Drawing.Size(50, 45);
            this.lblMedicalRecords.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cardRecords.Controls.Add(this.lblMedicalRecords);

            // panelUpcoming
            this.panelUpcoming.BackColor = System.Drawing.Color.White;
            this.panelUpcoming.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelUpcoming.Controls.Add(this.lblUpcomingTitle);
            this.panelUpcoming.Controls.Add(this.lvUpcoming);
            this.panelUpcoming.Location = new System.Drawing.Point(20, 200);
            this.panelUpcoming.Size = new System.Drawing.Size(460, 200);

            this.lblUpcomingTitle.AutoSize = true;
            this.lblUpcomingTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblUpcomingTitle.ForeColor = System.Drawing.Color.FromArgb(0, 105, 148);
            this.lblUpcomingTitle.Location = new System.Drawing.Point(15, 15);
            this.lblUpcomingTitle.Text = "Upcoming Appointments";

            this.lvUpcoming.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colDate,
                this.colDoctor,
                this.colStatus
            });
            this.lvUpcoming.FullRowSelect = true;
            this.lvUpcoming.GridLines = true;
            this.lvUpcoming.Location = new System.Drawing.Point(20, 45);
            this.lvUpcoming.Size = new System.Drawing.Size(420, 140);
            this.lvUpcoming.View = System.Windows.Forms.View.Details;

            this.colDate.Text = "Date";
            this.colDate.Width = 120;
            this.colDoctor.Text = "Doctor";
            this.colDoctor.Width = 180;
            this.colStatus.Text = "Status";
            this.colStatus.Width = 100;

            // panelActions
            this.panelActions.BackColor = System.Drawing.Color.White;
            this.panelActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelActions.Controls.Add(this.btnViewDoctors);
            this.panelActions.Controls.Add(this.btnBookAppointment);
            this.panelActions.Controls.Add(this.btnMyAppointments);
            this.panelActions.Controls.Add(this.btnMedicalHistory);
            this.panelActions.Controls.Add(this.btnProfile);
            this.panelActions.Controls.Add(this.btnNotifications);
            this.panelActions.Controls.Add(this.btnFeedback);
            this.panelActions.Location = new System.Drawing.Point(500, 200);
            this.panelActions.Size = new System.Drawing.Size(480, 200);

            this.btnViewDoctors.BackColor = System.Drawing.Color.FromArgb(0, 191, 255);
            this.btnViewDoctors.FlatAppearance.BorderSize = 0;
            this.btnViewDoctors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewDoctors.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewDoctors.ForeColor = System.Drawing.Color.White;
            this.btnViewDoctors.Location = new System.Drawing.Point(20, 15);
            this.btnViewDoctors.Size = new System.Drawing.Size(140, 40);
            this.btnViewDoctors.Text = "👨‍⚕️ View Doctors";
            this.btnViewDoctors.Click += new System.EventHandler(this.btnViewDoctors_Click);

            this.btnBookAppointment.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnBookAppointment.FlatAppearance.BorderSize = 0;
            this.btnBookAppointment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBookAppointment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBookAppointment.ForeColor = System.Drawing.Color.White;
            this.btnBookAppointment.Location = new System.Drawing.Point(170, 15);
            this.btnBookAppointment.Size = new System.Drawing.Size(140, 40);
            this.btnBookAppointment.Text = "📅 Book Appointment";
            this.btnBookAppointment.Click += new System.EventHandler(this.btnBookAppointment_Click);

            this.btnMyAppointments.BackColor = System.Drawing.Color.FromArgb(155, 89, 182);
            this.btnMyAppointments.FlatAppearance.BorderSize = 0;
            this.btnMyAppointments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMyAppointments.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMyAppointments.ForeColor = System.Drawing.Color.White;
            this.btnMyAppointments.Location = new System.Drawing.Point(320, 15);
            this.btnMyAppointments.Size = new System.Drawing.Size(140, 40);
            this.btnMyAppointments.Text = "📋 My Appointments";
            this.btnMyAppointments.Click += new System.EventHandler(this.btnMyAppointments_Click);

            this.btnMedicalHistory.BackColor = System.Drawing.Color.FromArgb(241, 196, 15);
            this.btnMedicalHistory.FlatAppearance.BorderSize = 0;
            this.btnMedicalHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMedicalHistory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMedicalHistory.ForeColor = System.Drawing.Color.White;
            this.btnMedicalHistory.Location = new System.Drawing.Point(20, 65);
            this.btnMedicalHistory.Size = new System.Drawing.Size(140, 40);
            this.btnMedicalHistory.Text = "📁 Medical History";
            this.btnMedicalHistory.Click += new System.EventHandler(this.btnMedicalHistory_Click);

            this.btnProfile.BackColor = System.Drawing.Color.FromArgb(230, 126, 34);
            this.btnProfile.FlatAppearance.BorderSize = 0;
            this.btnProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProfile.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnProfile.ForeColor = System.Drawing.Color.White;
            this.btnProfile.Location = new System.Drawing.Point(170, 65);
            this.btnProfile.Size = new System.Drawing.Size(140, 40);
            this.btnProfile.Text = "👤 My Profile";
            this.btnProfile.Click += new System.EventHandler(this.btnProfile_Click);

            this.btnNotifications.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnNotifications.FlatAppearance.BorderSize = 0;
            this.btnNotifications.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotifications.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnNotifications.ForeColor = System.Drawing.Color.White;
            this.btnNotifications.Location = new System.Drawing.Point(320, 65);
            this.btnNotifications.Size = new System.Drawing.Size(140, 40);
            this.btnNotifications.Text = "🔔 Notifications";
            this.btnNotifications.Click += new System.EventHandler(this.btnNotifications_Click);

            this.btnFeedback.BackColor = System.Drawing.Color.FromArgb(0, 191, 255);
            this.btnFeedback.FlatAppearance.BorderSize = 0;
            this.btnFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFeedback.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFeedback.ForeColor = System.Drawing.Color.White;
            this.btnFeedback.Location = new System.Drawing.Point(20, 115);
            this.btnFeedback.Size = new System.Drawing.Size(440, 40);
            this.btnFeedback.Text = "⭐ Give Feedback";
            this.btnFeedback.Click += new System.EventHandler(this.btnFeedback_Click);

            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.panelUpcoming);
            this.Controls.Add(this.panelStats);
            this.Controls.Add(this.panelWelcome);
            this.Controls.Add(this.panelHeader);

            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelWelcome.ResumeLayout(false);
            this.panelWelcome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPatientIcon)).EndInit();
            this.panelStats.ResumeLayout(false);
            this.cardNotifications.ResumeLayout(false);
            this.cardAppointments.ResumeLayout(false);
            this.cardRecords.ResumeLayout(false);
            this.panelUpcoming.ResumeLayout(false);
            this.panelUpcoming.PerformLayout();
            this.panelActions.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}