using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcConsultationPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lblPatientName = new System.Windows.Forms.Label();
            this.lblPatientAge = new System.Windows.Forms.Label();
            this.lblReason = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblDiagnosis = new System.Windows.Forms.Label();
            this.txtDiagnosis = new System.Windows.Forms.TextBox();
            this.lblPrescription = new System.Windows.Forms.Label();
            this.txtPrescription = new System.Windows.Forms.TextBox();
            this.chkFollowUp = new System.Windows.Forms.CheckBox();
            this.dtpFollowUp = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            
            this.mainLayout.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();
            
            // mainLayout
            this.mainLayout.ColumnCount = 2;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.mainLayout.Controls.Add(this.pnlLeft, 0, 0);
            this.mainLayout.Controls.Add(this.pnlRight, 1, 0);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Padding = new System.Windows.Forms.Padding(20);
            this.mainLayout.RowCount = 1;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(1000, 800);
            
            // pnlLeft
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlLeft.Controls.Add(this.lblReason);
            this.pnlLeft.Controls.Add(this.lblPatientAge);
            this.pnlLeft.Controls.Add(this.lblPatientName);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(23, 23);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(20);
            this.pnlLeft.Size = new System.Drawing.Size(330, 754);
            
            // lblPatientName
            this.lblPatientName.AutoSize = true;
            this.lblPatientName.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblPatientName.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblPatientName.Location = new System.Drawing.Point(20, 20);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Size = new System.Drawing.Size(155, 30);
            this.lblPatientName.Text = "Patient Name";
            
            // lblPatientAge
            this.lblPatientAge.AutoSize = true;
            this.lblPatientAge.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPatientAge.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblPatientAge.Location = new System.Drawing.Point(20, 55);
            this.lblPatientAge.Name = "lblPatientAge";
            this.lblPatientAge.Size = new System.Drawing.Size(91, 19);
            this.lblPatientAge.Text = "Age / Gender";
            
            // lblReason
            this.lblReason.AutoSize = true;
            this.lblReason.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblReason.Location = new System.Drawing.Point(20, 90);
            this.lblReason.MaximumSize = new System.Drawing.Size(250, 0);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(167, 19);
            this.lblReason.Text = "Reason for visit: loading...";
            
            // pnlRight
            this.pnlRight.Controls.Add(this.btnSave);
            this.pnlRight.Controls.Add(this.dtpFollowUp);
            this.pnlRight.Controls.Add(this.chkFollowUp);
            this.pnlRight.Controls.Add(this.txtPrescription);
            this.pnlRight.Controls.Add(this.lblPrescription);
            this.pnlRight.Controls.Add(this.txtDiagnosis);
            this.pnlRight.Controls.Add(this.lblDiagnosis);
            this.pnlRight.Controls.Add(this.txtNotes);
            this.pnlRight.Controls.Add(this.lblNotes);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(359, 23);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(20);
            this.pnlRight.Size = new System.Drawing.Size(618, 754);
            
            // lblNotes
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblNotes.Location = new System.Drawing.Point(20, 0);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(107, 15);
            this.lblNotes.Text = "CLINICAL NOTES:";
            
            // txtNotes
            this.txtNotes.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtNotes.Location = new System.Drawing.Point(20, 20);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(550, 150);
            
            // lblDiagnosis
            this.lblDiagnosis.AutoSize = true;
            this.lblDiagnosis.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDiagnosis.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblDiagnosis.Location = new System.Drawing.Point(20, 190);
            this.lblDiagnosis.Name = "lblDiagnosis";
            this.lblDiagnosis.Size = new System.Drawing.Size(73, 15);
            this.lblDiagnosis.Text = "DIAGNOSIS:";
            
            // txtDiagnosis
            this.txtDiagnosis.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.txtDiagnosis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDiagnosis.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtDiagnosis.Location = new System.Drawing.Point(20, 210);
            this.txtDiagnosis.Multiline = true;
            this.txtDiagnosis.Name = "txtDiagnosis";
            this.txtDiagnosis.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDiagnosis.Size = new System.Drawing.Size(550, 80);
            
            // lblPrescription
            this.lblPrescription.AutoSize = true;
            this.lblPrescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPrescription.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblPrescription.Location = new System.Drawing.Point(20, 310);
            this.lblPrescription.Name = "lblPrescription";
            this.lblPrescription.Size = new System.Drawing.Size(147, 15);
            this.lblPrescription.Text = "PRESCRIPTION / ADVICE:";
            
            // txtPrescription
            this.txtPrescription.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.txtPrescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrescription.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPrescription.Location = new System.Drawing.Point(20, 330);
            this.txtPrescription.Multiline = true;
            this.txtPrescription.Name = "txtPrescription";
            this.txtPrescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPrescription.Size = new System.Drawing.Size(550, 120);
            
            // chkFollowUp
            this.chkFollowUp.AutoSize = true;
            this.chkFollowUp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.chkFollowUp.Location = new System.Drawing.Point(20, 470);
            this.chkFollowUp.Name = "chkFollowUp";
            this.chkFollowUp.Size = new System.Drawing.Size(148, 19);
            this.chkFollowUp.Text = "Follow-up suggested?";
            this.chkFollowUp.UseVisualStyleBackColor = true;
            
            // dtpFollowUp
            this.dtpFollowUp.Enabled = false;
            this.dtpFollowUp.Location = new System.Drawing.Point(20, 495);
            this.dtpFollowUp.Name = "dtpFollowUp";
            this.dtpFollowUp.Size = new System.Drawing.Size(150, 20);
            
            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(20, 565);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(400, 55);
            this.btnSave.Text = " MARK AS COMPLETED && SAVE NOTES";
            // UcConsultationPanel
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.mainLayout);
            this.Name = "UcConsultationPanel";
            this.Size = new System.Drawing.Size(1000, 800);
            
            this.mainLayout.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblPatientName;
        private System.Windows.Forms.Label lblPatientAge;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblDiagnosis;
        private System.Windows.Forms.TextBox txtDiagnosis;
        private System.Windows.Forms.Label lblPrescription;
        private System.Windows.Forms.TextBox txtPrescription;
        private System.Windows.Forms.CheckBox chkFollowUp;
        private System.Windows.Forms.DateTimePicker dtpFollowUp;
        private System.Windows.Forms.Button btnSave;
    }
}
