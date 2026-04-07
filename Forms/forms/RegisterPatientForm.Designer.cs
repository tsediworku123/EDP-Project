using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem
{
    partial class RegisterPatientForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFormContainer = new System.Windows.Forms.Panel();
            this.pnlPreview = new System.Windows.Forms.Panel();
            this.lblRequiredNote = new System.Windows.Forms.Label();
            this.lblPreviewName = new System.Windows.Forms.Label();
            this.lblPreviewDetails = new System.Windows.Forms.Label();
            this.lblPreviewNotes = new System.Windows.Forms.Label();

            this.txtFullName = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.dtpDOB = new System.Windows.Forms.DateTimePicker();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.txtIDNumber = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtAllergies = new System.Windows.Forms.TextBox();
            this.txtEmergName = new System.Windows.Forms.TextBox();
            this.txtEmergPhone = new System.Windows.Forms.TextBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();

            this.lblPhoneValidation = new System.Windows.Forms.Label();
            this.lblAgeInfo = new System.Windows.Forms.Label();

            this.btnRegister = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnPrintCard = new System.Windows.Forms.Button();
            this.btnRegisterAndBook = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // Header
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 80;
            this.lblTitle.Text = "Register New Patient / Create Account";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.AutoSize = true;
            this.pnlHeader.Controls.Add(this.lblTitle);

            // Form Container (Left)
            this.pnlFormContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFormContainer.Width = 700;
            this.pnlFormContainer.Padding = new System.Windows.Forms.Padding(40, 30, 40, 30);
            this.pnlFormContainer.AutoScroll = true;

            // Preview Panel (Right)
            this.pnlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPreview.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.pnlPreview.Padding = new System.Windows.Forms.Padding(30);

            // Buttons
            this.btnRegister.Text = "REGISTER PATIENT";
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);

            this.btnRegisterAndBook.Text = "REGISTER & BOOK APPOINTMENT";
            this.btnRegisterAndBook.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnRegisterAndBook.ForeColor = System.Drawing.Color.White;
            this.btnRegisterAndBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegisterAndBook.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRegisterAndBook.Click += new System.EventHandler(this.btnRegisterAndBook_Click);

            this.btnCancel.Text = "CANCEL";
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.btnClear.Text = "CLEAR";
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            this.btnPrintCard.Text = " REGISTER & PRINT CARD";
            this.btnPrintCard.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintCard.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrintCard.Click += new System.EventHandler(this.btnPrintCard_Click);

            // Form Main
            this.ClientSize = new System.Drawing.Size(1200, 850);
            this.Controls.Add(this.pnlPreview);
            this.Controls.Add(this.pnlFormContainer);
            this.Controls.Add(this.pnlHeader);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clinic System - Registration";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlFormContainer;
        private System.Windows.Forms.Panel pnlPreview;
        private System.Windows.Forms.Label lblRequiredNote;

        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.DateTimePicker dtpDOB;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.TextBox txtIDNumber;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtAllergies;
        private System.Windows.Forms.TextBox txtEmergName;
        private System.Windows.Forms.TextBox txtEmergPhone;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;

        private System.Windows.Forms.Label lblPhoneValidation;
        private System.Windows.Forms.Label lblAgeInfo;

        private System.Windows.Forms.Label lblPreviewName;
        private System.Windows.Forms.Label lblPreviewDetails;
        private System.Windows.Forms.Label lblPreviewNotes;

        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnRegisterAndBook;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnPrintCard;
    }
}
