using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class NotificationsForm : Form
    {
        private Patient currentPatient;

        public NotificationsForm()
        {
            InitializeComponent();
            currentPatient = DataManager.GetCurrentPatient();
            LoadNotifications();
        }

        private void LoadNotifications()
        {
            lvNotifications.Items.Clear();
            var notifications = DataManager.GetPatientNotifications(currentPatient.Id);

            foreach (var notif in notifications)
            {
                ListViewItem item = new ListViewItem(notif.CreatedAt.ToString("MMM dd, yyyy HH:mm"));
                item.SubItems.Add(notif.Title);
                item.SubItems.Add(notif.Message);
                item.SubItems.Add(notif.IsRead ? "Read" : "Unread");
                item.Tag = notif;

                // Color code based on read status
                if (!notif.IsRead)
                {
                    item.BackColor = Color.FromArgb(255, 255, 200); // Light yellow for unread
                    item.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
                else
                {
                    item.BackColor = Color.White;
                }

                // Color code by type
                if (notif.Type == "Success")
                    item.ForeColor = Color.FromArgb(46, 204, 113);
                else if (notif.Type == "Warning")
                    item.ForeColor = Color.FromArgb(241, 196, 15);
                else if (notif.Type == "Error")
                    item.ForeColor = Color.FromArgb(231, 76, 60);

                lvNotifications.Items.Add(item);
            }

            lblStatus.Text = $"You have {notifications.Count(n => !n.IsRead)} unread notifications";
            UpdateMarkAllButton();
        }

        private void UpdateMarkAllButton()
        {
            btnMarkAllRead.Enabled = lvNotifications.Items.Cast<ListViewItem>()
                .Any(item => ((Notification)item.Tag).IsRead == false);
        }

        private void lvNotifications_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvNotifications.SelectedItems.Count > 0)
            {
                Notification selected = (Notification)lvNotifications.SelectedItems[0].Tag;

                if (!selected.IsRead)
                {
                    DataManager.MarkNotificationAsRead(selected.Id);
                    LoadNotifications();
                }
            }
        }

        private void btnMarkAllRead_Click(object sender, EventArgs e)
        {
            var unread = DataManager.Notifications
                .Where(n => n.PatientId == currentPatient.Id && !n.IsRead)
                .ToList();

            foreach (var notif in unread)
            {
                DataManager.MarkNotificationAsRead(notif.Id);
            }

            LoadNotifications();
            MessageBox.Show("All notifications marked as read!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete all notifications?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DataManager.Notifications.RemoveAll(n => n.PatientId == currentPatient.Id);
                LoadNotifications();
                MessageBox.Show("All notifications deleted!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadNotifications();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}