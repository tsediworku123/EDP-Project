using System.Windows.Forms;
using System.Drawing;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcSystemSettings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            _autoBackupTimer?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle    = new System.Windows.Forms.Label();
            this.pnlMain     = new System.Windows.Forms.Panel();
            // Clinic identity
            this.lblHdr1       = new System.Windows.Forms.Label();
            this.lblClinicName = new System.Windows.Forms.Label();
            this.txtClinicName = new System.Windows.Forms.TextBox();
            this.lblAddr       = new System.Windows.Forms.Label();
            this.txtClinicAddr = new System.Windows.Forms.TextBox();
            this.lblPhone      = new System.Windows.Forms.Label();
            this.txtClinicPhone = new System.Windows.Forms.TextBox();
            // Hours
            this.lblHdr2   = new System.Windows.Forms.Label();
            this.lblStart  = new System.Windows.Forms.Label();
            this.cmbStart  = new System.Windows.Forms.ComboBox();
            this.lblEnd    = new System.Windows.Forms.Label();
            this.cmbEnd    = new System.Windows.Forms.ComboBox();
            // Scheduling
            this.lblHdr3   = new System.Windows.Forms.Label();
            this.chkDouble = new System.Windows.Forms.CheckBox();
            this.lblSlot   = new System.Windows.Forms.Label();
            this.nSlots    = new System.Windows.Forms.NumericUpDown();
            // Appearance & Automation
            this.lblHdr4       = new System.Windows.Forms.Label();
            this.chkDarkMode   = new System.Windows.Forms.CheckBox();
            this.chkAutoBackup = new System.Windows.Forms.CheckBox();
            // Save
            this.btnSave = new System.Windows.Forms.Button();

            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nSlots)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Text = "SYSTEM CONFIGURATION & CLINIC RULES";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(25, 20);
            this.lblTitle.Size = new System.Drawing.Size(700, 35);

            // pnlMain (scrollable white card)
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.AutoScroll = true;
            this.pnlMain.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            this.pnlMain.Location = new System.Drawing.Point(25, 65);
            this.pnlMain.Size = new System.Drawing.Size(960, 600);
            this.pnlMain.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);

            // ── Clinic Identity ──────────────────────────────────
            Section(this.lblHdr1, "CLINIC IDENTITY", 30);
            Row(this.lblClinicName, "Organization Name:", this.txtClinicName, "Alpha Clinic", 70);
            Row(this.lblAddr,       "Address:",            this.txtClinicAddr,  "",            115);
            Row(this.lblPhone,      "Phone / Contact:",    this.txtClinicPhone, "",            160);

            // ── Operational Hours ────────────────────────────────
            Section(this.lblHdr2, "OPERATIONAL HOURS", 215);
            Row(this.lblStart, "Clinic Opens:",  this.cmbStart, null, 255);
            Row(this.lblEnd,   "Clinic Closes:", this.cmbEnd,   null, 300);
            this.cmbStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEnd.DropDownStyle   = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // ── Scheduling Policies ──────────────────────────────
            Section(this.lblHdr3, "SCHEDULING POLICIES", 355);
            this.chkDouble.Text = "Strict Mode: Prevent all overlapping appointments";
            this.chkDouble.Location = new System.Drawing.Point(40, 390);
            this.chkDouble.AutoSize = true;
            this.chkDouble.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            Row(this.lblSlot, "Default Slot (minutes):", this.nSlots, null, 430);
            this.nSlots.Minimum = 5; this.nSlots.Maximum = 90; this.nSlots.Value = 20;
            this.nSlots.Width = 100;

            // ── Appearance & Automation ─────────────────────────
            Section(this.lblHdr4, "APPEARANCE & AUTOMATION", 485);
            this.chkDarkMode.Text = "Dark Mode (applies after save)";
            this.chkDarkMode.Location = new System.Drawing.Point(40, 520);
            this.chkDarkMode.AutoSize = true;
            this.chkDarkMode.Font = new System.Drawing.Font("Segoe UI", 9.5F);

            this.chkAutoBackup.Text = "Enable Auto-Backup (runs every hour in background)";
            this.chkAutoBackup.Location = new System.Drawing.Point(40, 552);
            this.chkAutoBackup.AutoSize = true;
            this.chkAutoBackup.Font = new System.Drawing.Font("Segoe UI", 9.5F);

            // ── Save button ─────────────────────────────────────
            this.btnSave.Text = "SAVE ALL SETTINGS";
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(40, 600);
            this.btnSave.Size = new System.Drawing.Size(260, 48);
            this.btnSave.Cursor = Cursors.Hand;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // Add to panel
            this.pnlMain.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblHdr1, this.lblClinicName, this.txtClinicName,
                this.lblAddr, this.txtClinicAddr, this.lblPhone, this.txtClinicPhone,
                this.lblHdr2, this.lblStart, this.cmbStart, this.lblEnd, this.cmbEnd,
                this.lblHdr3, this.chkDouble, this.lblSlot, this.nSlots,
                this.lblHdr4, this.chkDarkMode, this.chkAutoBackup,
                this.btnSave
            });

            // UcSystemSettings
            this.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlMain);
            this.Size = new System.Drawing.Size(1020, 700);

            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nSlots)).EndInit();
            this.ResumeLayout(false);
        }

        // ── Layout helpers ──────────────────────────────────────────────────
        private void Section(System.Windows.Forms.Label lbl, string text, int y)
        {
            lbl.Text = text;
            lbl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lbl.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            lbl.Location = new System.Drawing.Point(30, y);
            lbl.Size = new System.Drawing.Size(500, 25);
        }

        private void Row(System.Windows.Forms.Label lbl, string text, System.Windows.Forms.Control ctrl, string defaultVal, int y)
        {
            lbl.Text = text;
            lbl.Location = new System.Drawing.Point(40, y);
            lbl.AutoSize = true;
            lbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            ctrl.Location = new System.Drawing.Point(220, y - 3);
            ctrl.Width = 400;
            ctrl.Font = new System.Drawing.Font("Segoe UI", 10F);
            if (defaultVal != null && ctrl is System.Windows.Forms.TextBox tb) tb.Text = defaultVal;
        }

        // ── Field declarations ─────────────────────────────────────────────
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblHdr1, lblClinicName, lblAddr, lblPhone;
        private System.Windows.Forms.TextBox txtClinicName, txtClinicAddr, txtClinicPhone;
        private System.Windows.Forms.Label lblHdr2, lblStart, lblEnd;
        private System.Windows.Forms.ComboBox cmbStart, cmbEnd;
        private System.Windows.Forms.Label lblHdr3, lblSlot;
        private System.Windows.Forms.CheckBox chkDouble;
        private System.Windows.Forms.NumericUpDown nSlots;
        private System.Windows.Forms.Label lblHdr4;
        private System.Windows.Forms.CheckBox chkDarkMode, chkAutoBackup;
        private System.Windows.Forms.Button btnSave;
    }
}
