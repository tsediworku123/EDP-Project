using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public class RateDoctorForm : Form
    {
        private Appointment _appointment;
        private int _selectedRating = 0;
        private Button[] _starButtons = new Button[5];
        private TextBox txtComment;
        private Label lblDoctorInfo;

        public RateDoctorForm(Appointment appointment)
        {
            _appointment = appointment;
            InitializeUI();
            this.Text = "Rate Doctor";
            this.Size = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void InitializeUI()
        {
            this.BackColor = Color.White;
            
            Panel header = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(155, 89, 182) };
            Label lblTitle = new Label { 
                Text = "Rate Your Visit", 
                ForeColor = Color.White, 
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };
            header.Controls.Add(lblTitle);
            this.Controls.Add(header);

            string docName = DataManager.Doctors.FirstOrDefault(d => d.Id == _appointment.DoctorId)?.FullName ?? "the Doctor";
            lblDoctorInfo = new Label {
                Text = $"How was your experience with {docName}?",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 80),
                Size = new Size(380, 40),
                TextAlign = ContentAlignment.TopCenter
            };
            this.Controls.Add(lblDoctorInfo);

            // Star Buttons
            Panel pnlStars = new Panel { Location = new Point(70, 130), Size = new Size(300, 50) };
            for (int i = 0; i < 5; i++)
            {
                int ratingValue = i + 1;
                _starButtons[i] = new Button {
                    Text = "☆",
                    Font = new Font("Segoe UI", 20),
                    Size = new Size(50, 50),
                    Location = new Point(i * 55, 0),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.LightGray,
                    Cursor = Cursors.Hand,
                    Tag = ratingValue
                };
                _starButtons[i].FlatAppearance.BorderSize = 0;
                _starButtons[i].Click += StarButton_Click;
                pnlStars.Controls.Add(_starButtons[i]);
            }
            this.Controls.Add(pnlStars);
            UpdateStars();

            Label lblComment = new Label { Text = "Comment (Optional):", Location = new Point(40, 195), AutoSize = true };
            this.Controls.Add(lblComment);

            txtComment = new TextBox {
                Multiline = true,
                Location = new Point(40, 215),
                Size = new Size(360, 80),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(txtComment);

            Button btnSubmit = new Button {
                Text = "Submit Feedback",
                Size = new Size(150, 40),
                Location = new Point(250, 310),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSubmit.Click += BtnSubmit_Click;
            this.Controls.Add(btnSubmit);

            Button btnCancel = new Button {
                Text = "Cancel",
                Size = new Size(100, 40),
                Location = new Point(140, 310),
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);
        }

        private void StarButton_Click(object sender, EventArgs e)
        {
            _selectedRating = (int)((Button)sender).Tag;
            UpdateStars();
        }

        private void UpdateStars()
        {
            for (int i = 0; i < 5; i++)
            {
                _starButtons[i].Text = (i < _selectedRating) ? "★" : "☆";
                _starButtons[i].ForeColor = (i < _selectedRating) ? Color.Gold : Color.Gray;
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (_selectedRating == 0)
            {
                MessageBox.Show("Please select a star rating.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var patient = DataManager.Patients.FirstOrDefault(p => p.Id == _appointment.PatientId);
            var doc = DataManager.Doctors.FirstOrDefault(d => d.Id == _appointment.DoctorId);

            Feedback feedback = new Feedback
            {
                AppointmentId = _appointment.Id,
                PatientId = _appointment.PatientId,
                PatientName = patient?.FullName ?? "Patient",
                DoctorId = _appointment.DoctorId,
                DoctorName = doc?.FullName ?? "Unknown",
                Rating = _selectedRating,
                Comment = txtComment.Text.Trim(),
                FeedbackDate = DateTime.Now,
                FeedbackType = "Doctor"
            };

            // Update Appointment object for consistency
            _appointment.PatientRating = _selectedRating;
            _appointment.PatientFeedback = txtComment.Text.Trim();

            DataManager.AddFeedback(feedback);
            DataManager.SaveAppointments();
            DataManager.SaveAllData();

            MessageBox.Show("Thank you for your feedback!", "Submitted");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
