using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace ClinicAppointmentSystem
{
    public partial class AdminDashboard : Form
    {
        private bool isSidebarCollapsed = false;
        private int sidebarExpandedWidth = 250;
        private int sidebarCollapsedWidth = 70;

        // Doctor data storage
        private DataTable doctorsDataTable;
        private List<Doctor> doctorsList;

        // Filter controls - declare as fields to avoid null references
        private ComboBox cmbSpecialization;
        private ComboBox cmbStatus;
        private ComboBox cmbDepartment;
        private TextBox txtSearchFilter;
        private Label lblResultsCount;
        private DataGridView doctorsGrid;

        // Patient management UI fields
        private ComboBox cmbPatientGender;
        private TextBox txtPatientSearch;
        private Label lblPatientResults;
        private DataGridView patientsGrid;

        public AdminDashboard()
        {
            LoadSampleDoctorData(); 
            LoadSamplePatientData();
            LoadSampleAppointmentData(); // Initialize appointments
            InitializeComponent();
            SetupButtonHoverEffects();
            LoadDashboardHome();
        }

        // Doctor class
        public class Doctor
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Specialization { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Status { get; set; }
            public string Department { get; set; }
            public string Experience { get; set; }
            public string Qualification { get; set; }
            public string Schedule { get; set; }
            public string AvailableHours { get; set; }
            public int MaxPatientsPerDay { get; set; }
        }

        // Patient class
        public class Patient
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public DateTime DOB { get; set; }
            public string Gender { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string EmergencyContactName { get; set; }
            public string EmergencyContactPhone { get; set; }
            public string MedicalCondition { get; set; }
            public string SpecialistRequired { get; set; }
            public DateTime AppointmentDate { get; set; }
            public string AppointmentStatus { get; set; }
            public DateTime RegistrationDate { get; set; }
        }

        // Appointment class
        public class Appointment
        {
            public string ID { get; set; }
            public string PatientName { get; set; }
            public string DoctorName { get; set; }
            public string DoctorSpecialization { get; set; }
            public DateTime AppointmentDate { get; set; }
            public string Status { get; set; }
            public string Reason { get; set; }
        }

        private List<Appointment> appointmentsList;

        private void LoadSampleAppointmentData()
        {
            appointmentsList = new List<Appointment>
            {
                new Appointment { ID = "A001", PatientName = "Alice Johnson", DoctorName = "Dr. John Smith", DoctorSpecialization = "Cardiology", AppointmentDate = DateTime.Now.AddDays(1).Date.AddHours(10), Status = "Approved", Reason = "Follow-up" },
                new Appointment { ID = "A002", PatientName = "Robert Williams", DoctorName = "Dr. Robert Wilson", DoctorSpecialization = "Orthopedics", AppointmentDate = DateTime.Now.AddDays(2).Date.AddHours(14), Status = "Rejected", Reason = "Routine Checkup" },
                new Appointment { ID = "A003", PatientName = "Emily Brown", DoctorName = "Dr. Sarah Johnson", DoctorSpecialization = "Pediatrics", AppointmentDate = DateTime.Now.AddDays(3).Date.AddHours(9), Status = "Pending", Reason = "Sudden Symptoms" },
                new Appointment { ID = "A004", PatientName = "David Miller", DoctorName = "Dr. Robert Wilson", DoctorSpecialization = "Orthopedics", AppointmentDate = DateTime.Now.AddDays(4).Date.AddHours(11), Status = "Approved", Reason = "Emergency" },
                new Appointment { ID = "A005", PatientName = "Sophia Davis", DoctorName = "Dr. Michael Brown", DoctorSpecialization = "Dermatology", AppointmentDate = DateTime.Now.AddDays(5).Date.AddHours(15), Status = "Pending", Reason = "Consultation" }
            };
        }

        private List<Patient> patientsList;

        private void LoadSamplePatientData()
        {
            patientsList = new List<Patient>
            {
                new Patient { ID = "P001", Name = "Alice Johnson", Gender = "Female", MedicalCondition = "Hypertension", SpecialistRequired = "Cardiology", AppointmentDate = DateTime.Now.AddDays(1).Date.AddHours(10), AppointmentStatus = "Approved", RegistrationDate = DateTime.Now.AddMonths(-6) },
                new Patient { ID = "P002", Name = "Robert Williams", Gender = "Male", MedicalCondition = "Diabetes Type 2", SpecialistRequired = "Endocrinology", AppointmentDate = DateTime.Now.AddDays(2).Date.AddHours(14), AppointmentStatus = "Rejected", RegistrationDate = DateTime.Now.AddMonths(-4) },
                new Patient { ID = "P003", Name = "Emily Brown", Gender = "Female", MedicalCondition = "Asthma", SpecialistRequired = "Pulmonology", AppointmentDate = DateTime.Now.AddDays(3).Date.AddHours(9), AppointmentStatus = "Pending", RegistrationDate = DateTime.Now.AddMonths(-1) },
                new Patient { ID = "P004", Name = "David Miller", Gender = "Male", MedicalCondition = "Back Pain", SpecialistRequired = "Orthopedics", AppointmentDate = DateTime.Now.AddDays(4).Date.AddHours(11), AppointmentStatus = "Approved", RegistrationDate = DateTime.Now.AddMonths(-2) },
                new Patient { ID = "P005", Name = "Sophia Davis", Gender = "Female", MedicalCondition = "Allergies", SpecialistRequired = "Immunology", AppointmentDate = DateTime.Now.AddDays(5).Date.AddHours(15), AppointmentStatus = "Pending", RegistrationDate = DateTime.Now.AddMonths(-3) }
            };
        }

        private void LoadSampleDoctorData()
        {
            doctorsList = new List<Doctor>
            {
                new Doctor { ID = "D001", Name = "Dr. John Smith", Specialization = "Cardiology", Phone = "555-0101", Email = "john.smith@clinic.com", Status = "Available", Department = "Cardiology", Experience = "15 years", Qualification = "MD, FACC", Schedule = "Mon, Wed, Fri", AvailableHours = "09:00 AM - 05:00 PM", MaxPatientsPerDay = 20 },
                new Doctor { ID = "D002", Name = "Dr. Sarah Johnson", Specialization = "Pediatrics", Phone = "555-0102", Email = "sarah.johnson@clinic.com", Status = "Available", Department = "Pediatrics", Experience = "12 years", Qualification = "MD, FAAP", Schedule = "Tue, Thu, Sat", AvailableHours = "10:00 AM - 06:00 PM", MaxPatientsPerDay = 25 },
                new Doctor { ID = "D003", Name = "Dr. Michael Brown", Specialization = "Dermatology", Phone = "555-0103", Email = "michael.brown@clinic.com", Status = "On Leave", Department = "Dermatology", Experience = "8 years", Qualification = "MD", Schedule = "Mon-Fri", AvailableHours = "08:00 AM - 04:00 PM", MaxPatientsPerDay = 15 },
                new Doctor { ID = "D004", Name = "Dr. Emily Davis", Specialization = "Neurology", Phone = "555-0104", Email = "emily.davis@clinic.com", Status = "Available", Department = "Neurology", Experience = "10 years", Qualification = "MD, PhD", Schedule = "Wed, Fri", AvailableHours = "11:00 AM - 07:00 PM", MaxPatientsPerDay = 10 },
                new Doctor { ID = "D005", Name = "Dr. Robert Wilson", Specialization = "Orthopedics", Phone = "555-0105", Email = "robert.wilson@clinic.com", Status = "Busy", Department = "Orthopedics", Experience = "20 years", Qualification = "MD, FACS", Schedule = "Mon-Thu", AvailableHours = "08:00 AM - 03:00 PM", MaxPatientsPerDay = 30 },
                new Doctor { ID = "D006", Name = "Dr. Lisa Anderson", Specialization = "Ophthalmology", Phone = "555-0106", Email = "lisa.anderson@clinic.com", Status = "Available", Department = "Ophthalmology", Experience = "7 years", Qualification = "MD", Schedule = "Tue-Sat", AvailableHours = "09:00 AM - 05:00 PM", MaxPatientsPerDay = 18 },
                new Doctor { ID = "D007", Name = "Dr. James Taylor", Specialization = "Dentistry", Phone = "555-0107", Email = "james.taylor@clinic.com", Status = "Available", Department = "Dentistry", Experience = "9 years", Qualification = "DDS", Schedule = "Mon-Fri", AvailableHours = "10:00 AM - 06:00 PM", MaxPatientsPerDay = 12 },
                new Doctor { ID = "D008", Name = "Dr. Patricia White", Specialization = "Psychiatry", Phone = "555-0108", Email = "patricia.white@clinic.com", Status = "Busy", Department = "Psychiatry", Experience = "14 years", Qualification = "MD, PhD", Schedule = "Wed-Sun", AvailableHours = "12:00 PM - 08:00 PM", MaxPatientsPerDay = 8 },
                new Doctor { ID = "D009", Name = "Dr. Thomas Lee", Specialization = "Urology", Phone = "555-0109", Email = "thomas.lee@clinic.com", Status = "Available", Department = "Urology", Experience = "11 years", Qualification = "MD", Schedule = "Mon-Thu", AvailableHours = "09:00 AM - 05:00 PM", MaxPatientsPerDay = 15 },
                new Doctor { ID = "D010", Name = "Dr. Maria Garcia", Specialization = "Gynecology", Phone = "555-0110", Email = "maria.garcia@clinic.com", Status = "On Leave", Department = "Gynecology", Experience = "16 years", Qualification = "MD, FACOG", Schedule = "Tue-Fri", AvailableHours = "08:00 AM - 04:00 PM", MaxPatientsPerDay = 20 }
            };
        }

        private void btnToggleSidebar_Click(object sender, EventArgs e)
        {
            isSidebarCollapsed = !isSidebarCollapsed;

            if (isSidebarCollapsed)
            {
                sidebarPanel.Width = sidebarCollapsedWidth;
                btnToggleSidebar.Text = "☰";
                btnToggleSidebar.Location = new Point(15, 15);

                btnDashboard.Text = "     📊";
                btnDoctors.Text = "     👨‍⚕️";
                btnPatients.Text = "     👤";
                btnAppointments.Text = "     📅";
                btnReports.Text = "     📈";
                btnLogout.Text = "     🚪";

                foreach (Button btn in new[] { btnDashboard, btnDoctors, btnPatients,
                                              btnAppointments, btnReports, btnLogout })
                {
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.Padding = new Padding(0);
                    btn.Size = new Size(60, 45);
                }

                foreach (Control ctrl in sidebarPanel.Controls)
                {
                    if (ctrl is Label && ctrl != btnToggleSidebar)
                    {
                        if (ctrl.Text.Contains("CLINIC") || ctrl.Text == "ADMIN" || ctrl.Text == "Version 2.0")
                        {
                            ctrl.Visible = false;
                        }
                    }
                }
            }
            else
            {
                sidebarPanel.Width = sidebarExpandedWidth;
                btnToggleSidebar.Text = "◀";
                btnToggleSidebar.Location = new Point(15, 15);

                btnDashboard.Text = "     📊 DASHBOARD";
                btnDoctors.Text = "     👨‍⚕️ DOCTORS";
                btnPatients.Text = "     👤 PATIENTS";
                btnAppointments.Text = "     📅 APPOINTMENTS";
                btnReports.Text = "     📈 REPORTS";
                btnLogout.Text = "     🚪 LOGOUT";

                foreach (Button btn in new[] { btnDashboard, btnDoctors, btnPatients,
                                              btnAppointments, btnReports, btnLogout })
                {
                    btn.TextAlign = ContentAlignment.MiddleLeft;
                    btn.Padding = new Padding(15, 0, 0, 0);
                    btn.Size = new Size(230, 45);
                }

                foreach (Control ctrl in sidebarPanel.Controls)
                {
                    if (ctrl is Label)
                    {
                        ctrl.Visible = true;
                    }
                }
            }

            UpdateContentPanel();
        }

        private void UpdateContentPanel()
        {
            contentPanel.Location = new Point(sidebarPanel.Width, 60);
            contentPanel.Width = this.ClientSize.Width - sidebarPanel.Width;
            contentPanel.Height = this.ClientSize.Height - 90;

            string currentView = GetCurrentView();
            if (currentView == "DASHBOARD")
                LoadDashboardHome();
            else if (currentView == "DOCTORS")
                LoadDoctorsView();
            else if (currentView == "PATIENTS")
                LoadPatientsView();
            else if (currentView == "APPOINTMENTS")
                LoadAppointmentsView();
            else if (currentView == "REPORTS")
                LoadReportsView();
        }

        private string GetCurrentView()
        {
            if (btnDashboard.BackColor == Color.FromArgb(52, 152, 219))
                return "DASHBOARD";
            if (btnDoctors.BackColor == Color.FromArgb(52, 152, 219))
                return "DOCTORS";
            if (btnPatients.BackColor == Color.FromArgb(52, 152, 219))
                return "PATIENTS";
            if (btnAppointments.BackColor == Color.FromArgb(52, 152, 219))
                return "APPOINTMENTS";
            if (btnReports.BackColor == Color.FromArgb(52, 152, 219))
                return "REPORTS";
            return "DASHBOARD";
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateContentPanel();
        }

        private void SetupButtonHoverEffects()
        {
            Color activeColor = Color.FromArgb(52, 152, 219);
            Color inactiveColor = Color.FromArgb(44, 62, 80);
            Color hoverColor = Color.FromArgb(41, 128, 185);
            Color logoutHoverColor = Color.FromArgb(231, 76, 60);

            btnDashboard.MouseEnter += (s, e) => { if (btnDashboard.BackColor != activeColor) btnDashboard.BackColor = hoverColor; };
            btnDashboard.MouseLeave += (s, e) => { if (btnDashboard.BackColor != activeColor) btnDashboard.BackColor = inactiveColor; };

            btnDoctors.MouseEnter += (s, e) => { if (btnDoctors.BackColor != activeColor) btnDoctors.BackColor = hoverColor; };
            btnDoctors.MouseLeave += (s, e) => { if (btnDoctors.BackColor != activeColor) btnDoctors.BackColor = inactiveColor; };

            btnPatients.MouseEnter += (s, e) => { if (btnPatients.BackColor != activeColor) btnPatients.BackColor = hoverColor; };
            btnPatients.MouseLeave += (s, e) => { if (btnPatients.BackColor != activeColor) btnPatients.BackColor = inactiveColor; };

            btnAppointments.MouseEnter += (s, e) => { if (btnAppointments.BackColor != activeColor) btnAppointments.BackColor = hoverColor; };
            btnAppointments.MouseLeave += (s, e) => { if (btnAppointments.BackColor != activeColor) btnAppointments.BackColor = inactiveColor; };

            btnReports.MouseEnter += (s, e) => { if (btnReports.BackColor != activeColor) btnReports.BackColor = hoverColor; };
            btnReports.MouseLeave += (s, e) => { if (btnReports.BackColor != activeColor) btnReports.BackColor = inactiveColor; };

            btnLogout.MouseEnter += (s, e) => btnLogout.BackColor = logoutHoverColor;
            btnLogout.MouseLeave += (s, e) => btnLogout.BackColor = Color.FromArgb(192, 57, 43);

            btnToggleSidebar.MouseEnter += (s, e) => btnToggleSidebar.BackColor = Color.FromArgb(52, 73, 94);
            btnToggleSidebar.MouseLeave += (s, e) => btnToggleSidebar.BackColor = Color.Transparent;
        }

        private void SidebarButton_Click(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;

            if (clicked == btnLogout) return;

            ResetSidebarButtons();
            clicked.BackColor = Color.FromArgb(52, 152, 219);

            if (clicked == btnDashboard)
                LoadDashboardHome();
            else if (clicked == btnDoctors)
                LoadDoctorsView();
            else if (clicked == btnPatients)
                LoadPatientsView();
            else if (clicked == btnAppointments)
                LoadAppointmentsView();
            else if (clicked == btnReports)
                LoadReportsView();
        }

        private void ResetSidebarButtons()
        {
            btnDashboard.BackColor = Color.FromArgb(44, 62, 80);
            btnDoctors.BackColor = Color.FromArgb(44, 62, 80);
            btnPatients.BackColor = Color.FromArgb(44, 62, 80);
            btnAppointments.BackColor = Color.FromArgb(44, 62, 80);
            btnReports.BackColor = Color.FromArgb(44, 62, 80);
        }

        // ========== DASHBOARD HOME ==========
        private void LoadDashboardHome()
        {
            if (doctorsList == null) return; // Guard against premature resize calls
            contentPanel.Controls.Clear();

            int contentWidth = contentPanel.Width - 40;

            // Welcome Card
            Panel welcomeCard = new Panel
            {
                BackColor = Color.FromArgb(52, 152, 219),
                Location = new Point(20, 20),
                Width = contentWidth,
                Height = 100
            };

            Label lblWelcome = new Label
            {
                Text = "Welcome back, Admin! 👋",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 25),
                Size = new Size(500, 40),
                BackColor = Color.Transparent
            };

            Label lblSubtitle = new Label
            {
                Text = "Manage your clinic efficiently",
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(220, 240, 255),
                Location = new Point(30, 65),
                Size = new Size(300, 20),
                BackColor = Color.Transparent
            };

            Label lblDate = new Label
            {
                Text = DateTime.Now.ToString("dddd, MMMM dd"),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(contentWidth - 250, 35),
                Size = new Size(230, 25),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            welcomeCard.Controls.Add(lblWelcome);
            welcomeCard.Controls.Add(lblSubtitle);
            welcomeCard.Controls.Add(lblDate);
            contentPanel.Controls.Add(welcomeCard);

            // Stats Cards
            int cardWidth = (contentWidth - 30) / 4;
            if (cardWidth < 150) cardWidth = 150;

            Panel statsPanel = new Panel
            {
                Location = new Point(20, 140),
                Width = contentWidth,
                Height = 120
            };

           statsPanel.Controls.Add(CreateStatCard("Total Doctors", doctorsList.Count.ToString(), "👨‍⚕️", Color.FromArgb(52, 152, 219), 0, cardWidth));
            statsPanel.Controls.Add(CreateStatCard("Available", doctorsList.Count(d => d.Status == "Available").ToString(), "✅", Color.FromArgb(46, 204, 113), cardWidth + 10, cardWidth));
            statsPanel.Controls.Add(CreateStatCard("Busy", doctorsList.Count(d => d.Status == "Busy").ToString(), "⏳", Color.FromArgb(155, 89, 182), (cardWidth + 10) * 2, cardWidth));
            statsPanel.Controls.Add(CreateStatCard("On Leave", doctorsList.Count(d => d.Status == "On Leave").ToString(), "🌴", Color.FromArgb(230, 126, 34), (cardWidth + 10) * 3, cardWidth));

            contentPanel.Controls.Add(statsPanel);

            // Recent Appointments
            Label lblRecent = new Label
            {
                Text = "📋 Recent Appointments",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 280),
                Size = new Size(250, 30)
            };
            contentPanel.Controls.Add(lblRecent);

            // Appointments Grid
            DataGridView grid = CreateAppointmentsGrid();
            grid.Location = new Point(20, 320);
            grid.Width = contentWidth;
            grid.Height = 250;

            if (appointmentsList != null)
            {
                foreach (var appt in appointmentsList.Take(5))
                {
                    grid.Rows.Add(appt.ID, appt.PatientName, appt.DoctorName, appt.DoctorSpecialization, appt.AppointmentDate.ToString("ddd, MMM dd HH:mm"), appt.Status);
                }
            }

            contentPanel.Controls.Add(grid);
        }

        private Panel CreateStatCard(string title, string value, string icon, Color color, int x, int width)
        {
            Panel card = new Panel
            {
                BackColor = Color.White,
                Location = new Point(x, 0),
                Size = new Size(width, 100),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 24F),
                Location = new Point(10, 20),
                Size = new Size(40, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(55, 20),
                Width = width - 65,
                Height = 30,
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(55, 50),
                Width = width - 65,
                Height = 20,
                TextAlign = ContentAlignment.MiddleRight
            };

            card.Controls.Add(lblIcon);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);

            return card;
        }

        private DataGridView CreateAppointmentsGrid()
        {
            DataGridView grid = new DataGridView
            {
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                Font = new Font("Segoe UI", 10F),
                RowTemplate = { Height = 40 },
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            grid.Columns.Add("ID", "ID");
            grid.Columns.Add("Patient", "Patient");
            grid.Columns.Add("Doctor", "Doctor");
            grid.Columns.Add("Spec", "Specialization");
            grid.Columns.Add("Date", "Date/Time");
            grid.Columns.Add("Status", "Status");

            grid.Columns[0].FillWeight = 8;
            grid.Columns[1].FillWeight = 18;
            grid.Columns[2].FillWeight = 18;
            grid.Columns[3].FillWeight = 15;
            grid.Columns[4].FillWeight = 22;
            grid.Columns[5].FillWeight = 12;

            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.EnableHeadersVisualStyles = false;

            grid.CellFormatting += (s, ev) => {
                if (grid.Columns[ev.ColumnIndex].Name == "Status" && ev.Value != null) {
                    string stat = ev.Value.ToString();
                    if (stat == "Approved") ev.CellStyle.ForeColor = Color.Green;
                    else if (stat == "Rejected") ev.CellStyle.ForeColor = Color.Red;
                    else if (stat == "Pending") ev.CellStyle.ForeColor = Color.Orange;
                }
            };

            return grid;
        }

        // ========== DOCTORS PAGE WITH FILTERS ==========
        private void LoadDoctorsView()
        {
            if (doctorsList == null) return;
            contentPanel.Controls.Clear();

            int contentWidth = contentPanel.Width - 40;
            int currentY = 20;

            // Header
            Label lblTitle = new Label
            {
                Text = "👨‍⚕️ Doctor Management",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, currentY),
                Size = new Size(350, 40)
            };
            contentPanel.Controls.Add(lblTitle);

            // Add Doctor Button
            Button btnAddDoctor = new Button
            {
                Text = "➕ Add New Doctor",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(contentWidth - 150, currentY + 5),
                Size = new Size(150, 35),
                Cursor = Cursors.Hand
            };
            btnAddDoctor.FlatAppearance.BorderSize = 0;
            btnAddDoctor.Click += BtnAddDoctor_Click;
            contentPanel.Controls.Add(btnAddDoctor);

            currentY += 50;

            // Create Filter Panel and store controls in fields
            CreateFilterControls(contentWidth, currentY);

            currentY += 80;

            // Results count label
            lblResultsCount = new Label
            {
                Text = $"Showing {doctorsList.Count} doctors",
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(20, currentY - 5),
                Size = new Size(200, 20)
            };
            contentPanel.Controls.Add(lblResultsCount);

            // Create Doctors Grid
            doctorsGrid = CreateDoctorsGrid();
            doctorsGrid.Location = new Point(20, currentY + 15);
            doctorsGrid.Width = contentWidth;
            doctorsGrid.Height = contentPanel.Height - currentY - 70;
            doctorsGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            contentPanel.Controls.Add(doctorsGrid);

            // Populate the grid immediately to restore the former state
            FilterDoctors(null, null);
        }

        private void CreateFilterControls(int width, int yPos)
        {
            Panel filterPanel = new Panel
            {
                Location = new Point(20, yPos),
                Width = width,
                Height = 70,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Filter by Specialization
            Label lblSpecialization = new Label
            {
                Text = "Specialization:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(15, 15),
                Size = new Size(100, 20)
            };

            cmbSpecialization = new ComboBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(120, 12),
                Size = new Size(140, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSpecialization.Items.Add("All Specializations");
            cmbSpecialization.Items.AddRange(doctorsList.Select(d => d.Specialization).Distinct().ToArray());
            cmbSpecialization.SelectedIndex = 0;
            cmbSpecialization.SelectedIndexChanged += FilterDoctors;

            // Filter by Status
            Label lblStatus = new Label
            {
                Text = "Status:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(280, 15),
                Size = new Size(50, 20)
            };

            cmbStatus = new ComboBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(335, 12),
                Size = new Size(110, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new string[] { "All Status", "Available", "Busy", "On Leave" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.SelectedIndexChanged += FilterDoctors;

            // Filter by Department
            Label lblDepartment = new Label
            {
                Text = "Department:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(465, 15),
                Size = new Size(80, 20)
            };

            cmbDepartment = new ComboBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(550, 12),
                Size = new Size(140, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDepartment.Items.Add("All Departments");
            cmbDepartment.Items.AddRange(doctorsList.Select(d => d.Department).Distinct().ToArray());
            cmbDepartment.SelectedIndex = 0;
            cmbDepartment.SelectedIndexChanged += FilterDoctors;

            // Search by Name
            Label lblSearch = new Label
            {
                Text = "Search:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(15, 45),
                Size = new Size(60, 20)
            };

            txtSearchFilter = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(80, 42),
                Size = new Size(200, 25),
                Text = "Search by name..."
            };
            txtSearchFilter.Enter += (s, e) => {
                if (txtSearchFilter.Text == "Search by name...")
                {
                    txtSearchFilter.Text = "";
                    txtSearchFilter.ForeColor = Color.Black;
                }
            };
            txtSearchFilter.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearchFilter.Text))
                {
                    txtSearchFilter.Text = "Search by name...";
                    txtSearchFilter.ForeColor = Color.Gray;
                }
            };
            txtSearchFilter.TextChanged += FilterDoctors;

            // Clear Filters Button
            Button btnClear = new Button
            {
                Text = "Clear Filters",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(width - 120, 35),
                Size = new Size(100, 30),
                Cursor = Cursors.Hand
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += (s, e) => {
                cmbSpecialization.SelectedIndex = 0;
                cmbStatus.SelectedIndex = 0;
                cmbDepartment.SelectedIndex = 0;
                txtSearchFilter.Text = "Search by name...";
                txtSearchFilter.ForeColor = Color.Gray;
                FilterDoctors(s, e);
            };

            filterPanel.Controls.AddRange(new Control[] {
                lblSpecialization, cmbSpecialization,
                lblStatus, cmbStatus,
                lblDepartment, cmbDepartment,
                lblSearch, txtSearchFilter,
                btnClear
            });

            contentPanel.Controls.Add(filterPanel);
        }

        private void FilterDoctors(object sender, EventArgs e)
        {
            // Null checks to prevent exceptions
            if (doctorsGrid == null || cmbSpecialization == null || cmbStatus == null ||
                cmbDepartment == null || txtSearchFilter == null || lblResultsCount == null)
                return;

            string specialization = cmbSpecialization.SelectedItem?.ToString();
            string status = cmbStatus.SelectedItem?.ToString();
            string department = cmbDepartment.SelectedItem?.ToString();
            string searchText = txtSearchFilter.Text == "Search by name..." ? "" : txtSearchFilter.Text.ToLower();

            // Apply filters
            var filteredDoctors = doctorsList.AsEnumerable();

            if (!string.IsNullOrEmpty(specialization) && specialization != "All Specializations")
                filteredDoctors = filteredDoctors.Where(d => d.Specialization == specialization);

            if (!string.IsNullOrEmpty(status) && status != "All Status")
                filteredDoctors = filteredDoctors.Where(d => d.Status == status);

            if (!string.IsNullOrEmpty(department) && department != "All Departments")
                filteredDoctors = filteredDoctors.Where(d => d.Department == department);

            if (!string.IsNullOrEmpty(searchText))
                filteredDoctors = filteredDoctors.Where(d => d.Name.ToLower().Contains(searchText));

            // Update grid
            doctorsGrid.Rows.Clear();
            foreach (var doctor in filteredDoctors)
            {
                doctorsGrid.Rows.Add(doctor.ID, doctor.Name, doctor.Specialization,
                                    doctor.Phone, doctor.Email, doctor.Status);
            }

            // Update results count
            lblResultsCount.Text = $"Showing {filteredDoctors.Count()} of {doctorsList.Count} doctors";
        }

        private DataGridView CreateDoctorsGrid()
        {
            DataGridView grid = new DataGridView
            {
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                Font = new Font("Segoe UI", 10F),
                RowTemplate = { Height = 40 },
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            // Columns
            grid.Columns.Add("ID", "ID");
            grid.Columns.Add("Name", "Name");
            grid.Columns.Add("Specialization", "Specialization");
            grid.Columns.Add("Phone", "Phone");
            grid.Columns.Add("Email", "Email");
            grid.Columns.Add("Status", "Status");

            // Column Resizing (Corrected for "Former State")
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.Columns[0].FillWeight = 8;
            grid.Columns[1].FillWeight = 22;
            grid.Columns[2].FillWeight = 18;
            grid.Columns[3].FillWeight = 15;
            grid.Columns[4].FillWeight = 25;
            grid.Columns[5].FillWeight = 12;

            grid.ScrollBars = ScrollBars.Both;

            // Header style
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersHeight = 45;
            grid.EnableHeadersVisualStyles = false;

            // Row style
            grid.DefaultCellStyle.Padding = new Padding(5);
            grid.RowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            grid.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            grid.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Status color formatting
            grid.CellFormatting += (s, e) =>
            {
                if (grid.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
                {
                    string status = e.Value.ToString();
                    if (status == "Available")
                        e.CellStyle.ForeColor = Color.Green;
                    else if (status == "Busy")
                        e.CellStyle.ForeColor = Color.Orange;
                    else if (status == "On Leave")
                        e.CellStyle.ForeColor = Color.Red;
                }
            };

            // Add double-click to view details
            grid.CellDoubleClick += DoctorsGrid_CellDoubleClick;

            // Load data
            foreach (var doctor in doctorsList)
            {
                grid.Rows.Add(doctor.ID, doctor.Name, doctor.Specialization,
                            doctor.Phone, doctor.Email, doctor.Status);
            }

            return grid;
        }

        private void DoctorsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView grid = sender as DataGridView;
            string doctorId = grid.Rows[e.RowIndex].Cells["ID"].Value.ToString();

            var doctor = doctorsList.FirstOrDefault(d => d.ID == doctorId);
            if (doctor != null)
            {
                ShowDoctorDetails(doctor);
            }
        }

        private void ShowDoctorDetails(Doctor doctor)
        {
            Form detailsForm = new Form
            {
                Text = "Doctor Details - " + doctor.Name,
                Size = new Size(500, 480),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            Panel detailsPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(440, 360),
                BackColor = Color.White,
                AutoScroll = true
            };

            int yPos = 10;
            int labelWidth = 140;
            int valueWidth = 280;

            AddDetailRow(detailsPanel, "ID:", doctor.ID, ref yPos, labelWidth, valueWidth);
            AddDetailRow(detailsPanel, "Name:", doctor.Name, ref yPos, labelWidth, valueWidth);
            AddDetailRow(detailsPanel, "Specialization:", doctor.Specialization, ref yPos, labelWidth, valueWidth);
            AddDetailRow(detailsPanel, "Department:", doctor.Department, ref yPos, labelWidth, valueWidth);
            AddDetailRow(detailsPanel, "Phone:", doctor.Phone, ref yPos, labelWidth, valueWidth);
            AddDetailRow(detailsPanel, "Email:", doctor.Email, ref yPos, labelWidth, valueWidth);
            AddDetailRow(detailsPanel, "Status:", doctor.Status, ref yPos, labelWidth, valueWidth);
            AddDetailRow(detailsPanel, "Experience:", doctor.Experience, ref yPos, labelWidth, valueWidth);
            AddDetailRow(detailsPanel, "Qualification:", doctor.Qualification, ref yPos, labelWidth, valueWidth);
            AddDetailRow(detailsPanel, "Schedule:", doctor.Schedule, ref yPos, labelWidth, valueWidth);
            AddDetailRow(detailsPanel, "Available Hours:", doctor.AvailableHours, ref yPos, labelWidth, valueWidth);
            AddDetailRow(detailsPanel, "Daily Capacity:", doctor.MaxPatientsPerDay.ToString() + " patients", ref yPos, labelWidth, valueWidth);

            Button btnClose = new Button
            {
                Text = "Close",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(190, 390),
                Size = new Size(120, 35),
                DialogResult = DialogResult.OK
            };
            btnClose.FlatAppearance.BorderSize = 0;

            detailsForm.Controls.Add(detailsPanel);
            detailsForm.Controls.Add(btnClose);
            detailsForm.ShowDialog(this);
        }

        private void AddDetailRow(Panel panel, string label, string value, ref int yPos, int labelWidth, int valueWidth)
        {
            Label lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(10, yPos),
                Size = new Size(labelWidth, 25)
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(labelWidth + 10, yPos),
                Size = new Size(valueWidth, 25),
                AutoEllipsis = true
            };

            panel.Controls.Add(lblLabel);
            panel.Controls.Add(lblValue);
            yPos += 30;
        }

        private void BtnAddDoctor_Click(object sender, EventArgs e)
        {
            Form addForm = new Form
            {
                Text = "Register New Doctor",
                Size = new Size(520, 720),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            Panel container = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), AutoScroll = true };
            addForm.Controls.Add(container);

            int y = 10;
            var txtName = AddInput(container, "Full Name:", "e.g. Dr. John Carter", ref y);
            var txtSpec = AddInput(container, "Specialization:", "e.g. Cardiology", ref y);
            var txtDept = AddInput(container, "Department:", "e.g. General Medicine", ref y);
            var txtPhone = AddInput(container, "Phone:", "e.g. 555-0199", ref y);
            var txtEmail = AddInput(container, "Email:", "e.g. doctor@clinic.com", ref y);
            var txtExp = AddInput(container, "Experience:", "e.g. 10 Years", ref y);
            var txtQual = AddInput(container, "Qualification:", "e.g. MD, PhD", ref y);
            
            // Days Selection (Checkboxes)
            Label lblDays = new Label { Text = "Schedule Days:", Location = new Point(10, y), Size = new Size(150, 20), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            FlowLayoutPanel daysPanel = new FlowLayoutPanel { Location = new Point(170, y), Size = new Size(300, 60), BackColor = Color.Transparent };
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            List<CheckBox> dayChecks = new List<CheckBox>();
            foreach (var d in days) {
                CheckBox chk = new CheckBox { Text = d, Width = 60, Font = new Font("Segoe UI", 8F) };
                dayChecks.Add(chk); daysPanel.Controls.Add(chk);
            }
            container.Controls.Add(lblDays);
            container.Controls.Add(daysPanel);
            y += 70;

            // Time Interval Selection
            Label lblHours = new Label { Text = "Available Hours:", Location = new Point(10, y), Size = new Size(150, 20), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            DateTimePicker dtpStart = new DateTimePicker { Location = new Point(170, y), Width = 110, Format = DateTimePickerFormat.Time, ShowUpDown = true };
            DateTimePicker dtpEnd = new DateTimePicker { Location = new Point(290, y), Width = 110, Format = DateTimePickerFormat.Time, ShowUpDown = true };
            dtpStart.Value = DateTime.Today.AddHours(9); dtpEnd.Value = DateTime.Today.AddHours(17);
            container.Controls.AddRange(new Control[] { lblHours, dtpStart, dtpEnd });
            y += 40;

            Label lblCap = new Label { Text = "Daily Patient Capacity:", Location = new Point(10, y), Size = new Size(150, 20), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            NumericUpDown numCap = new NumericUpDown { Location = new Point(170, y), Width = 100, Minimum = 1, Maximum = 100, Value = 20 };
            container.Controls.AddRange(new Control[] { lblCap, numCap });
            y += 50;

            Button btnSave = new Button
            {
                Text = "Save Doctor",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(150, y),
                Size = new Size(180, 40),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            container.Controls.Add(btnSave);

            btnSave.Click += (s, ev) => {
                if (string.IsNullOrWhiteSpace(txtName.Text) || txtName.Text.StartsWith("e.g.")) {
                    MessageBox.Show("Please enter a valid full name."); return;
                }
                
                string selectedDays = string.Join(", ", dayChecks.Where(c => c.Checked).Select(c => c.Text));
                if (string.IsNullOrEmpty(selectedDays)) {
                    MessageBox.Show("Please select at least one working day."); return;
                }

                string hours = dtpStart.Value.ToString("hh:mm tt") + " - " + dtpEnd.Value.ToString("hh:mm tt");
                string newId = "D" + (doctorsList.Count + 1).ToString("D3");
                
                doctorsList.Add(new Doctor {
                    ID = newId,
                    Name = txtName.Text,
                    Specialization = txtSpec.Text.StartsWith("e.g.") ? "" : txtSpec.Text,
                    Department = txtDept.Text.StartsWith("e.g.") ? "" : txtDept.Text,
                    Phone = txtPhone.Text.StartsWith("e.g.") ? "" : txtPhone.Text,
                    Email = txtEmail.Text.StartsWith("e.g.") ? "" : txtEmail.Text,
                    Experience = txtExp.Text.StartsWith("e.g.") ? "" : txtExp.Text,
                    Qualification = txtQual.Text.StartsWith("e.g.") ? "" : txtQual.Text,
                    Schedule = selectedDays,
                    AvailableHours = hours,
                    MaxPatientsPerDay = (int)numCap.Value,
                    Status = "Available"
                });
                
                addForm.DialogResult = DialogResult.OK;
                addForm.Close();
                LoadDoctorsView(); // Refresh the grid
            };

            addForm.ShowDialog();
        }

        private TextBox AddInput(Panel p, string label, string placeholder, ref int y)
        {
            Label lbl = new Label { Text = label, Location = new Point(10, y), Size = new Size(150, 20), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            TextBox txt = new TextBox { Location = new Point(170, y), Width = 280, Font = new Font("Segoe UI", 10F), ForeColor = Color.Gray, Text = placeholder };
            
            txt.Enter += (s, e) => {
                if (txt.Text == placeholder) {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };
            txt.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txt.Text)) {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };

            p.Controls.Add(lbl);
            p.Controls.Add(txt);
            y += 35;
            return txt;
        }

        // ========== OTHER VIEWS ==========
        private void LoadPatientsView()
        {
            if (patientsList == null) return;
            contentPanel.Controls.Clear();

            int contentWidth = contentPanel.Width - 40;
            int currentY = 20;

            // Header
            Label lblTitle = new Label
            {
                Text = "👤 Patient Management",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, currentY),
                Size = new Size(350, 40)
            };
            contentPanel.Controls.Add(lblTitle);

            // Add Patient Button
            Button btnAddPatient = new Button
            {
                Text = "➕ Register Patient",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(contentWidth - 170, currentY + 5),
                Size = new Size(170, 35),
                Cursor = Cursors.Hand
            };
            btnAddPatient.FlatAppearance.BorderSize = 0;
            btnAddPatient.Click += BtnAddPatient_Click;
            contentPanel.Controls.Add(btnAddPatient);

            currentY += 50;

            // Filter Panel
            Panel filterPanel = new Panel
            {
                Location = new Point(20, currentY),
                Width = contentWidth,
                Height = 60,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblGender = new Label { Text = "Gender:", Location = new Point(15, 20), Size = new Size(60, 20), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            cmbPatientGender = new ComboBox { Location = new Point(80, 17), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10F) };
            cmbPatientGender.Items.AddRange(new string[] { "All", "Male", "Female" });
            cmbPatientGender.SelectedIndex = 0;
            cmbPatientGender.SelectedIndexChanged += FilterPatients;

            Label lblSearch = new Label { Text = "Search:", Location = new Point(230, 20), Size = new Size(60, 20), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            txtPatientSearch = new TextBox { Location = new Point(300, 17), Width = 250, Font = new Font("Segoe UI", 10F), Text = "Search by name..." , ForeColor = Color.Gray };
            txtPatientSearch.Enter += (s, ev) => { if (txtPatientSearch.Text == "Search by name...") { txtPatientSearch.Text = ""; txtPatientSearch.ForeColor = Color.Black; } };
            txtPatientSearch.Leave += (s, ev) => { if (string.IsNullOrWhiteSpace(txtPatientSearch.Text)) { txtPatientSearch.Text = "Search by name..."; txtPatientSearch.ForeColor = Color.Gray; } };
            txtPatientSearch.TextChanged += FilterPatients;

            filterPanel.Controls.AddRange(new Control[] { lblGender, cmbPatientGender, lblSearch, txtPatientSearch });
            contentPanel.Controls.Add(filterPanel);

            currentY += 70;

            lblPatientResults = new Label { Text = $"Showing {patientsList.Count} patients", Font = new Font("Segoe UI", 9F, FontStyle.Italic), Location = new Point(20, currentY), Size = new Size(200, 20) };
            contentPanel.Controls.Add(lblPatientResults);

            patientsGrid = CreatePatientsGrid();
            patientsGrid.Location = new Point(20, currentY + 25);
            patientsGrid.Width = contentWidth;
            patientsGrid.Height = contentPanel.Height - currentY - 80;
            patientsGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            contentPanel.Controls.Add(patientsGrid);

            FilterPatients(null, null);
        }

        private void LoadAppointmentsView()
        {
            if (appointmentsList == null) return;
            contentPanel.Controls.Clear();
            
            int contentWidth = contentPanel.Width - 40;

            Label lblTitle = new Label
            {
                Text = "📅 Appointment Management",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 20),
                Size = new Size(400, 40)
            };
            contentPanel.Controls.Add(lblTitle);

            Button btnAddAppt = new Button
            {
                Text = "➕ New Appointment",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(contentWidth - 180, 25),
                Size = new Size(180, 35),
                Cursor = Cursors.Hand
            };
            btnAddAppt.FlatAppearance.BorderSize = 0;
            btnAddAppt.Click += BtnAddAppointment_Click;
            contentPanel.Controls.Add(btnAddAppt);

            DataGridView grid = CreateAppointmentsGrid();
            grid.Location = new Point(20, 80);
            grid.Width = contentWidth;
            grid.Height = contentPanel.Height - 120;
            grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            contentPanel.Controls.Add(grid);
            
            // Populate rows
            foreach (var appt in appointmentsList)
            {
                grid.Rows.Add(appt.ID, appt.PatientName, appt.DoctorName, appt.DoctorSpecialization, appt.AppointmentDate.ToString("ddd, MMM dd HH:mm"), appt.Status);
            }
        }



        private void LoadReportsView()
        {
            if (patientsList == null || doctorsList == null || appointmentsList == null) return;
            contentPanel.Controls.Clear();

            // Scrolling Container for all report elements
            Panel scrollContainer = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10) };
            contentPanel.Controls.Add(scrollContainer);

            int contentWidth = contentPanel.Width - 60;
            int currentY = 10;

            // Header
            Label lblTitle = new Label { Text = "📊 Clinical Analytics & Reports", Font = new Font("Segoe UI", 22F, FontStyle.Bold), ForeColor = Color.FromArgb(44, 62, 80), Location = new Point(10, currentY), Size = new Size(500, 45) };
            scrollContainer.Controls.Add(lblTitle);
            currentY += 60;

            // Summary Metric Cards
            int cardWidth = (contentWidth - 40) / 3;
            scrollContainer.Controls.Add(CreateSummaryCard("Total Patients", patientsList.Count.ToString(), "👥", Color.FromArgb(52, 152, 219), 10, cardWidth, currentY));
            scrollContainer.Controls.Add(CreateSummaryCard("Active Doctors", doctorsList.Count.ToString(), "👨‍⚕️", Color.FromArgb(46, 204, 113), 10 + cardWidth + 20, cardWidth, currentY));
            scrollContainer.Controls.Add(CreateSummaryCard("Appointments", appointmentsList.Count.ToString(), "📅", Color.FromArgb(155, 89, 182), 10 + (cardWidth + 20) * 2, cardWidth, currentY));
            currentY += 120;

            // Charts Section - Two side-by-side charts (Native Drawing to avoid reference issues)
            int chartWidth = (contentWidth / 2) - 15;
            
            // 1. Native Pie Chart - Status Breakdown
            Panel pnlPie = new Panel { Location = new Point(10, currentY), Size = new Size(chartWidth, 300), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            Label lblPie = new Label { Text = "Appointment Status Distribution", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Dock = DockStyle.Top, Height = 30, TextAlign = ContentAlignment.MiddleCenter };
            pnlPie.Controls.Add(lblPie);

            int app = appointmentsList.Count(a => a.Status == "Approved");
            int pen = appointmentsList.Count(a => a.Status == "Pending");
            int rej = appointmentsList.Count(a => a.Status == "Rejected");
            int total = app + pen + rej;

            pnlPie.Paint += (s, ev) => {
                Graphics g = ev.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(20, 40, pnlPie.Height - 80, pnlPie.Height - 80);
                
                if (total == 0) return;

                float startAngle = 0;
                float sweepApp = (float)app / total * 360;
                float sweepPen = (float)pen / total * 360;
                float sweepRej = (float)rej / total * 360;

                Brush bApp = new SolidBrush(Color.FromArgb(46, 204, 113));
                Brush bPen = new SolidBrush(Color.FromArgb(241, 196, 15));
                Brush bRej = new SolidBrush(Color.FromArgb(231, 76, 60));

                if (sweepApp > 0) { g.FillPie(bApp, rect, startAngle, sweepApp); startAngle += sweepApp; }
                if (sweepPen > 0) { g.FillPie(bPen, rect, startAngle, sweepPen); startAngle += sweepPen; }
                if (sweepRej > 0) { g.FillPie(bRej, rect, startAngle, sweepRej); }

                // Legend
                int ly = 60;
                void AddLegend(string text, Color c, ref int y) {
                    g.FillRectangle(new SolidBrush(c), pnlPie.Width - 140, y, 15, 15);
                    g.DrawString(text, new Font("Segoe UI", 8F), Brushes.Black, pnlPie.Width - 120, y - 2);
                    y += 25;
                }
                AddLegend($"Approved: {app}", Color.FromArgb(46, 204, 113), ref ly);
                AddLegend($"Pending: {pen}", Color.FromArgb(241, 196, 15), ref ly);
                AddLegend($"Rejected: {rej}", Color.FromArgb(231, 76, 60), ref ly);
            };
            scrollContainer.Controls.Add(pnlPie);

            // 2. Native Bar Chart - Specialization
            Panel pnlBar = new Panel { Location = new Point(10 + chartWidth + 20, currentY), Size = new Size(chartWidth, 300), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            Label lblBarTitle = new Label { Text = "Popular Medical Specialties", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Dock = DockStyle.Top, Height = 30, TextAlign = ContentAlignment.MiddleCenter };
            pnlBar.Controls.Add(lblBarTitle);

            var spGroups = appointmentsList.GroupBy(a => a.DoctorSpecialization)
                                           .Select(g => new { Name = g.Key, Count = g.Count() })
                                           .OrderByDescending(x => x.Count).Take(6).ToList();
            
            int maxC = spGroups.Count > 0 ? spGroups.Max(x => x.Count) : 1;
            int by = 50;
            foreach (var sp in spGroups) {
                Label lblSpName = new Label { Text = sp.Name, Location = new Point(10, by), Size = new Size(110, 20), Font = new Font("Segoe UI", 8F) };
                Panel bar = new Panel { Location = new Point(130, by), Height = 15, BackColor = Color.FromArgb(52, 152, 219), Width = (int)((float)sp.Count / maxC * (chartWidth - 180)) };
                Label lblSpVal = new Label { Text = sp.Count.ToString(), Location = new Point(135 + bar.Width, by), Size = new Size(30, 20), Font = new Font("Segoe UI", 8F, FontStyle.Bold) };
                pnlBar.Controls.AddRange(new Control[] { lblSpName, bar, lblSpVal });
                by += 35;
            }
            scrollContainer.Controls.Add(pnlBar);

            currentY += 320;

            // Details Table
            Label lblTable = new Label { Text = "Recent Appointment Log Summary", Font = new Font("Segoe UI", 12F, FontStyle.Bold), Location = new Point(10, currentY), Size = new Size(400, 30) };
            scrollContainer.Controls.Add(lblTable);
            currentY += 35;

            DataGridView reportGrid = new DataGridView { Location = new Point(10, currentY), Width = contentWidth, Height = 300, BackgroundColor = Color.White, BorderStyle = BorderStyle.FixedSingle, RowHeadersVisible = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            reportGrid.Columns.Add("P", "Patient"); reportGrid.Columns.Add("D", "Doctor"); reportGrid.Columns.Add("S", "Specialty"); reportGrid.Columns.Add("St", "Status");
            foreach (var a in appointmentsList.Take(15)) reportGrid.Rows.Add(a.PatientName, a.DoctorName, a.DoctorSpecialization, a.Status);
            
            scrollContainer.Controls.Add(reportGrid);
            currentY += 350;

            // Empty spacer for scrolling
            Panel spacer = new Panel { Location = new Point(0, currentY), Size = new Size(10, 20) };
            scrollContainer.Controls.Add(spacer);
        }

        private Panel CreateSummaryCard(string title, string value, string icon, Color color, int x, int width, int y)
        {
            Panel card = new Panel { Location = new Point(x, y), Size = new Size(width, 100), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            Label lblIcon = new Label { Text = icon, Font = new Font("Segoe UI", 24F), Location = new Point(10, 15), Size = new Size(50, 70), TextAlign = ContentAlignment.MiddleCenter };
            Label lblVal = new Label { Text = value, Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = color, Location = new Point(60, 15), Size = new Size(width - 70, 40), TextAlign = ContentAlignment.MiddleRight };
            Label lblT = new Label { Text = title, Font = new Font("Segoe UI", 10F), ForeColor = Color.Gray, Location = new Point(60, 55), Size = new Size(width - 70, 25), TextAlign = ContentAlignment.MiddleRight };
            card.Controls.AddRange(new Control[] { lblIcon, lblVal, lblT });
            return card;
        }

        private void AddStatusMetric(Panel parent, string title, string val, Color c, int x, int w)
        {
            Label lblT = new Label { Text = title, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Location = new Point(x, 20), Width = w, TextAlign = ContentAlignment.MiddleCenter };
            Label lblV = new Label { Text = val, Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = c, Location = new Point(x, 45), Width = w, TextAlign = ContentAlignment.MiddleCenter };
            parent.Controls.AddRange(new Control[] { lblT, lblV });
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search patients, doctors...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search patients, doctors...";
                txtSearch.ForeColor = Color.Gray;
            }
        }        private void FilterPatients(object sender, EventArgs e)
        {
            if (patientsGrid == null || cmbPatientGender == null || txtPatientSearch == null || patientsList == null) return;

            string gender = cmbPatientGender.SelectedItem.ToString();
            string search = txtPatientSearch.Text == "Search by name..." ? "" : txtPatientSearch.Text.ToLower();

            var filtered = patientsList.Where(p => 
                (gender == "All" || p.Gender == gender) &&
                (string.IsNullOrEmpty(search) || p.Name.ToLower().Contains(search))
            ).ToList();

            patientsGrid.Rows.Clear();
            foreach (var p in filtered)
            {
                patientsGrid.Rows.Add(p.ID, p.Name, p.Gender, p.SpecialistRequired, p.AppointmentDate.ToString("ddd, MMM dd HH:mm"), p.AppointmentStatus);
            }
            lblPatientResults.Text = $"Showing {filtered.Count} of {patientsList.Count} patients";
        }

        private DataGridView CreatePatientsGrid()
        {
            DataGridView grid = new DataGridView
            {
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                Font = new Font("Segoe UI", 10F),
                RowTemplate = { Height = 40 },
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            grid.Columns.Add("ID", "ID");
            grid.Columns.Add("Name", "Name");
            grid.Columns.Add("Gender", "Gender");
            grid.Columns.Add("Spec", "Specialist");
            grid.Columns.Add("Appt", "Appt. Time");
            grid.Columns.Add("Status", "Status");

            grid.Columns[0].FillWeight = 8;
            grid.Columns[1].FillWeight = 20;
            grid.Columns[2].FillWeight = 10;
            grid.Columns[3].FillWeight = 16;
            grid.Columns[4].FillWeight = 22;
            grid.Columns[5].FillWeight = 12;

            // Header Style
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.EnableHeadersVisualStyles = false;

            // Color formatting for Appointment Status
            grid.CellFormatting += (s, ev) => {
                if (grid.Columns[ev.ColumnIndex].Name == "Status" && ev.Value != null) {
                    string stat = ev.Value.ToString();
                    if (stat == "Approved") ev.CellStyle.ForeColor = Color.Green;
                    else if (stat == "Rejected") ev.CellStyle.ForeColor = Color.Red;
                    else if (stat == "Pending") ev.CellStyle.ForeColor = Color.Orange;
                }
            };

            return grid;
        }

        private void BtnAddPatient_Click(object sender, EventArgs e)
        {
            Form addForm = new Form
            {
                Text = "New Patient Registration",
                Size = new Size(500, 650),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                BackColor = Color.White
            };

            Panel container = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), AutoScroll = true };
            addForm.Controls.Add(container);

            int y = 10;
            var txtPName = AddInput(container, "Full Name:", "e.g. Jane Doe", ref y);
            
            Label lblDOB = new Label { Text = "Date of Birth:", Location = new Point(10, y), Size = new Size(150, 20), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            DateTimePicker dtpDOB = new DateTimePicker { Location = new Point(170, y), Width = 280, Format = DateTimePickerFormat.Short };
            container.Controls.AddRange(new Control[] { lblDOB, dtpDOB });
            y += 35;

            Label lblPGender = new Label { Text = "Gender:", Location = new Point(10, y), Size = new Size(150, 20), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            ComboBox cmbPGender = new ComboBox { Location = new Point(170, y), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPGender.Items.AddRange(new string[] { "Male", "Female" });
            cmbPGender.SelectedIndex = 0;
            container.Controls.AddRange(new Control[] { lblPGender, cmbPGender });
            y += 35;

            var txtPPhone = AddInput(container, "Phone:", "e.g. 555-0100", ref y);
            var txtPEmail = AddInput(container, "Email:", "e.g. jane@mail.com", ref y);
            var txtPAddr = AddInput(container, "Address:", "Full Address", ref y);
            var txtPCase = AddInput(container, "Disease / Condition:", "e.g. Hypertension", ref y);
            
            Label lblPSpec = new Label { Text = "Required Specialist:", Location = new Point(10, y), Size = new Size(150, 20), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            ComboBox cmbPSpec = new ComboBox { Location = new Point(170, y), Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPSpec.Items.AddRange(new string[] { "Cardiology", "Pediatrics", "Dermatology", "Neurology", "Orthopedics", "Ophthalmology", "Dentistry", "Urology", "Psychiatry" });
            cmbPSpec.SelectedIndex = 0;
            container.Controls.AddRange(new Control[] { lblPSpec, cmbPSpec });
            y += 35;

            var txtPEName = AddInput(container, "Emergency Contact:", "Name", ref y);
            var txtPEPhone = AddInput(container, "Contact Phone:", "Phone Number", ref y);

            Button btnSaveP = new Button { Text = "Register Patient", Font = new Font("Segoe UI", 11F, FontStyle.Bold), BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(150, y + 20), Size = new Size(200, 40), Cursor = Cursors.Hand };
            btnSaveP.FlatAppearance.BorderSize = 0;
            container.Controls.Add(btnSaveP);

            btnSaveP.Click += (s, ev) => {
                if (string.IsNullOrWhiteSpace(txtPName.Text) || txtPName.Text.StartsWith("e.g.")) { MessageBox.Show("Please enter a valid name."); return; }
                
                string newPId = "P" + (patientsList.Count + 1).ToString("D3");
                patientsList.Add(new Patient {
                    ID = newPId,
                    Name = txtPName.Text,
                    DOB = dtpDOB.Value,
                    Gender = cmbPGender.SelectedItem.ToString(),
                    Phone = txtPPhone.Text,
                    Email = txtPEmail.Text,
                    Address = txtPAddr.Text,
                    MedicalCondition = txtPCase.Text.Contains("e.g.") ? "" : txtPCase.Text,
                    SpecialistRequired = cmbPSpec.SelectedItem.ToString(),
                    EmergencyContactName = txtPEName.Text,
                    EmergencyContactPhone = txtPEPhone.Text,
                    RegistrationDate = DateTime.Now,
                    AppointmentDate = DateTime.Now.AddDays(7), // Default placeholder date
                    AppointmentStatus = "Pending"
                });

                addForm.DialogResult = DialogResult.OK;
                addForm.Close();
                LoadPatientsView();
            };

            addForm.ShowDialog();
        }


        private void BtnAddAppointment_Click(object sender, EventArgs e)
        {
            Form bookForm = new Form { Text = "Book Appointment", Size = new Size(500, 650), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, BackColor = Color.White };
            Panel container = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            bookForm.Controls.Add(container);

            int y = 10;
            Label lblP = new Label { Text = "Select Patient:", Location = new Point(10, y), Size = new Size(150, 20), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            ComboBox cmbP = new ComboBox { Location = new Point(170, y), Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbP.Items.AddRange(patientsList.Select(p => p.Name).ToArray());
            if (cmbP.Items.Count > 0) cmbP.SelectedIndex = 0;
            container.Controls.AddRange(new Control[] { lblP, cmbP });
            y += 40;

            Label lblDate = new Label { Text = "Appointment Date:", Location = new Point(10, y), Size = new Size(150, 20), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            DateTimePicker dtp = new DateTimePicker { Location = new Point(170, y), Width = 280, Format = DateTimePickerFormat.Short };
            container.Controls.AddRange(new Control[] { lblDate, dtp });
            y += 40;

            Label lblDoc = new Label { Text = "Select Doctor:", Location = new Point(10, y), Size = new Size(150, 20), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            ComboBox cmbDoc = new ComboBox { Location = new Point(170, y), Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            container.Controls.AddRange(new Control[] { lblDoc, cmbDoc });
            y += 40;

            Label lblInfo = new Label { Text = "Availability Info", Location = new Point(10, y), Size = new Size(450, 60), ForeColor = Color.FromArgb(52, 73, 94), Font = new Font("Segoe UI", 9F, FontStyle.Italic) };
            container.Controls.Add(lblInfo);
            y += 70;

            var txtReason = AddInput(container, "Reason:", "e.g. Health Checkup", ref y);

            Button btnCheck = new Button { Text = "Check Availability", Font = new Font("Segoe UI", 10F, FontStyle.Bold), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(150, y + 20), Size = new Size(180, 35), Cursor = Cursors.Hand };
            Button btnBook = new Button { Text = "Book Appointment", Font = new Font("Segoe UI", 11F, FontStyle.Bold), BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(150, y + 65), Size = new Size(180, 40), Enabled = false, Cursor = Cursors.Hand };
            container.Controls.AddRange(new Control[] { btnCheck, btnBook });

            void UpdateDoctors() {
                cmbDoc.Items.Clear();
                string day = dtp.Value.ToString("ddd"); // Mon, Tue, etc.
                var availableDocs = doctorsList.Where(d => d.Schedule.Contains(day)).ToList();
                // Add Name (Specialization) to the dropdown
                cmbDoc.Items.AddRange(availableDocs.Select(d => $"{d.Name} ({d.Specialization})").ToArray());
                
                if (cmbDoc.Items.Count > 0) {
                    cmbDoc.SelectedIndex = 0;
                    lblInfo.Text = "Pick a doctor and check availability.";
                    lblInfo.ForeColor = Color.FromArgb(52, 73, 94);
                } else {
                    lblInfo.Text = "No doctors available on this day.";
                    lblInfo.ForeColor = Color.Red;
                }
                btnBook.Enabled = false;
            }

            dtp.ValueChanged += (s, ev) => UpdateDoctors();
            UpdateDoctors();

            btnCheck.Click += (s, ev) => {
                if (cmbDoc.SelectedItem == null) return;
                
                // Extract doctor name from the "Dr. Name (Spec)" format
                string selectedText = cmbDoc.SelectedItem.ToString();
                string docName = selectedText.Split('(')[0].Trim();
                
                var doc = doctorsList.First(d => d.Name == docName);
                int currentPatients = appointmentsList.Count(a => a.DoctorName == docName && a.AppointmentDate.Date == dtp.Value.Date);
                
                lblInfo.Text = $"Doctor: {docName}\nSpecialty: {doc.Specialization}\nStatus: {currentPatients}/{doc.MaxPatientsPerDay} patients booked.";
                
                if (currentPatients < doc.MaxPatientsPerDay) {
                    btnBook.Enabled = true; lblInfo.ForeColor = Color.Green;
                } else {
                    btnBook.Enabled = false; lblInfo.ForeColor = Color.Red;
                    lblInfo.Text += "\nCapacity full - Please pick another doctor/day.";
                }
            };

            btnBook.Click += (s, ev) => {
                string selectedText = cmbDoc.SelectedItem.ToString();
                string docName = selectedText.Split('(')[0].Trim();
                var selectedDoc = doctorsList.First(d => d.Name == docName);

                appointmentsList.Add(new Appointment {
                    ID = "A" + (appointmentsList.Count + 1).ToString("D3"),
                    PatientName = cmbP.SelectedItem.ToString(),
                    DoctorName = docName,
                    DoctorSpecialization = selectedDoc.Specialization,
                    AppointmentDate = dtp.Value.Date.AddHours(10),
                    Status = "Approved",
                    Reason = txtReason.Text.Contains("e.g.") ? "Checkup" : txtReason.Text
                });
                bookForm.DialogResult = DialogResult.OK;
                bookForm.Close();
                LoadAppointmentsView();
            };

            bookForm.ShowDialog();
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
