using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class GiveFeedbackForm : Form
    {
        private Patient currentPatient;
        private int selectedRating = 0;
        private Panel centerPanel;

        public GiveFeedbackForm()
        {
            InitializeComponent();
            CreateCenterPanel();
            currentPatient = DataManager.GetCurrentPatient();

            if (currentPatient == null)
            {
                MessageBox.Show("You must be logged in to give feedback.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            LoadDoctors();
            SetupRatingStars();
            this.Resize += GiveFeedbackForm_Resize;
        }

        private void CreateCenterPanel()
        {
            centerPanel = new Panel();
            centerPanel.BackColor = Color.Transparent;
            centerPanel.Size = new Size(550, 500);
            this.Controls.Add(centerPanel);

            // Move content panel to center panel
            this.Controls.Remove(panelContent);
            centerPanel.Controls.Add(this.panelContent);

            CenterPanel();
        }

        private void GiveFeedbackForm_Resize(object sender, EventArgs e)
        {
            CenterPanel();
        }

        private void CenterPanel()
        {
            if (centerPanel != null)
            {
                centerPanel.Location = new Point(
                    (this.ClientSize.Width - centerPanel.Width) / 2,
                    (this.ClientSize.Height - centerPanel.Height) / 2
                );

                panelContent.Location = new Point(
                    (centerPanel.Width - panelContent.Width) / 2,
                    20
                );
            }
        }

        private void LoadDoctors()
        {
            cmbDoctor.Items.Clear();
            cmbDoctor.Items.Add("-- General Clinic Feedback --");
            foreach (var doctor in DataManager.Doctors)
            {
                cmbDoctor.Items.Add(doctor.FullName + " - " + doctor.Specialization);
            }
            cmbDoctor.SelectedIndex = 0;
        }

        private void SetupRatingStars()
        {
            // Clear any existing stars
            for (int i = panelStars.Controls.Count - 1; i >= 0; i--)
            {
                panelStars.Controls[i].Dispose();
            }
            panelStars.Controls.Clear();

            // Create star rating buttons
            for (int i = 1; i <= 5; i++)
            {
                Button star = new Button();
                star.Text = "★";
                star.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
                star.ForeColor = Color.Gray;
                star.BackColor = Color.Transparent;
                star.FlatStyle = FlatStyle.Flat;
                star.FlatAppearance.BorderSize = 0;
                star.Size = new Size(40, 40);
                star.Location = new Point((i - 1) * 45, 0);
                star.Tag = i;
                star.Click += Star_Click;
                star.MouseEnter += Star_MouseEnter;
                star.MouseLeave += Star_MouseLeave;
                panelStars.Controls.Add(star);
            }
        }

        private void Star_Click(object sender, EventArgs e)
        {
            Button clickedStar = (Button)sender;
            selectedRating = (int)clickedStar.Tag;

            for (int i = 0; i < panelStars.Controls.Count; i++)
            {
                Button star = (Button)panelStars.Controls[i];
                int starNum = (int)star.Tag;
                star.ForeColor = starNum <= selectedRating ? Color.Gold : Color.Gray;
            }
        }

        private void Star_MouseEnter(object sender, EventArgs e)
        {
            Button hoverStar = (Button)sender;
            int hoverNum = (int)hoverStar.Tag;

            for (int i = 0; i < panelStars.Controls.Count; i++)
            {
                Button star = (Button)panelStars.Controls[i];
                int starNum = (int)star.Tag;
                star.ForeColor = starNum <= hoverNum ? Color.Orange : Color.Gray;
            }
        }

        private void Star_MouseLeave(object sender, EventArgs e)
        {
            for (int i = 0; i < panelStars.Controls.Count; i++)
            {
                Button star = (Button)panelStars.Controls[i];
                int starNum = (int)star.Tag;
                star.ForeColor = starNum <= selectedRating ? Color.Gold : Color.Gray;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (selectedRating == 0)
            {
                MessageBox.Show("Please select a rating!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtComment.Text))
            {
                MessageBox.Show("Please enter your feedback comment!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedDoctor = cmbDoctor.SelectedItem.ToString();
            int? doctorId = null;
            string doctorName = "Clinic";

            if (cmbDoctor.SelectedIndex > 0)
            {
                string doctorFullName = selectedDoctor.Split('-')[0].Trim();
                var doctor = DataManager.Doctors.FirstOrDefault(d => d.FullName == doctorFullName);
                if (doctor != null)
                {
                    doctorId = doctor.Id;
                    doctorName = doctor.FullName;
                }
            }

            Feedback feedback = new Feedback
            {
                PatientId = currentPatient.Id,
                PatientName = currentPatient.FullName,
                DoctorId = doctorId,
                DoctorName = doctorName,
                Rating = selectedRating,
                Comment = txtComment.Text.Trim(),
                FeedbackDate = DateTime.Now,
                FeedbackType = doctorId.HasValue ? "Doctor" : "Clinic"
            };

            DataManager.AddFeedback(feedback);

            MessageBox.Show("Thank you for your feedback!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}