using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;
using ClinicAppointmentSystem.Utils;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcPatientProfile : UserControl
    {
        private int _patientId;
        private Patient _patient;

        public UcPatientProfile(int patientId)
        {
            this._patientId = patientId;
            this._patient = DataManager.Patients.FirstOrDefault(p => p.Id == patientId);
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        private void UcPatientProfile_Load(object sender, EventArgs e)
        {
            InitializeFields();
        }

        private void InitializeFields()
        {
            this.Controls.Clear();
            this.BackColor = PatientTheme.Background;

            // Container for all responsive flow
            FlowLayoutPanel flpMain = new FlowLayoutPanel { 
                Dock = DockStyle.Fill, 
                AutoScroll = true, 
                Padding = new Padding(40), 
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            this.Controls.Add(flpMain);

            // ── HERO SECTION ──
            Panel pnlHero = new Panel { Size = new Size(1000, 180), BackColor = PatientTheme.Surface, Margin = new Padding(0, 0, 0, 40) };
            pnlHero.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                // Draw rounded border
                using (Pen p = new Pen(PatientTheme.Border, 1))
                    e.Graphics.DrawRectangle(p, 0, 0, pnlHero.Width - 1, pnlHero.Height - 1);
                
                // Draw Initial Circle
                using (SolidBrush b = new SolidBrush(PatientTheme.PrimaryLight))
                    e.Graphics.FillEllipse(b, 40, 35, 110, 110);
                
                string initials = "PT";
                if (!string.IsNullOrEmpty(_patient?.FullName)) {
                    var parts = _patient.FullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    initials = parts.Length > 1 ? parts[0].Substring(0,1) + parts[parts.Length-1].Substring(0,1) : parts[0].Substring(0,1);
                }
                
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    e.Graphics.DrawString(initials.ToUpper(), new Font("Segoe UI", 32, FontStyle.Bold), new SolidBrush(PatientTheme.Primary), new Rectangle(40, 35, 110, 110), sf);
                }
            };

            Label lblGreeting = new Label { 
                Text = $"Welcome back, {(_patient?.FullName?.Split(' ').FirstOrDefault() ?? "Patient")}", 
                Font = PatientTheme.TitleLarge, 
                ForeColor = PatientTheme.TextPrimary, 
                Location = new Point(175, 55), 
                AutoSize = true 
            };
            Label lblStatus = new Label { 
                Text = "Verified Active Clinical Member", 
                Font = PatientTheme.Subtitle, 
                ForeColor = PatientTheme.TextSecondary, 
                Location = new Point(180, 100), 
                AutoSize = true 
            };
            pnlHero.Controls.AddRange(new Control[] { lblGreeting, lblStatus });
            flpMain.Controls.Add(pnlHero);

            // ── MAIN CONTENT (TWO COLUMNS) ──
            TableLayoutPanel tlp = new TableLayoutPanel { Size = new Size(1000, 550), ColumnCount = 2, Margin = new Padding(0) };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // LEFT COLUMN : Personal Information
            Panel pnlLeft = new Panel { Dock = DockStyle.Fill, BackColor = PatientTheme.Surface, Margin = new Padding(0, 0, 20, 0), Padding = new Padding(35) };
            pnlLeft.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(PatientTheme.Border, 1), 0, 0, pnlLeft.Width - 1, pnlLeft.Height - 1);
            
            Label lPersonalTitle = new Label { Text = "\u1F464 Personal Information", Font = PatientTheme.LabelBold, ForeColor = PatientTheme.Primary, Location = new Point(35, 25), AutoSize = true };
            pnlLeft.Controls.Add(lPersonalTitle);

            int ly = 80;
            int fw = 380;
            var txtName = AddEditableField(pnlLeft, "FULL NAME", _patient?.FullName ?? "", 35, ref ly, fw);
            var txtPhone = AddEditableField(pnlLeft, "PHONE NUMBER", _patient?.Phone ?? "", 35, ref ly, fw);
            var txtEmail = AddEditableField(pnlLeft, "EMAIL ADDRESS", _patient?.Email ?? "", 35, ref ly, fw);
            var txtGender = AddEditableField(pnlLeft, "GENDER", _patient?.Gender ?? "", 35, ref ly, fw);
            var txtDOB = AddEditableField(pnlLeft, "DATE OF BIRTH", _patient?.DateOfBirth.ToString("yyyy-MM-dd") ?? "", 35, ref ly, fw);
            var txtAddress = AddEditableField(pnlLeft, "RESIDENTIAL ADDRESS", _patient?.Address ?? "", 35, ref ly, fw);
            var txtEmerg = AddEditableField(pnlLeft, "EMERGENCY CONTACT", _patient?.EmergencyContactName ?? "", 35, ref ly, fw);
            tlp.Controls.Add(pnlLeft, 0, 0);

            // RIGHT COLUMN : Medical Summary
            Panel pnlRight = new Panel { Dock = DockStyle.Fill, BackColor = PatientTheme.Surface, Margin = new Padding(20, 0, 0, 0), Padding = new Padding(35) };
            pnlRight.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(PatientTheme.Border, 1), 0, 0, pnlRight.Width - 1, pnlRight.Height - 1);

            Label lMedicalTitle = new Label { Text = "\u2695 Medical Summary", Font = PatientTheme.LabelBold, ForeColor = PatientTheme.Success, Location = new Point(35, 25), AutoSize = true };
            pnlRight.Controls.Add(lMedicalTitle);

            int ry = 80;
            var txtBlood = AddEditableField(pnlRight, "BLOOD GROUP", _patient?.BloodGroup ?? "Unknown", 35, ref ry, fw);
            var txtAllergies = AddEditableField(pnlRight, "ALLERGIES", _patient?.AllergiesOrChronicConditions ?? "None", 35, ref ry, fw, true);
            var txtChronic = AddEditableField(pnlRight, "CHRONIC CONDITIONS", _patient?.ChronicConditions ?? "None", 35, ref ry, fw);
            var txtMeds = AddEditableField(pnlRight, "CURRENT MEDICATIONS", _patient?.CurrentMedications ?? "None", 35, ref ry, fw);
            
            // Last Visit
            var lastVisit = DataManager.Appointments.Where(a => a.PatientId == _patientId && a.AppointmentDate < DateTime.Now && a.Status == "Completed").OrderByDescending(a => a.AppointmentDate).FirstOrDefault();
            string lastVisitTxt = "No past visits found";
            if (lastVisit != null) {
                var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == lastVisit.DoctorId);
                lastVisitTxt = $"{lastVisit.AppointmentDate:dd MMMM yyyy} with Dr. {doc?.FullName ?? "Physician"}";
            }
            AddEditableField(pnlRight, "LAST VISIT", lastVisitTxt, 35, ref ry, fw).ReadOnly = true;
            tlp.Controls.Add(pnlRight, 1, 0);

            flpMain.Controls.Add(tlp);

            // ── SAVE BUTTON ──
            Panel pnlFooter = new Panel { Size = new Size(1000, 120), Margin = new Padding(0, 40, 0, 40) };
            Button btnSave = new Button {
                Text = "Save Changes",
                Size = new Size(320, 60),
                Location = new Point(340, 20),
                BackColor = PatientTheme.Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 12),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => {
                if (_patient != null) {
                    _patient.FullName = txtName.Text;
                    _patient.Phone = txtPhone.Text;
                    _patient.Email = txtEmail.Text;
                    _patient.Gender = txtGender.Text;
                    _patient.Address = txtAddress.Text;
                    _patient.EmergencyContactName = txtEmerg.Text;
                    
                    _patient.BloodGroup = txtBlood.Text;
                    _patient.AllergiesOrChronicConditions = txtAllergies.Text;
                    _patient.ChronicConditions = txtChronic.Text;
                    _patient.CurrentMedications = txtMeds.Text;
                    
                    DataManager.SavePatients();
                    MessageBox.Show("Your health profile has been successfully updated.", "Profile Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    var pd = this.FindForm() as PatientDashboard;
                    pd?.Refresh();
                }
            };
            pnlFooter.Controls.Add(btnSave);
            flpMain.Controls.Add(pnlFooter);
        }

        private TextBox AddEditableField(Panel p, string label, string value, int x, ref int y, int width, bool isAlert = false)
        {
            Label lbl = new Label { Text = label, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = PatientTheme.TextMuted, Location = new Point(x, y), AutoSize = true };
            TextBox txt = new TextBox { 
                Text = value, 
                Font = PatientTheme.BodyRegular, 
                Location = new Point(x, y + 22), 
                Width = width, 
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = PatientTheme.Background
            };
            
            if (isAlert && !string.IsNullOrWhiteSpace(value) && !value.Equals("None", StringComparison.OrdinalIgnoreCase)) {
                txt.ForeColor = PatientTheme.Danger;
                txt.BackColor = PatientTheme.DangerLight;
            } else {
                txt.ForeColor = PatientTheme.TextPrimary;
            }

            p.Controls.Add(lbl);
            p.Controls.Add(txt);
            y += 75; // Increased spacing for more white space
            return txt;
        }
    }
}
