using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Controls;

namespace ClinicAppointmentSystem
{
    public partial class DoctorDashboard : Form
    {
        private int _doctorId;
        private Doctor _currentDoctor;

        private bool isSidebarCollapsed = false;
        private int sidebarExpandedWidth = 240;
        private int sidebarCollapsedWidth = 70;

        public DoctorDashboard(int doctorId)
        {
            this._doctorId = doctorId;
            this._currentDoctor = DataManager.Doctors.FirstOrDefault(d => d.Id == doctorId);
            
            InitializeComponent();
            InitSidebarButtonTags();
            SetupDoctorData();
            SetupButtonHoverEffects();
            
            // Set Dashboard Home as default
            LoadHome();

            // Clock Timer
            Timer t = new Timer { Interval = 1000 };
            t.Tick += (s, e) => {
                if (!this.IsDisposed && lblClock != null)
                    lblClock.Text = DateTime.Now.ToString("dddd, MMM dd | hh:mm tt");
                
                int n = DataManager.Notifications.Count(nt => nt.DoctorId == _doctorId && !nt.IsRead);
                lblNotifications.Text = $" {n}";
            };
            t.Start();
        }

        private void SetupDoctorData()
        {
            if (_currentDoctor == null) return;

            lblDocName.Text = $"Dr. {_currentDoctor.FullName}";
            btnStatus.Text = _currentDoctor.CurrentStatus == "On Leave" ? "🔴 On Leave" : "🟢 Available";

            double avgRating = DataManager.Appointments.Where(a => a.DoctorId == _doctorId && a.PatientRating > 0).Select(a => a.PatientRating).DefaultIfEmpty(0).Average();
            int reviews = DataManager.Appointments.Count(a => a.DoctorId == _doctorId && a.PatientRating > 0);
            lblRating.Text = $"⭐ {avgRating:F1} ({reviews} reviews)";
        }

        private void InitSidebarButtonTags()
        {
            foreach (Control ctrl in sidebarPanel.Controls) {
                if (ctrl is Button b && b != btnToggleSidebar) {
                    string full = b.Text;
                    string icon = "";
                    string trimmed = full.TrimStart();
                    if (trimmed.Length >= 4) {
                        // Icon is the middle part in "  ICON  Label"
                        icon = trimmed.Substring(0, 1);
                    }
                    b.Tag = new string[] { full, icon };
                }
            }
        }

        private void SetupButtonHoverEffects()
        {
            Color hoverColor = Color.FromArgb(51, 65, 85); // Slate 700
            Color activeColor = Color.FromArgb(59, 130, 246); // Blue 500

            Action<Button> apply = (b) => {
                b.MouseEnter += (s, ev) => { if (b.BackColor != activeColor) b.BackColor = hoverColor; };
                b.MouseLeave += (s, ev) => { if (b.BackColor != activeColor) b.BackColor = Color.Transparent; };
            };

            apply(btnHome); apply(btnAppointments); apply(btnQueue); apply(btnNotes);
            apply(btnReviews); apply(btnAvailability); apply(btnSecurity);
        }

        private void btnToggleSidebar_Click(object sender, EventArgs e)
        {
            isSidebarCollapsed = !isSidebarCollapsed;
            
            this.SuspendLayout();
            sidebarPanel.Width = isSidebarCollapsed ? sidebarCollapsedWidth : sidebarExpandedWidth;
            btnToggleSidebar.Text = isSidebarCollapsed ? "\u25B6" : "\u2630";

            foreach (Control ctrl in sidebarPanel.Controls) {
                if (ctrl is Button b && b != btnToggleSidebar) {
                    b.Width = sidebarPanel.Width - 10;
                    if (b.Tag is string[] tags && tags.Length >= 2) {
                        if (isSidebarCollapsed) {
                            b.Text = tags[1]; 
                            b.TextAlign = ContentAlignment.MiddleCenter;
                            b.Padding = new Padding(0); 
                            b.Font = new Font("Segoe UI Semibold", 15); // Larger font for icons
                        } else {
                            b.Text = tags[0]; 
                            b.TextAlign = ContentAlignment.MiddleLeft;
                            b.Padding = new Padding(10, 0, 0, 0); 
                            b.Font = new Font("Segoe UI Semibold", 9); // Restore text font
                        }
                    }
                }
            }
            this.ResumeLayout();
        }

        private void SidebarButton_Click(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            if (clicked == btnLogout) { Logout_Click(sender, e); return; }

            ResetSidebarButtons();
            clicked.BackColor = Color.FromArgb(59, 130, 246); 

            if (clicked == btnHome) LoadHome();
            else if (clicked == btnAppointments) LoadTodayPatients();
            else if (clicked == btnQueue) LoadQueue();
            else if (clicked == btnNotes) LoadNotes();
            else if (clicked == btnReviews) LoadFeedback();
            else if (clicked == btnAvailability) LoadAvailability();
            else if (clicked == btnSecurity) ShowControl(new UcSecurityManager());
        }

        private void ResetSidebarButtons()
        {
            foreach (Control ctrl in sidebarPanel.Controls) {
                if (ctrl is Button b && b != btnLogout && b != btnToggleSidebar) 
                    b.BackColor = Color.Transparent;
            }
        }

        public void ShowControl(Control uc)
        {
            contentPanel.SuspendLayout();
            foreach (Control c in contentPanel.Controls) {
                c.Dispose();
            }
            contentPanel.Controls.Clear();
            
            uc.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(uc);
            contentPanel.ResumeLayout();
        }

        // Routing Handlers
        public void LoadHome() => ShowControl(new UcDoctorHome(_doctorId));
        public void LoadTodayPatients() => ShowControl(new UcDoctorAppointments(_doctorId));
        public void LoadQueue() => ShowControl(new UcPatientQueue(_doctorId));
        public void LoadNotes() => ShowControl(new UcDoctorHistory(_doctorId));
        public void LoadFeedback() => ShowControl(new UcDoctorFeedback(_doctorId));
        public void LoadAvailability() => ShowControl(new UcDoctorAvailability(_doctorId));
        public void OpenConsultation(int apptId) => ShowControl(new UcConsultationPanel(apptId));

        private void btnStatus_Click(object sender, EventArgs e)
        {
            if (_currentDoctor == null) return;
            _currentDoctor.CurrentStatus = _currentDoctor.CurrentStatus == "On Leave" ? "Available" : "On Leave";
            btnStatus.Text = _currentDoctor.CurrentStatus == "On Leave" ? "🔴 On Leave" : "🟢 Available";
            DataManager.SaveDoctors();
        }

        public void Logout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("End your clinical session?", "Security Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.BeginInvoke(new Action(() => Program.MainForm.ForceLogout(false)));
            }
        }
    }
}
