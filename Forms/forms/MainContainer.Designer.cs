namespace ClinicAppointmentSystem
{
    partial class MainContainer
    {
        private System.ComponentModel.IContainer components = null;

        public System.Windows.Forms.MenuStrip menuStrip;
        public System.Windows.Forms.StatusStrip statusStrip;
        public System.Windows.Forms.ToolStripStatusLabel lblStatus;
        public System.Windows.Forms.ToolStripStatusLabel lblUser;

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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUser = new System.Windows.Forms.ToolStripStatusLabel();

            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Clinic Management System";
            this.ClientSize = new System.Drawing.Size(1400, 850);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.IsMdiContainer = true;
            this.BackColor = System.Drawing.Color.FromArgb(240, 248, 255);
            this.Name = "MainContainer";

            // MenuStrip
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";

            // StatusStrip
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblStatus,
                new System.Windows.Forms.ToolStripStatusLabel(" | "),
                this.lblUser
            });
            this.statusStrip.Location = new System.Drawing.Point(0, 676);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1200, 22);
            this.statusStrip.TabIndex = 1;

            this.lblStatus.Text = "Ready";
            this.lblUser.Text = "Not logged in";

            this.MainMenuStrip = this.menuStrip;
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.statusStrip);

            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
