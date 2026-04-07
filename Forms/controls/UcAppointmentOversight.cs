using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public partial class UcAppointmentOversight : UserControl
    {
        public UcAppointmentOversight()
        {
            InitializeComponent();
            SetupGridColumns();
            LoadDoctorsFilter();
            cmbApptStatusFilter.SelectedIndex = 0;
            LoadAppointments();
        }

        private void SetupGridColumns()
        {
            apptGrid.Columns.Clear();
            apptGrid.Columns.Add("ID", "Appt ID");
            apptGrid.Columns.Add("Patient", "Patient Name");
            apptGrid.Columns.Add("Doctor", "Doctor Name");
            apptGrid.Columns.Add("Specialty", "Specialization");
            apptGrid.Columns.Add("DateTime", "Date & Time");
            apptGrid.Columns.Add("Status", "Status");
        }

        private void LoadDoctorsFilter()
        {
            cmbApptDoctorFilter.Items.Clear();
            cmbApptDoctorFilter.Items.Add("All Doctors");
            foreach (var doc in DataManager.Doctors)
            {
                cmbApptDoctorFilter.Items.Add(doc.FullName);
            }
            cmbApptDoctorFilter.SelectedIndex = 0;
        }

        private void LoadAppointments()
        {
            FilterAppointments(null, null);
        }

        private void FilterAppointments(object sender, EventArgs e)
        {
            if (apptGrid == null || cmbApptDoctorFilter.SelectedItem == null || cmbApptStatusFilter.SelectedItem == null) return;

            DateTime date = dtpApptDateFilter.Value.Date;
            string docName = cmbApptDoctorFilter.SelectedItem.ToString();
            string status = cmbApptStatusFilter.SelectedItem.ToString();

            var filtered = DataManager.Appointments.Where(a => 
                a.AppointmentDate.Date == date && 
                (docName == "All Doctors" || DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId)?.FullName == docName) &&
                (status == "All" || a.Status == status)
            ).ToList();

            apptGrid.Rows.Clear();
            foreach (var a in filtered)
            {
                string patientName = DataManager.Patients.FirstOrDefault(p => p.Id == a.PatientId)?.FullName ?? "Unknown";
                var doctor = DataManager.Doctors.FirstOrDefault(d => d.Id == a.DoctorId);
                string doctorName = doctor?.FullName ?? "Unknown";
                string specialty = doctor?.Specialty ?? "N/A";
                
                apptGrid.Rows.Add("A" + a.Id.ToString("D3"), patientName, doctorName, specialty, a.AppointmentDate.ToString("ddd, MMM dd HH:mm"), a.Status);
            }
            lblResults.Text = $"Showing {filtered.Count} of {DataManager.Appointments.Count} appointments on {date:MMM dd}";
        }

        private void btnCancelAppt_Click(object sender, EventArgs e)
        {
            if (apptGrid.SelectedRows.Count == 0) return;
            string idStr = apptGrid.SelectedRows[0].Cells["ID"].Value.ToString();
            int id = int.Parse(idStr.Replace("A", ""));
            var apt = DataManager.Appointments.FirstOrDefault(a => a.Id == id);
            
            if (apt != null) {
                if (MessageBox.Show("Cancel this appointment? This action is final.", "Admin Override", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    apt.Status = "Cancelled";
                    LoadAppointments();
                }
            }
        }

        private void btnChangeStatus_Click(object sender, EventArgs e)
        {
            if (apptGrid.SelectedRows.Count == 0) return;
            string idStr = apptGrid.SelectedRows[0].Cells["ID"].Value.ToString();
            int id = int.Parse(idStr.Replace("A", ""));
            var apt = DataManager.Appointments.FirstOrDefault(a => a.Id == id);

            if (apt != null) {
                Form f = new Form { Text = "Update Status", Size = new Size(300, 200), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, BackColor = Color.White };
                f.Controls.Add(new Label { Text = "Choose New Status:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
                ComboBox c = new ComboBox { Location = new Point(20, 50), Width = 240, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10F) };
                c.Items.AddRange(new string[] { "Scheduled", "Completed", "Cancelled", "No-show" });
                c.SelectedItem = apt.Status;
                Button b = new Button { Text = "Save Changes", Location = new Point(90, 100), Size = new Size(120, 35), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
                b.Click += (s2, e2) => { apt.Status = c.SelectedItem.ToString(); f.DialogResult = DialogResult.OK; };
                f.Controls.AddRange(new Control[] { c, b });
                if (f.ShowDialog() == DialogResult.OK) LoadAppointments();
            }
        }

        private void btnReassign_Click(object sender, EventArgs e)
        {
            if (apptGrid.SelectedRows.Count == 0) return;
            string idStr = apptGrid.SelectedRows[0].Cells["ID"].Value.ToString();
            int id = int.Parse(idStr.Replace("A", ""));
            var apt = DataManager.Appointments.FirstOrDefault(a => a.Id == id);

            if (apt != null) {
                Form f = new Form { Text = "Reassign Doctor", Size = new Size(350, 220), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
                ComboBox c = new ComboBox { Location = new Point(50, 50), Width = 230, DropDownStyle = ComboBoxStyle.DropDownList };
                c.Items.AddRange(DataManager.Doctors.Select(d => $"{d.FullName} ({d.Specialty})").ToArray());
                Button b = new Button { Text = "Confirm", Location = new Point(125, 110), Size = new Size(100, 35) };
                b.Click += (s2, e2) => { 
                    if (c.SelectedItem != null) {
                        string name = c.SelectedItem.ToString().Split('(')[0].Trim();
                        apt.DoctorId = DataManager.Doctors.First(d => d.FullName == name).Id;
                        f.DialogResult = DialogResult.OK; 
                    }
                };
                f.Controls.AddRange(new Control[] { c, b });
                if (f.ShowDialog() == DialogResult.OK) LoadAppointments();
            }
        }
    }
}
