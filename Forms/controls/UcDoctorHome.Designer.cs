namespace ClinicAppointmentSystem.Controls
{
    partial class UcDoctorHome
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.FlowLayoutPanel pnlCards;
        private System.Windows.Forms.Panel pnlPatientArea;

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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlPatientArea = new System.Windows.Forms.Panel();
            
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            
            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 110;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(30, 20, 30, 0);
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Text = "Doctor Dashboard";
            
            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSubtitle.Location = new System.Drawing.Point(35, 70);
            this.lblSubtitle.Text = "Your daily clinical overview.";
            
            // pnlCards
            this.pnlCards.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCards.Height = 160;
            this.pnlCards.Padding = new System.Windows.Forms.Padding(25, 20, 25, 20);
            this.pnlCards.AutoScroll = true;
            this.pnlCards.WrapContents = false;
            this.pnlCards.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;

            // pnlPatientArea
            this.pnlPatientArea.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlPatientArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPatientArea.Padding = new System.Windows.Forms.Padding(30);

            // UcDoctorHome
            this.Controls.Add(this.pnlPatientArea);
            this.Controls.Add(this.pnlCards);
            this.Controls.Add(this.pnlHeader);
            this.Name = "UcDoctorHome";
            this.Size = new System.Drawing.Size(1000, 750);
            
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
