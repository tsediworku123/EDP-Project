namespace ClinicAppointmentSystem
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controls
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExportPDF;
        private System.Windows.Forms.Button btnPrint;

        // Daily Report Panel
        private System.Windows.Forms.Panel panelDaily;
        private System.Windows.Forms.Label lblDailyTitle;
        private System.Windows.Forms.Label lblTodayTotal;
        private System.Windows.Forms.Label lblTodayCompleted;
        private System.Windows.Forms.Label lblTodayPending;
        private System.Windows.Forms.Label lblTodayCancelled;
        private System.Windows.Forms.Label lblTodayTotalLabel;
        private System.Windows.Forms.Label lblTodayCompletedLabel;
        private System.Windows.Forms.Label lblTodayPendingLabel;
        private System.Windows.Forms.Label lblTodayCancelledLabel;

        // Monthly Report Panel
        private System.Windows.Forms.Panel panelMonthly;
        private System.Windows.Forms.Label lblMonthlyTitle;
        private System.Windows.Forms.Label lblMonthlyTotal;
        private System.Windows.Forms.Label lblMonthlyCompleted;
        private System.Windows.Forms.Label lblMonthlyPending;
        private System.Windows.Forms.Label lblMonthlyCancelled;
        private System.Windows.Forms.Label lblMonthlyTotalLabel;
        private System.Windows.Forms.Label lblMonthlyCompletedLabel;
        private System.Windows.Forms.Label lblMonthlyPendingLabel;
        private System.Windows.Forms.Label lblMonthlyCancelledLabel;

        // Patient Stats Panel
        private System.Windows.Forms.Panel panelPatients;
        private System.Windows.Forms.Label lblPatientsTitle;
        private System.Windows.Forms.Label lblTotalPatients;
        private System.Windows.Forms.Label lblMalePatients;
        private System.Windows.Forms.Label lblFemalePatients;
        private System.Windows.Forms.Label lblTotalPatientsLabel;
        private System.Windows.Forms.Label lblMalePatientsLabel;
        private System.Windows.Forms.Label lblFemalePatientsLabel;

        // Doctor Performance
        private System.Windows.Forms.Panel panelDoctorPerf;
        private System.Windows.Forms.Label lblDoctorPerfTitle;
        private System.Windows.Forms.ListView lvDoctorPerformance;
        private System.Windows.Forms.ColumnHeader colDocId;
        private System.Windows.Forms.ColumnHeader colDocName;
        private System.Windows.Forms.ColumnHeader colSpecialization;
        private System.Windows.Forms.ColumnHeader colTotalAppts;
        private System.Windows.Forms.ColumnHeader colCompletedAppts;
        private System.Windows.Forms.ColumnHeader colSuccessRate;

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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExportPDF = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panelDaily = new System.Windows.Forms.Panel();
            this.panelMonthly = new System.Windows.Forms.Panel();
            this.panelPatients = new System.Windows.Forms.Panel();
            this.panelDoctorPerf = new System.Windows.Forms.Panel();
            this.lvDoctorPerformance = new System.Windows.Forms.ListView();
            this.colDocId = new System.Windows.Forms.ColumnHeader();
            this.colDocName = new System.Windows.Forms.ColumnHeader();
            this.colSpecialization = new System.Windows.Forms.ColumnHeader();
            this.colTotalAppts = new System.Windows.Forms.ColumnHeader();
            this.colCompletedAppts = new System.Windows.Forms.ColumnHeader();
            this.colSuccessRate = new System.Windows.Forms.ColumnHeader();

            // Daily Report Labels
            this.lblDailyTitle = new System.Windows.Forms.Label();
            this.lblTodayTotalLabel = new System.Windows.Forms.Label();
            this.lblTodayCompletedLabel = new System.Windows.Forms.Label();
            this.lblTodayPendingLabel = new System.Windows.Forms.Label();
            this.lblTodayCancelledLabel = new System.Windows.Forms.Label();
            this.lblTodayTotal = new System.Windows.Forms.Label();
            this.lblTodayCompleted = new System.Windows.Forms.Label();
            this.lblTodayPending = new System.Windows.Forms.Label();
            this.lblTodayCancelled = new System.Windows.Forms.Label();

            // Monthly Report Labels
            this.lblMonthlyTitle = new System.Windows.Forms.Label();
            this.lblMonthlyTotalLabel = new System.Windows.Forms.Label();
            this.lblMonthlyCompletedLabel = new System.Windows.Forms.Label();
            this.lblMonthlyPendingLabel = new System.Windows.Forms.Label();
            this.lblMonthlyCancelledLabel = new System.Windows.Forms.Label();
            this.lblMonthlyTotal = new System.Windows.Forms.Label();
            this.lblMonthlyCompleted = new System.Windows.Forms.Label();
            this.lblMonthlyPending = new System.Windows.Forms.Label();
            this.lblMonthlyCancelled = new System.Windows.Forms.Label();

            // Patient Stats Labels
            this.lblPatientsTitle = new System.Windows.Forms.Label();
            this.lblTotalPatientsLabel = new System.Windows.Forms.Label();
            this.lblMalePatientsLabel = new System.Windows.Forms.Label();
            this.lblFemalePatientsLabel = new System.Windows.Forms.Label();
            this.lblTotalPatients = new System.Windows.Forms.Label();
            this.lblMalePatients = new System.Windows.Forms.Label();
            this.lblFemalePatients = new System.Windows.Forms.Label();

            this.panelHeader.SuspendLayout();
            this.panelDaily.SuspendLayout();
            this.panelMonthly.SuspendLayout();
            this.panelPatients.SuspendLayout();
            this.panelDoctorPerf.SuspendLayout();
            this.SuspendLayout();

            // ========== FORM PROPERTIES ==========
            this.Text = "Reports & Analytics";
            this.ClientSize = new System.Drawing.Size(1300, 800);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.BackColor = System.Drawing.Color.FromArgb(240, 248, 255);

            // ========== HEADER PANEL ==========
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(0, 191, 255);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 80;
            this.panelHeader.TabIndex = 0;

            this.lblTitle.Text = "Reports & Analytics";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Size = new System.Drawing.Size(400, 45);

            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker.Location = new System.Drawing.Point(800, 25);
            this.datePicker.Size = new System.Drawing.Size(150, 27);
            this.datePicker.Value = System.DateTime.Now;

            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(970, 20);
            this.btnRefresh.Size = new System.Drawing.Size(100, 40);
            this.btnRefresh.Text = "🔄 Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.btnExportPDF.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnExportPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPDF.ForeColor = System.Drawing.Color.White;
            this.btnExportPDF.Location = new System.Drawing.Point(1080, 20);
            this.btnExportPDF.Size = new System.Drawing.Size(100, 40);
            this.btnExportPDF.Text = "📄 Export PDF";
            this.btnExportPDF.Click += new System.EventHandler(this.btnExportPDF_Click);

            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(155, 89, 182);
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(1190, 20);
            this.btnPrint.Size = new System.Drawing.Size(90, 40);
            this.btnPrint.Text = "🖨️ Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);

            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.datePicker);
            this.panelHeader.Controls.Add(this.btnRefresh);
            this.panelHeader.Controls.Add(this.btnExportPDF);
            this.panelHeader.Controls.Add(this.btnPrint);

            // ========== DAILY REPORT PANEL ==========
            this.panelDaily.BackColor = System.Drawing.Color.White;
            this.panelDaily.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDaily.Location = new System.Drawing.Point(20, 100);
            this.panelDaily.Size = new System.Drawing.Size(300, 200);
            this.panelDaily.TabIndex = 1;

            this.lblDailyTitle.Text = "📊 Daily Report";
            this.lblDailyTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblDailyTitle.ForeColor = System.Drawing.Color.FromArgb(0, 105, 148);
            this.lblDailyTitle.Location = new System.Drawing.Point(10, 10);
            this.lblDailyTitle.Size = new System.Drawing.Size(200, 25);

            this.lblTodayTotalLabel.Text = "Total:";
            this.lblTodayTotalLabel.Location = new System.Drawing.Point(20, 50);
            this.lblTodayTotalLabel.Size = new System.Drawing.Size(80, 20);

            this.lblTodayTotal.Text = "0";
            this.lblTodayTotal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTodayTotal.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.lblTodayTotal.Location = new System.Drawing.Point(120, 45);
            this.lblTodayTotal.Size = new System.Drawing.Size(50, 25);

            this.lblTodayCompletedLabel.Text = "Completed:";
            this.lblTodayCompletedLabel.Location = new System.Drawing.Point(20, 80);
            this.lblTodayCompletedLabel.Size = new System.Drawing.Size(80, 20);

            this.lblTodayCompleted.Text = "0";
            this.lblTodayCompleted.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTodayCompleted.ForeColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.lblTodayCompleted.Location = new System.Drawing.Point(120, 78);
            this.lblTodayCompleted.Size = new System.Drawing.Size(50, 20);

            this.lblTodayPendingLabel.Text = "Pending:";
            this.lblTodayPendingLabel.Location = new System.Drawing.Point(20, 110);
            this.lblTodayPendingLabel.Size = new System.Drawing.Size(80, 20);

            this.lblTodayPending.Text = "0";
            this.lblTodayPending.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTodayPending.ForeColor = System.Drawing.Color.FromArgb(241, 196, 15);
            this.lblTodayPending.Location = new System.Drawing.Point(120, 108);
            this.lblTodayPending.Size = new System.Drawing.Size(50, 20);

            this.lblTodayCancelledLabel.Text = "Cancelled:";
            this.lblTodayCancelledLabel.Location = new System.Drawing.Point(20, 140);
            this.lblTodayCancelledLabel.Size = new System.Drawing.Size(80, 20);

            this.lblTodayCancelled.Text = "0";
            this.lblTodayCancelled.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTodayCancelled.ForeColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.lblTodayCancelled.Location = new System.Drawing.Point(120, 138);
            this.lblTodayCancelled.Size = new System.Drawing.Size(50, 20);

            this.panelDaily.Controls.Add(this.lblDailyTitle);
            this.panelDaily.Controls.Add(this.lblTodayTotalLabel);
            this.panelDaily.Controls.Add(this.lblTodayTotal);
            this.panelDaily.Controls.Add(this.lblTodayCompletedLabel);
            this.panelDaily.Controls.Add(this.lblTodayCompleted);
            this.panelDaily.Controls.Add(this.lblTodayPendingLabel);
            this.panelDaily.Controls.Add(this.lblTodayPending);
            this.panelDaily.Controls.Add(this.lblTodayCancelledLabel);
            this.panelDaily.Controls.Add(this.lblTodayCancelled);

            // ========== MONTHLY REPORT PANEL ==========
            this.panelMonthly.BackColor = System.Drawing.Color.White;
            this.panelMonthly.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMonthly.Location = new System.Drawing.Point(340, 100);
            this.panelMonthly.Size = new System.Drawing.Size(300, 200);
            this.panelMonthly.TabIndex = 2;

            this.lblMonthlyTitle.Text = "📅 Monthly Report";
            this.lblMonthlyTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblMonthlyTitle.ForeColor = System.Drawing.Color.FromArgb(0, 105, 148);
            this.lblMonthlyTitle.Location = new System.Drawing.Point(10, 10);
            this.lblMonthlyTitle.Size = new System.Drawing.Size(200, 25);

            this.lblMonthlyTotalLabel.Text = "Total:";
            this.lblMonthlyTotalLabel.Location = new System.Drawing.Point(20, 50);
            this.lblMonthlyTotalLabel.Size = new System.Drawing.Size(80, 20);

            this.lblMonthlyTotal.Text = "0";
            this.lblMonthlyTotal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblMonthlyTotal.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.lblMonthlyTotal.Location = new System.Drawing.Point(120, 45);
            this.lblMonthlyTotal.Size = new System.Drawing.Size(50, 25);

            this.lblMonthlyCompletedLabel.Text = "Completed:";
            this.lblMonthlyCompletedLabel.Location = new System.Drawing.Point(20, 80);
            this.lblMonthlyCompletedLabel.Size = new System.Drawing.Size(80, 20);

            this.lblMonthlyCompleted.Text = "0";
            this.lblMonthlyCompleted.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMonthlyCompleted.ForeColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.lblMonthlyCompleted.Location = new System.Drawing.Point(120, 78);
            this.lblMonthlyCompleted.Size = new System.Drawing.Size(50, 20);

            this.lblMonthlyPendingLabel.Text = "Pending:";
            this.lblMonthlyPendingLabel.Location = new System.Drawing.Point(20, 110);
            this.lblMonthlyPendingLabel.Size = new System.Drawing.Size(80, 20);

            this.lblMonthlyPending.Text = "0";
            this.lblMonthlyPending.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMonthlyPending.ForeColor = System.Drawing.Color.FromArgb(241, 196, 15);
            this.lblMonthlyPending.Location = new System.Drawing.Point(120, 108);
            this.lblMonthlyPending.Size = new System.Drawing.Size(50, 20);

            this.lblMonthlyCancelledLabel.Text = "Cancelled:";
            this.lblMonthlyCancelledLabel.Location = new System.Drawing.Point(20, 140);
            this.lblMonthlyCancelledLabel.Size = new System.Drawing.Size(80, 20);

            this.lblMonthlyCancelled.Text = "0";
            this.lblMonthlyCancelled.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMonthlyCancelled.ForeColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.lblMonthlyCancelled.Location = new System.Drawing.Point(120, 138);
            this.lblMonthlyCancelled.Size = new System.Drawing.Size(50, 20);

            this.panelMonthly.Controls.Add(this.lblMonthlyTitle);
            this.panelMonthly.Controls.Add(this.lblMonthlyTotalLabel);
            this.panelMonthly.Controls.Add(this.lblMonthlyTotal);
            this.panelMonthly.Controls.Add(this.lblMonthlyCompletedLabel);
            this.panelMonthly.Controls.Add(this.lblMonthlyCompleted);
            this.panelMonthly.Controls.Add(this.lblMonthlyPendingLabel);
            this.panelMonthly.Controls.Add(this.lblMonthlyPending);
            this.panelMonthly.Controls.Add(this.lblMonthlyCancelledLabel);
            this.panelMonthly.Controls.Add(this.lblMonthlyCancelled);

            // ========== PATIENT STATS PANEL ==========
            this.panelPatients.BackColor = System.Drawing.Color.White;
            this.panelPatients.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPatients.Location = new System.Drawing.Point(660, 100);
            this.panelPatients.Size = new System.Drawing.Size(300, 200);
            this.panelPatients.TabIndex = 3;

            this.lblPatientsTitle.Text = "👥 Patient Statistics";
            this.lblPatientsTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblPatientsTitle.ForeColor = System.Drawing.Color.FromArgb(0, 105, 148);
            this.lblPatientsTitle.Location = new System.Drawing.Point(10, 10);
            this.lblPatientsTitle.Size = new System.Drawing.Size(200, 25);

            this.lblTotalPatientsLabel.Text = "Total Patients:";
            this.lblTotalPatientsLabel.Location = new System.Drawing.Point(20, 50);
            this.lblTotalPatientsLabel.Size = new System.Drawing.Size(100, 20);

            this.lblTotalPatients.Text = "0";
            this.lblTotalPatients.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalPatients.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.lblTotalPatients.Location = new System.Drawing.Point(140, 45);
            this.lblTotalPatients.Size = new System.Drawing.Size(50, 25);

            this.lblMalePatientsLabel.Text = "Male:";
            this.lblMalePatientsLabel.Location = new System.Drawing.Point(20, 80);
            this.lblMalePatientsLabel.Size = new System.Drawing.Size(100, 20);

            this.lblMalePatients.Text = "0";
            this.lblMalePatients.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMalePatients.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.lblMalePatients.Location = new System.Drawing.Point(140, 78);
            this.lblMalePatients.Size = new System.Drawing.Size(50, 20);

            this.lblFemalePatientsLabel.Text = "Female:";
            this.lblFemalePatientsLabel.Location = new System.Drawing.Point(20, 110);
            this.lblFemalePatientsLabel.Size = new System.Drawing.Size(100, 20);

            this.lblFemalePatients.Text = "0";
            this.lblFemalePatients.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblFemalePatients.ForeColor = System.Drawing.Color.FromArgb(155, 89, 182);
            this.lblFemalePatients.Location = new System.Drawing.Point(140, 108);
            this.lblFemalePatients.Size = new System.Drawing.Size(50, 20);

            this.panelPatients.Controls.Add(this.lblPatientsTitle);
            this.panelPatients.Controls.Add(this.lblTotalPatientsLabel);
            this.panelPatients.Controls.Add(this.lblTotalPatients);
            this.panelPatients.Controls.Add(this.lblMalePatientsLabel);
            this.panelPatients.Controls.Add(this.lblMalePatients);
            this.panelPatients.Controls.Add(this.lblFemalePatientsLabel);
            this.panelPatients.Controls.Add(this.lblFemalePatients);

            // ========== DOCTOR PERFORMANCE PANEL ==========
            this.panelDoctorPerf.BackColor = System.Drawing.Color.White;
            this.panelDoctorPerf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDoctorPerf.Location = new System.Drawing.Point(20, 320);
            this.panelDoctorPerf.Size = new System.Drawing.Size(1260, 400);
            this.panelDoctorPerf.TabIndex = 4;
            // Initialize first!
            this.lblDoctorPerfTitle = new System.Windows.Forms.Label();
          
            this.lblDoctorPerfTitle.Text = "👨‍⚕️ Doctor Performance";
            this.lblDoctorPerfTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblDoctorPerfTitle.ForeColor = System.Drawing.Color.FromArgb(0, 105, 148);
            this.lblDoctorPerfTitle.Location = new System.Drawing.Point(15, 15);
            this.lblDoctorPerfTitle.Size = new System.Drawing.Size(300, 25);

            this.lvDoctorPerformance.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colDocId,
                this.colDocName,
                this.colSpecialization,
                this.colTotalAppts,
                this.colCompletedAppts,
                this.colSuccessRate
            });
            this.lvDoctorPerformance.FullRowSelect = true;
            this.lvDoctorPerformance.GridLines = true;
            this.lvDoctorPerformance.Location = new System.Drawing.Point(20, 50);
            this.lvDoctorPerformance.Size = new System.Drawing.Size(1220, 330);
            this.lvDoctorPerformance.TabIndex = 0;
            this.lvDoctorPerformance.UseCompatibleStateImageBehavior = false;
            this.lvDoctorPerformance.View = System.Windows.Forms.View.Details;

            this.colDocId.Text = "ID";
            this.colDocId.Width = 50;
            this.colDocName.Text = "Doctor Name";
            this.colDocName.Width = 200;
            this.colSpecialization.Text = "Specialization";
            this.colSpecialization.Width = 200;
            this.colTotalAppts.Text = "Total";
            this.colTotalAppts.Width = 80;
            this.colCompletedAppts.Text = "Completed";
            this.colCompletedAppts.Width = 100;
            this.colSuccessRate.Text = "Success Rate";
            this.colSuccessRate.Width = 120;

            this.panelDoctorPerf.Controls.Add(this.lblDoctorPerfTitle);
            this.panelDoctorPerf.Controls.Add(this.lvDoctorPerformance);

            // Add controls to form
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelDaily);
            this.Controls.Add(this.panelMonthly);
            this.Controls.Add(this.panelPatients);
            this.Controls.Add(this.panelDoctorPerf);

            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelDaily.ResumeLayout(false);
            this.panelDaily.PerformLayout();
            this.panelMonthly.ResumeLayout(false);
            this.panelMonthly.PerformLayout();
            this.panelPatients.ResumeLayout(false);
            this.panelPatients.PerformLayout();
            this.panelDoctorPerf.ResumeLayout(false);
            this.panelDoctorPerf.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}