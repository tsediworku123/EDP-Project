using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem
{
    partial class ReceptionDashboardForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ContextMenuStrip ctxMenuQueue;
        private System.Windows.Forms.TextBox txtGlobalSearch;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ctxMenuQueue = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtGlobalSearch = new System.Windows.Forms.TextBox();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblDateStr = new System.Windows.Forms.Label();
            
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlStats = new System.Windows.Forms.FlowLayoutPanel();
            
            this.pnlQueue = new System.Windows.Forms.Panel();
            this.lblQTitle = new System.Windows.Forms.Label();
            this.dgvQueue = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPatient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDoctor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckIn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colComplete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colCancel = new System.Windows.Forms.DataGridViewButtonColumn();

            this.pnlSidebar.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlQueue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).BeginInit();
            this.SuspendLayout();
            
            // CONTEXT MENU
            System.Windows.Forms.ToolStripMenuItem menuCheckIn = new System.Windows.Forms.ToolStripMenuItem();
            menuCheckIn.Text = " Check-In Patient";
            menuCheckIn.Click += new System.EventHandler(this.HandleCheckIn_Click);
            
            System.Windows.Forms.ToolStripMenuItem menuComplete = new System.Windows.Forms.ToolStripMenuItem();
            menuComplete.Text = " Mark Completed";
            menuComplete.Click += new System.EventHandler(this.HandleComplete_Click);

            System.Windows.Forms.ToolStripMenuItem menuCancel = new System.Windows.Forms.ToolStripMenuItem();
            menuCancel.Text = " Cancel Appointment";
            menuCancel.Click += new System.EventHandler(this.HandleCancel_Click);

            this.ctxMenuQueue.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { menuCheckIn, menuComplete, new System.Windows.Forms.ToolStripSeparator(), menuCancel });


            // SIDEBAR
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Width = 240;

            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblLogo.Text = " CLINIC PRO";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLogo.Height = 80;

            this.btnNavFindPatient = new System.Windows.Forms.Button();
            this.btnNavFindPatient.Text = "      PATIENT RECORDS (F1)";
            this.btnNavFindPatient.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavFindPatient.Height = 55;
            this.btnNavFindPatient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavFindPatient.FlatAppearance.BorderSize = 0;
            this.btnNavFindPatient.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.btnNavFindPatient.ForeColor = System.Drawing.Color.FromArgb(200, 210, 220);
            this.btnNavFindPatient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavFindPatient.Padding = new Padding(25, 0, 0, 0);
            this.btnNavFindPatient.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            this.btnNavNewBooking = new System.Windows.Forms.Button();
            this.btnNavNewBooking.Text = "      NEW BOOKING (F2)";
            this.btnNavNewBooking.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavNewBooking.Height = 55;
            this.btnNavNewBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavNewBooking.FlatAppearance.BorderSize = 0;
            this.btnNavNewBooking.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.btnNavNewBooking.ForeColor = System.Drawing.Color.FromArgb(200, 210, 220);
            this.btnNavNewBooking.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavNewBooking.Padding = new Padding(25, 0, 0, 0);
            this.btnNavNewBooking.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            this.btnNavSchedule = new System.Windows.Forms.Button();
            this.btnNavSchedule.Text = "      MANAGE SCHEDULE (F3)";
            this.btnNavSchedule.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavSchedule.Height = 55;
            this.btnNavSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavSchedule.FlatAppearance.BorderSize = 0;
            this.btnNavSchedule.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.btnNavSchedule.ForeColor = System.Drawing.Color.FromArgb(200, 210, 220);
            this.btnNavSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavSchedule.Padding = new Padding(25, 0, 0, 0);
            this.btnNavSchedule.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            this.btnNavDoctorAvailability = new System.Windows.Forms.Button();
            this.btnNavDoctorAvailability.Text = "      DOCTOR STATUS (F4)";
            this.btnNavDoctorAvailability.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavDoctorAvailability.Height = 55;
            this.btnNavDoctorAvailability.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavDoctorAvailability.FlatAppearance.BorderSize = 0;
            this.btnNavDoctorAvailability.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.btnNavDoctorAvailability.ForeColor = System.Drawing.Color.FromArgb(200, 210, 220);
            this.btnNavDoctorAvailability.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavDoctorAvailability.Padding = new Padding(25, 0, 0, 0);
            this.btnNavDoctorAvailability.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            this.btnLogout = new System.Windows.Forms.Button();
            this.btnLogout.Text = "EXIT SYSTEM";
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.Height = 60;
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            
            this.pnlSidebar.Controls.Add(this.btnLogout);
            this.pnlSidebar.Controls.Add(this.btnNavDoctorAvailability);
            this.pnlSidebar.Controls.Add(this.btnNavSchedule);
            this.pnlSidebar.Controls.Add(this.btnNavNewBooking);
            this.pnlSidebar.Controls.Add(this.btnNavFindPatient);
            this.pnlSidebar.Controls.Add(this.lblLogo);

            // MAIN AREA
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(240, 244, 248);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Controls.Add(this.pnlContent);
            this.pnlMain.Controls.Add(this.pnlHeader);

            // HEADER (Welcome Banner)
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 110;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(30, 20, 30, 0);

            this.lblWelcome.Text = "Welcome Back, FRONT DESK ";
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Location = new System.Drawing.Point(30, 25);

            this.lblDateStr.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy");
            this.lblDateStr.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblDateStr.ForeColor = System.Drawing.Color.FromArgb(220, 240, 255);
            this.lblDateStr.AutoSize = true;
            this.lblDateStr.Location = new System.Drawing.Point(35, 65);

            this.txtGlobalSearch.Width = 350;
            this.txtGlobalSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtGlobalSearch.Location = new System.Drawing.Point(550, 40);
            this.txtGlobalSearch.Name = "txtGlobalSearch";

            this.pnlHeader.Controls.Add(this.lblWelcome);
            this.pnlHeader.Controls.Add(this.lblDateStr);
            this.pnlHeader.Controls.Add(this.txtGlobalSearch);

            // CONTENT
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoScroll = true;
            this.pnlContent.Padding = new System.Windows.Forms.Padding(20);
            this.pnlContent.Controls.Add(this.pnlQueue);
            this.pnlContent.Controls.Add(this.pnlStats);

            // STATS PANEL (Horizontal Flow)
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStats.Height = 120;
            this.pnlStats.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.pnlStats.WrapContents = false;

            // QUEUE LISTING
            this.pnlQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQueue.BackColor = System.Drawing.Color.White;
            this.pnlQueue.Padding = new System.Windows.Forms.Padding(20);
            this.pnlQueue.Margin = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.pnlQueue.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.lblQTitle.Text = " TODAY'S APPOINTMENT QUEUE";
            this.lblQTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblQTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblQTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblQTitle.Height = 50;

            this.dgvQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvQueue.BackgroundColor = System.Drawing.Color.White;
            this.dgvQueue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvQueue.RowHeadersVisible = false;
            this.dgvQueue.AllowUserToAddRows = false;
            this.dgvQueue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvQueue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvQueue.ContextMenuStrip = this.ctxMenuQueue;
            this.dgvQueue.RowTemplate.Height = 50;
            this.dgvQueue.ColumnHeadersHeight = 40;
            this.dgvQueue.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvQueue.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(244, 247, 252);
            this.dgvQueue.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.dgvQueue.EnableHeadersVisualStyles = false;

            this.colTime.Name = "Time"; this.colTime.HeaderText = "SCHEDULED"; 
            this.colPatient.Name = "Patient"; this.colPatient.HeaderText = "PATIENT NAME";
            this.colDoctor.Name = "Doctor"; this.colDoctor.HeaderText = "ASSIGNED DOCTOR";
            this.colStatus.Name = "Status"; this.colStatus.HeaderText = "STATUS";

            this.colCheckIn.Name = "CheckIn"; this.colCheckIn.HeaderText = "ACTION"; this.colCheckIn.Text = "CHECK-IN"; this.colCheckIn.UseColumnTextForButtonValue = true;
            this.colComplete.Name = "Complete"; this.colComplete.HeaderText = "ACTION"; this.colComplete.Text = "COMPLETE"; this.colComplete.UseColumnTextForButtonValue = true;
            this.colCancel.Name = "Cancel"; this.colCancel.HeaderText = "ACTION"; this.colCancel.Text = "CANCEL"; this.colCancel.UseColumnTextForButtonValue = true;

            this.dgvQueue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colTime, this.colPatient, this.colDoctor, this.colStatus, this.colCheckIn, this.colComplete, this.colCancel
            });

            this.pnlQueue.Controls.Add(this.dgvQueue);
            this.pnlQueue.Controls.Add(this.lblQTitle);

            // FORM SETTINGS
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 800);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSidebar);
            this.Name = "ReceptionDashboardForm";
            this.Text = "Clinic Front Desk - Management Dashboard";
            
            // Wiring
            this.btnNavFindPatient.Click += new System.EventHandler(this.btnNavFindPatient_Click);
            this.btnNavNewBooking.Click += new System.EventHandler(this.btnNavNewBooking_Click);
            this.btnNavSchedule.Click += new System.EventHandler(this.btnNavSchedule_Click);
            this.btnNavDoctorAvailability.Click += new System.EventHandler(this.btnNavDoctorAvailability_Click);
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            this.dgvQueue.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvQueue_CellContentClick);

            this.pnlSidebar.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlQueue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button StyleSidebarButton(System.Windows.Forms.Button btn, string text, int y)
        {
            btn = new System.Windows.Forms.Button();
            btn.Text = text;
            btn.Dock = System.Windows.Forms.DockStyle.Top;
            btn.Height = 55;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            btn.ForeColor = System.Drawing.Color.FromArgb(200, 210, 220);
            btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(25, 0, 0, 0);
            btn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            return btn;
        }

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Button btnNavFindPatient;
        private System.Windows.Forms.Button btnNavNewBooking;
        private System.Windows.Forms.Button btnNavSchedule;
        private System.Windows.Forms.Button btnNavDoctorAvailability;
        private System.Windows.Forms.Button btnLogout;
        
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblDateStr;

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.FlowLayoutPanel pnlStats;
        
        private System.Windows.Forms.Panel pnlQueue;
        private System.Windows.Forms.Label lblQTitle;
        private System.Windows.Forms.DataGridView dgvQueue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDoctor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewButtonColumn colCheckIn;
        private System.Windows.Forms.DataGridViewButtonColumn colComplete;
        private System.Windows.Forms.DataGridViewButtonColumn colCancel;
    }
}
