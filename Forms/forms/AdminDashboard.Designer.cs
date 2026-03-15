using System.Windows.Forms;
using System.Drawing;

namespace ClinicAppointmentSystem
{
    partial class AdminDashboard
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelStats;
        private Panel cardPatients;
        private Panel cardDoctors;
        private Panel cardAppointments;
        private Panel cardToday;
        private Label lblTotalPatients;
        private Label lblTotalDoctors;
        private Label lblTotalAppointments;
        private Label lblTodayAppointments;
        private Label lblPatientsIcon;
        private Label lblDoctorsIcon;
        private Label lblAppointmentsIcon;
        private Label lblTodayIcon;
        private ListView lvRecentAppointments;
        private ColumnHeader colId;
        private ColumnHeader colPatient;
        private ColumnHeader colDoctor;
        private ColumnHeader colDateTime;
        private ColumnHeader colStatus;

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
            this.panelStats = new Panel();
            this.cardPatients = new Panel();
            this.cardDoctors = new Panel();
            this.cardAppointments = new Panel();
            this.cardToday = new Panel();
            this.lblTotalPatients = new Label();
            this.lblTotalDoctors = new Label();
            this.lblTotalAppointments = new Label();
            this.lblTodayAppointments = new Label();
            this.lblPatientsIcon = new Label();
            this.lblDoctorsIcon = new Label();
            this.lblAppointmentsIcon = new Label();
            this.lblTodayIcon = new Label();
            this.lvRecentAppointments = new ListView();
            this.colId = new ColumnHeader();
            this.colPatient = new ColumnHeader();
            this.colDoctor = new ColumnHeader();
            this.colDateTime = new ColumnHeader();
            this.colStatus = new ColumnHeader();

            this.SuspendLayout();

            // Form Properties
            this.Text = "Admin Dashboard";
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // panelStats
            this.panelStats.Location = new System.Drawing.Point(20, 20);
            this.panelStats.Size = new System.Drawing.Size(1160, 150);

            // cardPatients
            this.cardPatients.BackColor = Color.White;
            this.cardPatients.BorderStyle = BorderStyle.FixedSingle;
            this.cardPatients.Location = new System.Drawing.Point(0, 0);
            this.cardPatients.Size = new System.Drawing.Size(280, 150);

            this.lblPatientsIcon.Text = "👥";
            this.lblPatientsIcon.Font = new Font("Segoe UI", 24F);
            this.lblPatientsIcon.ForeColor = Color.FromArgb(52, 152, 219);
            this.lblPatientsIcon.Location = new System.Drawing.Point(20, 20);
            this.lblPatientsIcon.Size = new System.Drawing.Size(60, 50);
            this.cardPatients.Controls.Add(this.lblPatientsIcon);

            this.lblTotalPatients.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblTotalPatients.ForeColor = Color.FromArgb(52, 152, 219);
            this.lblTotalPatients.Location = new System.Drawing.Point(90, 30);
            this.lblTotalPatients.Size = new System.Drawing.Size(80, 40);
            this.lblTotalPatients.Text = "0";
            this.cardPatients.Controls.Add(this.lblTotalPatients);

            Label lblPatientsTitle = new Label();
            lblPatientsTitle.Text = "Total Patients";
            lblPatientsTitle.Location = new System.Drawing.Point(90, 70);
            lblPatientsTitle.Size = new System.Drawing.Size(100, 20);
            this.cardPatients.Controls.Add(lblPatientsTitle);

            this.panelStats.Controls.Add(this.cardPatients);

            // cardDoctors
            this.cardDoctors.BackColor = Color.White;
            this.cardDoctors.BorderStyle = BorderStyle.FixedSingle;
            this.cardDoctors.Location = new System.Drawing.Point(300, 0);
            this.cardDoctors.Size = new System.Drawing.Size(280, 150);

            this.lblDoctorsIcon.Text = "👨‍⚕️";
            this.lblDoctorsIcon.Font = new Font("Segoe UI", 24F);
            this.lblDoctorsIcon.ForeColor = Color.FromArgb(46, 204, 113);
            this.lblDoctorsIcon.Location = new System.Drawing.Point(20, 20);
            this.lblDoctorsIcon.Size = new System.Drawing.Size(60, 50);
            this.cardDoctors.Controls.Add(this.lblDoctorsIcon);

            this.lblTotalDoctors.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblTotalDoctors.ForeColor = Color.FromArgb(46, 204, 113);
            this.lblTotalDoctors.Location = new System.Drawing.Point(90, 30);
            this.lblTotalDoctors.Size = new System.Drawing.Size(80, 40);
            this.lblTotalDoctors.Text = "0";
            this.cardDoctors.Controls.Add(this.lblTotalDoctors);

            Label lblDoctorsTitle = new Label();
            lblDoctorsTitle.Text = "Total Doctors";
            lblDoctorsTitle.Location = new System.Drawing.Point(90, 70);
            lblDoctorsTitle.Size = new System.Drawing.Size(100, 20);
            this.cardDoctors.Controls.Add(lblDoctorsTitle);

            this.panelStats.Controls.Add(this.cardDoctors);

            // cardAppointments
            this.cardAppointments.BackColor = Color.White;
            this.cardAppointments.BorderStyle = BorderStyle.FixedSingle;
            this.cardAppointments.Location = new System.Drawing.Point(600, 0);
            this.cardAppointments.Size = new System.Drawing.Size(280, 150);

            this.lblAppointmentsIcon.Text = "📅";
            this.lblAppointmentsIcon.Font = new Font("Segoe UI", 24F);
            this.lblAppointmentsIcon.ForeColor = Color.FromArgb(155, 89, 182);
            this.lblAppointmentsIcon.Location = new System.Drawing.Point(20, 20);
            this.lblAppointmentsIcon.Size = new System.Drawing.Size(60, 50);
            this.cardAppointments.Controls.Add(this.lblAppointmentsIcon);

            this.lblTotalAppointments.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblTotalAppointments.ForeColor = Color.FromArgb(155, 89, 182);
            this.lblTotalAppointments.Location = new System.Drawing.Point(90, 30);
            this.lblTotalAppointments.Size = new System.Drawing.Size(80, 40);
            this.lblTotalAppointments.Text = "0";
            this.cardAppointments.Controls.Add(this.lblTotalAppointments);

            Label lblAppointmentsTitle = new Label();
            lblAppointmentsTitle.Text = "Total Appointments";
            lblAppointmentsTitle.Location = new System.Drawing.Point(90, 70);
            lblAppointmentsTitle.Size = new System.Drawing.Size(120, 20);
            this.cardAppointments.Controls.Add(lblAppointmentsTitle);

            this.panelStats.Controls.Add(this.cardAppointments);

            // cardToday
            this.cardToday.BackColor = Color.White;
            this.cardToday.BorderStyle = BorderStyle.FixedSingle;
            this.cardToday.Location = new System.Drawing.Point(900, 0);
            this.cardToday.Size = new System.Drawing.Size(260, 150);

            this.lblTodayIcon.Text = "📌";
            this.lblTodayIcon.Font = new Font("Segoe UI", 24F);
            this.lblTodayIcon.ForeColor = Color.FromArgb(241, 196, 15);
            this.lblTodayIcon.Location = new System.Drawing.Point(20, 20);
            this.lblTodayIcon.Size = new System.Drawing.Size(60, 50);
            this.cardToday.Controls.Add(this.lblTodayIcon);

            this.lblTodayAppointments.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblTodayAppointments.ForeColor = Color.FromArgb(241, 196, 15);
            this.lblTodayAppointments.Location = new System.Drawing.Point(90, 30);
            this.lblTodayAppointments.Size = new System.Drawing.Size(80, 40);
            this.lblTodayAppointments.Text = "0";
            this.cardToday.Controls.Add(this.lblTodayAppointments);

            Label lblTodayTitle = new Label();
            lblTodayTitle.Text = "Today's Appointments";
            lblTodayTitle.Location = new System.Drawing.Point(90, 70);
            lblTodayTitle.Size = new System.Drawing.Size(140, 20);
            this.cardToday.Controls.Add(lblTodayTitle);

            this.panelStats.Controls.Add(this.cardToday);

            // Recent Appointments ListView
            this.lvRecentAppointments.Columns.AddRange(new ColumnHeader[] {
                this.colId,
                this.colPatient,
                this.colDoctor,
                this.colDateTime,
                this.colStatus
            });
            this.lvRecentAppointments.FullRowSelect = true;
            this.lvRecentAppointments.GridLines = true;
            this.lvRecentAppointments.Location = new System.Drawing.Point(20, 200);
            this.lvRecentAppointments.Size = new System.Drawing.Size(1160, 450);
            this.lvRecentAppointments.View = View.Details;

            this.colId.Text = "ID";
            this.colId.Width = 50;
            this.colPatient.Text = "Patient";
            this.colPatient.Width = 250;
            this.colDoctor.Text = "Doctor";
            this.colDoctor.Width = 250;
            this.colDateTime.Text = "Date/Time";
            this.colDateTime.Width = 250;
            this.colStatus.Text = "Status";
            this.colStatus.Width = 150;

            // Add controls
            this.Controls.Add(this.panelStats);
            this.Controls.Add(this.lvRecentAppointments);

            this.ResumeLayout(false);
        }
    }
}