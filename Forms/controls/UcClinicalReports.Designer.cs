using System.Windows.Forms;
using System.Drawing;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcClinicalReports
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.scrollContainer = new System.Windows.Forms.Panel();
            this.pnlHeaderTools = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.pnlHeaderTools.SuspendLayout();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.Text = "CLINICAL PERFORMANCE & ANALYTICS";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(25, 20);
            this.lblTitle.Size = new System.Drawing.Size(500, 35);

            // pnlHeaderTools
            this.pnlHeaderTools.Controls.Add(this.btnPrint);
            this.pnlHeaderTools.Controls.Add(this.btnExport);
            this.pnlHeaderTools.Location = new System.Drawing.Point(740, 20);
            this.pnlHeaderTools.Size = new System.Drawing.Size(320, 40);
            this.pnlHeaderTools.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);

            // btnPrint
            this.btnPrint.Text = " PRINT REPORT";
            this.btnPrint.BackColor = System.Drawing.Color.White;
            this.btnPrint.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrint.Location = new System.Drawing.Point(10, 0);
            this.btnPrint.Size = new System.Drawing.Size(140, 35);
            this.btnPrint.Click += new System.EventHandler(this.btnPrintSummary_Click);

            // btnExport
            this.btnExport.Text = " EXPORT DATA";
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(160, 0);
            this.btnExport.Size = new System.Drawing.Size(140, 35);
            this.btnExport.Click += new System.EventHandler(this.btnExportCSV_Click);

            // scrollContainer
            this.scrollContainer.AutoScroll = true;
            this.scrollContainer.Location = new System.Drawing.Point(20, 75);
            this.scrollContainer.Size = new System.Drawing.Size(1040, 600);
            this.scrollContainer.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            this.scrollContainer.BackColor = System.Drawing.Color.Transparent;

            // UcClinicalReports
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlHeaderTools);
            this.Controls.Add(this.scrollContainer);
            this.Size = new System.Drawing.Size(1080, 700);
            this.pnlHeaderTools.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel scrollContainer;
        private System.Windows.Forms.Panel pnlHeaderTools;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExport;
    }
}
