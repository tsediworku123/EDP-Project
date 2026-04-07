using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem
{
    public partial class RegisterPatientForm : Form
    {
        public RegisterPatientForm()
        {
            InitializeComponent();
            SetupAppLayout();
            SetupEvents();
            
            // Fix Gender Options
            cmbGender.Items.Clear();
            cmbGender.Items.AddRange(new object[] { "Male", "Female", "Other" });
            cmbGender.SelectedIndex = 0;

            UpdatePreview();
        }

        private void SetupAppLayout()
        {
            // Add all input controls to the form panel first
            this.pnlFormContainer.Controls.AddRange(new System.Windows.Forms.Control[] {
                txtFullName, txtPhone, dtpDOB, cmbGender, txtIDNumber,
                txtAddress, txtEmail, txtAllergies,
                txtEmergName, txtEmergPhone, txtNotes, txtUsername, txtPassword,
                lblPhoneValidation, lblAgeInfo,
                btnRegister, btnRegisterAndBook, btnCancel, btnClear
            });

            // Style all TextBoxes consistently
            foreach (Control c in pnlFormContainer.Controls)
            {
                if (c is TextBox tb) {
                    tb.Font = new Font("Segoe UI", 10);
                    tb.Height = 32;
                    tb.BorderStyle = BorderStyle.FixedSingle;
                }
            }
            cmbGender.Font = new Font("Segoe UI", 10);
            dtpDOB.Font = new Font("Segoe UI", 10);

            int y = 30;
            int x1 = 40;
            int x2 = 360;
            int fieldSpacing = 85;

            // Row 1
            AddLabel(this.pnlFormContainer, "FULL NAME*", x1, y);
            this.txtFullName.Location = new System.Drawing.Point(x1, y + 25);
            this.txtFullName.Width = 280;
            
            AddLabel(this.pnlFormContainer, "PHONE NUMBER*", x2, y);
            this.txtPhone.Location = new System.Drawing.Point(x2, y + 25);
            this.txtPhone.Width = 280;
            this.txtPhone.MaxLength = 10;
            this.lblPhoneValidation.AutoSize = true;
            this.lblPhoneValidation.Text = "";
            this.lblPhoneValidation.Location = new System.Drawing.Point(x2, y + 55);
            this.pnlFormContainer.Controls.Add(this.lblPhoneValidation);
            y += fieldSpacing + 10;

            // Row 2
            AddLabel(this.pnlFormContainer, "DATE OF BIRTH*", x1, y);
            this.dtpDOB.Location = new System.Drawing.Point(x1, y + 25);
            this.dtpDOB.Width = 280;
            this.lblAgeInfo.AutoSize = true;
            this.lblAgeInfo.Text = "Age: ---";
            this.lblAgeInfo.Location = new System.Drawing.Point(x1, y + 55);
            this.pnlFormContainer.Controls.Add(this.lblAgeInfo);
            
            AddLabel(this.pnlFormContainer, "GENDER*", x2, y);
            this.cmbGender.Location = new System.Drawing.Point(x2, y + 25);
            this.cmbGender.Width = 280;
            y += fieldSpacing + 10;

            // Row 3
            AddLabel(this.pnlFormContainer, "ID / PASSPORT NUMBER", x1, y);
            this.txtIDNumber.Location = new System.Drawing.Point(x1, y + 25);
            this.txtIDNumber.Width = 280;
            
            AddLabel(this.pnlFormContainer, "EMAIL ADDRESS", x2, y);
            this.txtEmail.Location = new System.Drawing.Point(x2, y + 25);
            this.txtEmail.Width = 280;
            y += fieldSpacing;

            // Row 4 (Account)
            AddLabel(this.pnlFormContainer, "LOGIN USERNAME*", x1, y);
            this.txtUsername.Location = new System.Drawing.Point(x1, y + 25);
            this.txtUsername.Width = 280;

            AddLabel(this.pnlFormContainer, "LOGIN PASSWORD*", x2, y);
            this.txtPassword.Location = new System.Drawing.Point(x2, y + 25);
            this.txtPassword.Width = 280;
            this.txtPassword.UseSystemPasswordChar = true;
            y += fieldSpacing;

            // Full Width Fields
            AddLabel(this.pnlFormContainer, "RESIDENTIAL ADDRESS", x1, y);
            this.txtAddress.Location = new System.Drawing.Point(x1, y + 25);
            this.txtAddress.Width = 600;
            y += fieldSpacing;

            AddLabel(this.pnlFormContainer, "ALLERGIES & CHRONIC CONDITIONS", x1, y);
            this.txtAllergies.Location = new System.Drawing.Point(x1, y + 25);
            this.txtAllergies.Width = 600;
            y += fieldSpacing;

            // Row 6
            AddLabel(this.pnlFormContainer, "EMERGENCY CONTACT NAME", x1, y);
            this.txtEmergName.Location = new System.Drawing.Point(x1, y + 25);
            this.txtEmergName.Width = 280;
            
            AddLabel(this.pnlFormContainer, "EMERGContact PHONE", x2, y);
            this.txtEmergPhone.Location = new System.Drawing.Point(x2, y + 25);
            this.txtEmergPhone.Width = 280;
            this.txtEmergPhone.MaxLength = 10;
            y += fieldSpacing;

            AddLabel(this.pnlFormContainer, "SHORT MEDICAL NOTES", x1, y);
            this.txtNotes.Location = new System.Drawing.Point(x1, y + 25);
            this.txtNotes.Width = 600;
            this.txtNotes.Multiline = true;
            this.txtNotes.Height = 60;
            y += 100;

            // Positioning Buttons at the end of the form
            this.btnRegister.Location = new System.Drawing.Point(x1, y);
            this.btnRegister.Size = new System.Drawing.Size(280, 50);
            this.btnRegisterAndBook.Location = new System.Drawing.Point(x1, y + 60);
            this.btnRegisterAndBook.Size = new System.Drawing.Size(280, 50);
            
            this.btnCancel.Location = new System.Drawing.Point(340, y);
            this.btnCancel.Size = new System.Drawing.Size(140, 50);
            this.btnClear.Location = new System.Drawing.Point(500, y);
            this.btnClear.Size = new System.Drawing.Size(140, 50);

            // Preview Panel setup (card preview)
            Label lblPreviewHeader = new Label { Text = "PATIENT CARD PREVIEW", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(30, 30), AutoSize = true };
            this.pnlPreview.Controls.Add(lblPreviewHeader);

            Panel cardPanel = new Panel { Size = new Size(350, 350), Location = new Point(30, 70), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            
            this.lblPreviewName.Text = "PATIENT NAME";
            this.lblPreviewName.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblPreviewName.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblPreviewName.Location = new Point(20, 30);
            this.lblPreviewName.Size = new Size(310, 40);

            this.lblPreviewDetails.Text = "Gender | Age | Phone";
            this.lblPreviewDetails.Font = new Font("Segoe UI", 10);
            this.lblPreviewDetails.ForeColor = Color.FromArgb(71, 85, 105);
            this.lblPreviewDetails.Location = new Point(20, 80);
            this.lblPreviewDetails.Size = new Size(310, 60);

            this.lblPreviewNotes.Text = "Allergies Summary";
            this.lblPreviewNotes.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            this.lblPreviewNotes.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblPreviewNotes.Location = new Point(20, 150);
            this.lblPreviewNotes.Size = new Size(310, 80);

            cardPanel.Controls.AddRange(new Control[] { this.lblPreviewName, this.lblPreviewDetails, this.lblPreviewNotes });
            this.pnlPreview.Controls.Add(cardPanel);

            this.btnPrintCard.Location = new Point(30, 440);
            this.btnPrintCard.Size = new Size(350, 45);
            this.pnlPreview.Controls.Add(this.btnPrintCard);
        }

        private void AddLabel(Panel p, string text, int x, int y)
        {
            Label lbl = new Label { Text = text, Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105) };
            p.Controls.Add(lbl);
        }

        private void SetupEvents()
        {
            txtPhone.TextChanged += (s, e) => {
                bool isUnique = DataManager.IsPhoneUnique(txtPhone.Text.Trim());
                lblPhoneValidation.Text = isUnique ? " Phone number is unique" : " Phone number already exists!";
                lblPhoneValidation.ForeColor = isUnique ? Color.Green : Color.Red;
                UpdatePreview();
            };

            // Restrict to numbers only 
            txtPhone.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };
            txtEmergPhone.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };

            dtpDOB.ValueChanged += (s, e) => {
                int age = CalculateAge(dtpDOB.Value);
                lblAgeInfo.Text = $"Age: {age} years";
                UpdatePreview();
            };

            txtFullName.TextChanged += (s, e) => UpdatePreview();
            cmbGender.SelectedIndexChanged += (s, e) => UpdatePreview();
        }

        private int CalculateAge(DateTime dob)
        {
            int age = DateTime.Now.Year - dob.Year;
            if (DateTime.Now < dob.AddYears(age)) age--;
            return age;
        }

        private void UpdatePreview()
        {
            lblPreviewName.Text = string.IsNullOrEmpty(txtFullName.Text) ? "Patient Name" : txtFullName.Text.ToUpper();
            lblPreviewDetails.Text = $"{cmbGender.Text} | {lblAgeInfo.Text}\n{txtPhone.Text}";
            lblPreviewNotes.Text = string.IsNullOrEmpty(txtAllergies.Text) ? "No allergies recorded." : "Allergies: " + txtAllergies.Text;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            Patient newPatient = new Patient
            {
                FullName = txtFullName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                DateOfBirth = dtpDOB.Value,
                Gender = cmbGender.Text,
                NationalIdOrPassport = txtIDNumber.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                AllergiesOrChronicConditions = txtAllergies.Text.Trim(),
                EmergencyContactName = txtEmergName.Text.Trim(),
                EmergencyContactPhone = txtEmergPhone.Text.Trim(),
                MedicalNotes = txtNotes.Text.Trim()
            };

            DataManager.RegisterPatient(newPatient);

            MessageBox.Show($"Patient registered successfully!\n\nPatient ID: {newPatient.PatientCode}\nUsername: {newPatient.Username}\n\nYou can now log in using your username and password.", 
                "Registration Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnRegisterAndBook_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            Patient newPatient = new Patient
            {
                FullName = txtFullName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                DateOfBirth = dtpDOB.Value,
                Gender = cmbGender.Text,
                NationalIdOrPassport = txtIDNumber.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                AllergiesOrChronicConditions = txtAllergies.Text.Trim(),
                EmergencyContactName = txtEmergName.Text.Trim(),
                EmergencyContactPhone = txtEmergPhone.Text.Trim(),
                MedicalNotes = txtNotes.Text.Trim()
            };

            DataManager.RegisterPatient(newPatient);

            MessageBox.Show($"Patient registered successfully!\n\nPatient ID: {newPatient.PatientCode}\nOpening appointment booking screen view...", 
                "Registration Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Tag = "Book"; // Signal to open booking
            this.Close();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) || 
                string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill in all required fields (*) including Username and Password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtPhone.Text.Trim().Length != 10)
            {
                MessageBox.Show("Please enter a valid 10-digit phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
 
            if (txtEmergPhone.Text.Trim().Length > 0 && txtEmergPhone.Text.Trim().Length != 10)
            {
                MessageBox.Show("Please enter a valid 10-digit emergency contact phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!DataManager.IsPhoneUnique(txtPhone.Text.Trim()))
            {
                MessageBox.Show("This phone number is already registered.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (Control c in pnlFormContainer.Controls)
            {
                if (c is TextBox tb) tb.Clear();
            }
            dtpDOB.Value = DateTime.Now.AddYears(-20);
            cmbGender.SelectedIndex = -1;
            UpdatePreview();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cardPanel_Paint(object sender, PaintEventArgs e)
        {
            Control c = sender as Control;
            if (c != null)
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(203, 213, 225), 2), 0, 0, c.Width - 1, c.Height - 1);
        }

        private void btnPrintCard_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                MessageBox.Show("Patient Card sent to printer queue.", "Printing...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
