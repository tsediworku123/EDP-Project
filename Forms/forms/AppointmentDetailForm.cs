using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public class AppointmentDetailForm : Form
    {
        private Appointment _appointment;
        private Patient _patient;
        private Doctor _doctor;

        public AppointmentDetailForm(int appointmentId)
        {
            this._appointment = DataManager.Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (_appointment != null)
            {
                this._patient = DataManager.Patients.FirstOrDefault(p => p.Id == _appointment.PatientId);
                this._doctor = DataManager.Doctors.FirstOrDefault(d => d.Id == _appointment.DoctorId);
            }
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Appointment Details";
            this.Size = new Size(500, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            if (_appointment == null)
            {
                Label lblError = new Label { Text = "Appointment not found.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
                this.Controls.Add(lblError);
                return;
            }

            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.FromArgb(248, 250, 252) };
            Label lblTitle = new Label { 
                Text = "APPOINTMENT DETAILS", 
                Font = new Font("Segoe UI", 16, FontStyle.Bold), 
                ForeColor = Color.FromArgb(15, 23, 42), 
                TextAlign = ContentAlignment.MiddleCenter, 
                Dock = DockStyle.Fill 
            };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            Panel pnlContent = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30) };
            this.Controls.Add(pnlContent);

            int y = 20;
            AddDetail(pnlContent, "DATE & TIME", _appointment.AppointmentDate.ToString("MMMM dd, yyyy @ HH:mm"), ref y);
            AddDetail(pnlContent, "DOCTOR", $"Dr. {_doctor?.FullName ?? "Unknown"} ({_doctor?.Specialty ?? "General"})", ref y);
            AddDetail(pnlContent, "STATUS", _appointment.Status.ToUpper(), ref y, GetStatusColor(_appointment.Status));
            AddDetail(pnlContent, "REASON FOR VISIT", _appointment.Reason, ref y);
            
            if (!string.IsNullOrEmpty(_appointment.Diagnosis))
            {
                AddDetail(pnlContent, "DIAGNOSIS / NOTES", _appointment.Diagnosis, ref y);
            }

            y += 20;
            Button btnPrint = new Button {
                Text = " PRINT APPOINTMENT SLIP",
                Location = new Point(30, y),
                Size = new Size(420, 60),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += (s, e) => MessageBox.Show("Appointment slip sent to printer.", "Printing...");
            pnlContent.Controls.Add(btnPrint);

            y += 80;
            Button btnClose = new Button {
                Text = "CLOSE",
                Location = new Point(30, y),
                Size = new Size(420, 50),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.Click += (s, e) => this.Close();
            pnlContent.Controls.Add(btnClose);
        }

        private void AddDetail(Panel p, string label, string value, ref int y, Color? valColor = null)
        {
            Label lbl = new Label { Text = label, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(30, y), AutoSize = true };
            Label val = new Label { 
                Text = value, 
                Font = new Font("Segoe UI", 11), 
                ForeColor = valColor ?? Color.FromArgb(30, 41, 59), 
                Location = new Point(30, y + 25), 
                AutoSize = true, 
                MaximumSize = new Size(420, 0) 
            };
            p.Controls.Add(lbl);
            p.Controls.Add(val);
            y += (val.Height > 30 ? val.Height + 50 : 70);
        }

        private Color GetStatusColor(string status)
        {
            switch (status.ToLower())
            {
                case "scheduled": return Color.FromArgb(59, 130, 246);
                case "completed": return Color.FromArgb(16, 185, 129);
                case "cancelled": return Color.FromArgb(239, 68, 68);
                default: return Color.Gray;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) { this.Close(); return true; }
            if (keyData == (Keys.Control | Keys.P)) { MessageBox.Show("Printing..."); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
