namespace ClinicAppointmentSystem
{
    partial class DoctorDashboard
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblClock;
        private System.Windows.Forms.Label lblNotifications;
        private System.Windows.Forms.Button btnToggleSidebar;
        
        // Sidebar Buttons (Modern Slate Theme)
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnAppointments;
        private System.Windows.Forms.Button btnQueue;
        private System.Windows.Forms.Button btnNotes;
        private System.Windows.Forms.Button btnReviews;
        private System.Windows.Forms.Button btnAvailability;
        private System.Windows.Forms.Button btnSecurity;
        private System.Windows.Forms.Button btnLogout;

        // Top Bar Labels
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlDocInfo;
        private System.Windows.Forms.Label lblDocName;
        private System.Windows.Forms.Button btnStatus;
        private System.Windows.Forms.Label lblRating;
        private System.Windows.Forms.Button btnLogoutTop;

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
            this.components = new System.ComponentModel.Container();
            this.sidebarPanel = new System.Windows.Forms.Panel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.topBar = new System.Windows.Forms.Panel();
            this.lblClock = new System.Windows.Forms.Label();
            this.lblNotifications = new System.Windows.Forms.Label();
            this.btnToggleSidebar = new System.Windows.Forms.Button();
            
            // Sidebar buttons
            this.btnHome = new System.Windows.Forms.Button();
            this.btnAppointments = new System.Windows.Forms.Button();
            this.btnQueue = new System.Windows.Forms.Button();
            this.btnNotes = new System.Windows.Forms.Button();
            this.btnReviews = new System.Windows.Forms.Button();
            this.btnAvailability = new System.Windows.Forms.Button();
            this.btnSecurity = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();

            // Top bar elements
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlDocInfo = new System.Windows.Forms.Panel();
            this.lblDocName = new System.Windows.Forms.Label();
            this.btnStatus = new System.Windows.Forms.Button();
            this.lblRating = new System.Windows.Forms.Label();
            this.btnLogoutTop = new System.Windows.Forms.Button();

            this.sidebarPanel.SuspendLayout();
            this.topBar.SuspendLayout();
            this.pnlDocInfo.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();

            // sidebarPanel
            this.sidebarPanel.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.sidebarPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarPanel.Width = 240;
            this.sidebarPanel.AutoScroll = true;
            this.sidebarPanel.Name = "sidebarPanel";
            this.sidebarPanel.Controls.Add(this.btnToggleSidebar);
            this.sidebarPanel.Controls.Add(this.btnHome);
            this.sidebarPanel.Controls.Add(this.btnAppointments);
            this.sidebarPanel.Controls.Add(this.btnQueue);
            this.sidebarPanel.Controls.Add(this.btnNotes);
            this.sidebarPanel.Controls.Add(this.btnReviews);
            this.sidebarPanel.Controls.Add(this.btnAvailability);
            this.sidebarPanel.Controls.Add(this.btnSecurity);
            this.sidebarPanel.Controls.Add(this.btnLogout);

            // btnToggleSidebar
            this.btnToggleSidebar.Text = "\u2630";
            this.btnToggleSidebar.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnToggleSidebar.ForeColor = System.Drawing.Color.White;
            this.btnToggleSidebar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleSidebar.FlatAppearance.BorderSize = 0;
            this.btnToggleSidebar.Size = new System.Drawing.Size(40, 40);
            this.btnToggleSidebar.Location = new System.Drawing.Point(10, 10);
            this.btnToggleSidebar.Click += new System.EventHandler(this.btnToggleSidebar_Click);

            this.btnHome.Text = "  \u25A0  Dashboard";
            this.btnHome.Location = new System.Drawing.Point(5, 80);
            this.btnHome.Size = new System.Drawing.Size(230, 45);
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Font = new System.Drawing.Font("Segoe UI Semibold", 9);
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHome.Click += new System.EventHandler(this.SidebarButton_Click);

            this.btnAppointments.Text = "  \u2697  My Appointments";
            this.btnAppointments.Location = new System.Drawing.Point(5, 130);
            this.btnAppointments.Size = new System.Drawing.Size(230, 45);
            this.btnAppointments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAppointments.FlatAppearance.BorderSize = 0;
            this.btnAppointments.ForeColor = System.Drawing.Color.White;
            this.btnAppointments.Font = new System.Drawing.Font("Segoe UI Semibold", 9);
            this.btnAppointments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAppointments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAppointments.Click += new System.EventHandler(this.SidebarButton_Click);

            this.btnQueue.Text = "  \u231A  Patient Queue";
            this.btnQueue.Location = new System.Drawing.Point(5, 180);
            this.btnQueue.Size = new System.Drawing.Size(230, 45);
            this.btnQueue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQueue.FlatAppearance.BorderSize = 0;
            this.btnQueue.ForeColor = System.Drawing.Color.White;
            this.btnQueue.Font = new System.Drawing.Font("Segoe UI Semibold", 9);
            this.btnQueue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQueue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQueue.Click += new System.EventHandler(this.SidebarButton_Click);

            this.btnNotes.Text = "  \u270E  Medical Notes";
            this.btnNotes.Location = new System.Drawing.Point(5, 230);
            this.btnNotes.Size = new System.Drawing.Size(230, 45);
            this.btnNotes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotes.FlatAppearance.BorderSize = 0;
            this.btnNotes.ForeColor = System.Drawing.Color.White;
            this.btnNotes.Font = new System.Drawing.Font("Segoe UI Semibold", 9);
            this.btnNotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNotes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNotes.Click += new System.EventHandler(this.SidebarButton_Click);

            this.btnReviews.Text = "  \u2605  Patient Reviews";
            this.btnReviews.Location = new System.Drawing.Point(5, 280);
            this.btnReviews.Size = new System.Drawing.Size(230, 45);
            this.btnReviews.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReviews.FlatAppearance.BorderSize = 0;
            this.btnReviews.ForeColor = System.Drawing.Color.White;
            this.btnReviews.Font = new System.Drawing.Font("Segoe UI Semibold", 9);
            this.btnReviews.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReviews.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReviews.Click += new System.EventHandler(this.SidebarButton_Click);

            this.btnAvailability.Text = "  \u23F2  Availability";
            this.btnAvailability.Location = new System.Drawing.Point(5, 330);
            this.btnAvailability.Size = new System.Drawing.Size(230, 45);
            this.btnAvailability.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAvailability.FlatAppearance.BorderSize = 0;
            this.btnAvailability.ForeColor = System.Drawing.Color.White;
            this.btnAvailability.Font = new System.Drawing.Font("Segoe UI Semibold", 9);
            this.btnAvailability.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAvailability.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAvailability.Click += new System.EventHandler(this.SidebarButton_Click);

            this.btnSecurity.Text = "  \u26BF  Security";
            this.btnSecurity.Location = new System.Drawing.Point(5, 380);
            this.btnSecurity.Size = new System.Drawing.Size(230, 45);
            this.btnSecurity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSecurity.FlatAppearance.BorderSize = 0;
            this.btnSecurity.ForeColor = System.Drawing.Color.White;
            this.btnSecurity.Font = new System.Drawing.Font("Segoe UI Semibold", 9);
            this.btnSecurity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSecurity.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSecurity.Click += new System.EventHandler(this.SidebarButton_Click);

            this.btnLogout.Text = "  \u2716  Logout";
            this.btnLogout.Location = new System.Drawing.Point(5, 600);
            this.btnLogout.Size = new System.Drawing.Size(230, 45);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(252, 165, 165);
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI Semibold", 9);
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.Click += new System.EventHandler(this.SidebarButton_Click);
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;

            // topBar
            this.topBar.BackColor = System.Drawing.Color.White;
            this.topBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Height = 65;
            this.topBar.Controls.Add(this.lblTitle);
            this.topBar.Controls.Add(this.lblClock);
            this.topBar.Controls.Add(this.pnlDocInfo);

            // lblTitle
            this.lblTitle.Text = "CLINICAL MONITOR";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.AutoSize = true;

            // pnlDocInfo
            this.pnlDocInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDocInfo.Width = 520;
            this.pnlDocInfo.Controls.Add(this.lblDocName);
            this.pnlDocInfo.Controls.Add(this.btnStatus);
            this.pnlDocInfo.Controls.Add(this.lblRating);
            this.pnlDocInfo.Controls.Add(this.lblNotifications);

            // lblDocName
            this.lblDocName.Text = "Dr. Physician";
            this.lblDocName.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            this.lblDocName.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblDocName.Location = new System.Drawing.Point(10, 18);
            this.lblDocName.AutoSize = true;

            // btnStatus
            this.btnStatus.Text = "🟢 Available";
            this.btnStatus.Location = new System.Drawing.Point(180, 14);
            this.btnStatus.Size = new System.Drawing.Size(120, 32);
            this.btnStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatus.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.btnStatus.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.btnStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 8);
            this.btnStatus.Click += new System.EventHandler(this.btnStatus_Click);

            // lblRating
            this.lblRating.Text = "⭐ 0.0 (0 reviews)";
            this.lblRating.Font = new System.Drawing.Font("Segoe UI Semibold", 9);
            this.lblRating.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.lblRating.Location = new System.Drawing.Point(315, 18);
            this.lblRating.AutoSize = true;

            // lblNotifications
            this.lblNotifications.Text = " 0";
            this.lblNotifications.ForeColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.lblNotifications.Location = new System.Drawing.Point(450, 18);
            this.lblNotifications.AutoSize = true;

            // lblClock
            this.lblClock.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblClock.Width = 200;
            this.lblClock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblClock.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblClock.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);

            // contentPanel
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.contentPanel.Padding = new System.Windows.Forms.Padding(20);
            this.contentPanel.AutoScroll = true;

            // mainPanel
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Controls.Add(this.contentPanel);
            this.mainPanel.Controls.Add(this.topBar);

            // Final Form Setup
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1300, 750);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.sidebarPanel);
            
            this.sidebarPanel.ResumeLayout(false);
            this.topBar.ResumeLayout(false);
            this.pnlDocInfo.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void StyleSidebarButton(System.Windows.Forms.Button b, string txt, int y)
        {
            b.Text = txt;
            b.Location = new System.Drawing.Point(5, y);
            b.Size = new System.Drawing.Size(230, 45);
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.ForeColor = System.Drawing.Color.White;
            b.Font = new System.Drawing.Font("Segoe UI Semibold", 9);
            b.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            b.Cursor = System.Windows.Forms.Cursors.Hand;
            b.Click += new System.EventHandler(this.SidebarButton_Click);
        }
    }
}
