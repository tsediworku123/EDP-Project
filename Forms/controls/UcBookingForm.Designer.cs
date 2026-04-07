using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcBookingForm
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
            this.pnlBody = new System.Windows.Forms.Panel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblPatient = new System.Windows.Forms.Label();
            this.cmbPatient = new System.Windows.Forms.ComboBox();
            this.btnNewPatient = new System.Windows.Forms.Button();
            this.lblDoctor = new System.Windows.Forms.Label();
            this.cmbDoctor = new System.Windows.Forms.ComboBox();
            this.lblShift = new System.Windows.Forms.Label();
            this.cmbShift = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.lblDur = new System.Windows.Forms.Label();
            this.cmbDuration = new System.Windows.Forms.ComboBox();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblEndTimeDisplay = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.lblFee = new System.Windows.Forms.Label();
            this.nudFee = new System.Windows.Forms.NumericUpDown();
            this.chkPaid = new System.Windows.Forms.CheckBox();
            this.btnBook = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFee)).BeginInit();
            this.SuspendLayout();

            // pnlBody (Scrollable Area)
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.AutoScroll = true;
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlBody.Controls.Add(this.pnlMain);
            this.pnlBody.Controls.Add(this.lblTitle);
            this.pnlBody.Name = "pnlBody";

            // pnlFooter (Stationary Action Bar)
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 100;
            this.pnlFooter.BackColor = System.Drawing.Color.White;
            this.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Controls.Add(this.btnBook);
            this.pnlFooter.Name = "pnlFooter";

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(30, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "CLINICAL BOOKING ENGINE";

            // pnlMain (The Form)
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Controls.Add(this.txtReason);
            this.pnlMain.Controls.Add(this.nudFee);
            this.pnlMain.Controls.Add(this.lblFee);
            this.pnlMain.Controls.Add(this.chkPaid);
            this.pnlMain.Controls.Add(this.cmbDuration);
            this.pnlMain.Controls.Add(this.lblDur);
            this.pnlMain.Controls.Add(this.cmbType);
            this.pnlMain.Controls.Add(this.lblType);
            this.pnlMain.Controls.Add(this.lblEndTimeDisplay);
            this.pnlMain.Controls.Add(this.lblEnd);
            this.pnlMain.Controls.Add(this.dtpStartTime);
            this.pnlMain.Controls.Add(this.lblStart);
            this.pnlMain.Controls.Add(this.dtpDate);
            this.pnlMain.Controls.Add(this.lblDate);
            this.pnlMain.Controls.Add(this.cmbShift);
            this.pnlMain.Controls.Add(this.lblShift);
            this.pnlMain.Controls.Add(this.cmbDoctor);
            this.pnlMain.Controls.Add(this.lblDoctor);
            this.pnlMain.Controls.Add(this.btnNewPatient);
            this.pnlMain.Controls.Add(this.cmbPatient);
            this.pnlMain.Controls.Add(this.lblPatient);
            this.pnlMain.Location = new System.Drawing.Point(30, 75);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(650, 560);
            
            // lblPatient
            this.lblPatient.AutoSize = true;
            this.lblPatient.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblPatient.Location = new System.Drawing.Point(25, 30);
            this.lblPatient.Text = "SELECT PATIENT:";
            
            this.cmbPatient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cmbPatient.Location = new System.Drawing.Point(150, 27);
            this.cmbPatient.Name = "cmbPatient";
            this.cmbPatient.Size = new System.Drawing.Size(430, 21);

            this.btnNewPatient.Text = " + ";
            this.btnNewPatient.FlatStyle = FlatStyle.Flat;
            this.btnNewPatient.Location = new System.Drawing.Point(590, 26);
            this.btnNewPatient.Size = new System.Drawing.Size(35, 23);
            this.btnNewPatient.Click += new System.EventHandler(this.btnNewPatient_Click);
            
            // Doctor & Shift
            this.lblDoctor.AutoSize = true;
            this.lblDoctor.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblDoctor.Location = new System.Drawing.Point(25, 70);
            this.lblDoctor.Text = "SELECT DOCTOR:";
            
            this.cmbDoctor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDoctor.Location = new System.Drawing.Point(150, 67);
            this.cmbDoctor.Size = new System.Drawing.Size(430, 21);

            this.lblShift.AutoSize = true;
            this.lblShift.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblShift.Location = new System.Drawing.Point(25, 110);
            this.lblShift.Text = "SELECT SHIFT:";

            this.cmbShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShift.Location = new System.Drawing.Point(150, 107);
            this.cmbShift.Size = new System.Drawing.Size(430, 21);
            this.cmbShift.SelectedIndexChanged += new System.EventHandler(this.cmbShift_SelectedIndexChanged);
            
            // Timing
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblDate.Location = new System.Drawing.Point(25, 150);
            this.lblDate.Text = "DATE:";

            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(150, 147);
            this.dtpDate.Size = new System.Drawing.Size(120, 20);

            this.lblStart.AutoSize = true;
            this.lblStart.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblStart.Location = new System.Drawing.Point(300, 150);
            this.lblStart.Text = "START TIME:";

            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStartTime.ShowUpDown = true;
            this.dtpStartTime.Location = new System.Drawing.Point(380, 147);
            this.dtpStartTime.Size = new System.Drawing.Size(120, 20);
            this.dtpStartTime.ValueChanged += new System.EventHandler(this.TimingChanged);

            this.lblDur.AutoSize = true;
            this.lblDur.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblDur.Location = new System.Drawing.Point(25, 190);
            this.lblDur.Text = "DURATION:";

            this.cmbDuration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDuration.Items.AddRange(new object[] { "15", "20", "30", "45", "60" });
            this.cmbDuration.Location = new System.Drawing.Point(150, 187);
            this.cmbDuration.Size = new System.Drawing.Size(80, 21);
            this.cmbDuration.SelectedIndexChanged += new System.EventHandler(this.TimingChanged);

            this.lblEnd.AutoSize = true;
            this.lblEnd.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblEnd.Location = new System.Drawing.Point(250, 190);
            this.lblEnd.Text = "END TIME:";

            this.lblEndTimeDisplay.AutoSize = true;
            this.lblEndTimeDisplay.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEndTimeDisplay.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.lblEndTimeDisplay.Location = new System.Drawing.Point(330, 188);
            this.lblEndTimeDisplay.Text = "--:-- --";
            
            // Details
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblType.Location = new System.Drawing.Point(25, 230);
            this.lblType.Text = "APPT TYPE:";
            
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Items.AddRange(new object[] { "New Consultation", "Follow-up", "Urgent", "Procedure", "Teleconsultation"});
            this.cmbType.Location = new System.Drawing.Point(150, 227);
            this.cmbType.Size = new System.Drawing.Size(430, 21);
            
            this.lblFee.AutoSize = true;
            this.lblFee.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblFee.Location = new System.Drawing.Point(25, 270);
            this.lblFee.Text = "CONSULT FEE:";
            
            this.nudFee.Location = new System.Drawing.Point(150, 267);
            this.nudFee.Size = new System.Drawing.Size(100, 20);
            this.nudFee.Value = 50;
            this.nudFee.DecimalPlaces = 2;
            
            this.chkPaid.AutoSize = true;
            this.chkPaid.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.chkPaid.Location = new System.Drawing.Point(270, 267);
            this.chkPaid.Text = "MARK AS PAID";

            this.txtReason.Location = new System.Drawing.Point(25, 310);
            this.txtReason.Multiline = true;
            this.txtReason.Size = new System.Drawing.Size(600, 220);
            this.txtReason.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            
            // FOOTER ACTIONS
            this.btnBook.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBook.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnBook.ForeColor = System.Drawing.Color.White;
            this.btnBook.Location = new System.Drawing.Point(310, 20);
            this.btnBook.Size = new System.Drawing.Size(320, 60);
            this.btnBook.Text = "CONFIRM && SAVE APPOINTMENT";
            this.btnBook.Click += new System.EventHandler(this.btnBook_Click);

            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnCancel.Location = new System.Drawing.Point(30, 20);
            this.btnCancel.Size = new System.Drawing.Size(260, 60);
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.Click += new System.EventHandler(this.HandleCancel_Click);
            
            // UcBookingForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlFooter);
            this.Name = "UcBookingForm";
            this.Size = new System.Drawing.Size(1000, 750);
            
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFee)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblPatient;
        private System.Windows.Forms.ComboBox cmbPatient;
        private System.Windows.Forms.Button btnNewPatient;
        private System.Windows.Forms.Label lblDoctor;
        private System.Windows.Forms.ComboBox cmbDoctor;
        private System.Windows.Forms.Label lblShift;
        private System.Windows.Forms.ComboBox cmbShift;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label lblDur;
        private System.Windows.Forms.ComboBox cmbDuration;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Label lblEndTimeDisplay;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Label lblFee;
        private System.Windows.Forms.NumericUpDown nudFee;
        private System.Windows.Forms.CheckBox chkPaid;
        private System.Windows.Forms.Button btnBook;
        private System.Windows.Forms.Button btnCancel;
    }
}
