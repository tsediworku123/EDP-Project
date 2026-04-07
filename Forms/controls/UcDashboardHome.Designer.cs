using System.Windows.Forms;
using System.Drawing;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcDashboardHome
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
            this.components = new System.ComponentModel.Container();
            this.pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlQuickLinks = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlLiveGrid = new System.Windows.Forms.Panel();
            this.lblLiveTitle = new System.Windows.Forms.Label();
            this.dgvRecent = new System.Windows.Forms.DataGridView();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.lblCardsTitle = new System.Windows.Forms.Label();
            this.lblQuickTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecent)).BeginInit();
            this.SuspendLayout();
            
            // lblCardsTitle
            this.lblCardsTitle.Text = "CLINIC PERFORMANCE (REAL-TIME)";
            this.lblCardsTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCardsTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblCardsTitle.Location = new System.Drawing.Point(20, 20);
            this.lblCardsTitle.Size = new System.Drawing.Size(400, 25);

            // pnlCards
            this.pnlCards.Location = new System.Drawing.Point(15, 50);
            this.pnlCards.Width = 1060;
            this.pnlCards.Height = 130;
            this.pnlCards.BackColor = System.Drawing.Color.Transparent;

            // lblQuickTitle
            this.lblQuickTitle.Text = "ADMINISTRATIVE QUICK ACTIONS";
            this.lblQuickTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblQuickTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblQuickTitle.Location = new System.Drawing.Point(20, 195);
            this.lblQuickTitle.Size = new System.Drawing.Size(400, 25);

            // pnlQuickLinks
            this.pnlQuickLinks.Location = new System.Drawing.Point(15, 225);
            this.pnlQuickLinks.Width = 1060;
            this.pnlQuickLinks.Height = 140;
            this.pnlQuickLinks.BackColor = System.Drawing.Color.Transparent;

            // pnlLiveGrid
            this.pnlLiveGrid.Location = new System.Drawing.Point(20, 390);
            this.pnlLiveGrid.Size = new System.Drawing.Size(1060, 390);
            this.pnlLiveGrid.BackColor = System.Drawing.Color.White;
            this.pnlLiveGrid.Padding = new System.Windows.Forms.Padding(1);

            // lblLiveTitle
            this.lblLiveTitle.Text = "LIVE CLINIC OVERVIEW (ALL DOCTORS)";
            this.lblLiveTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLiveTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblLiveTitle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.lblLiveTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLiveTitle.Height = 45;
            this.lblLiveTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLiveTitle.Padding = new System.Windows.Forms.Padding(10,0,0,0);

            // dgvRecent
            this.dgvRecent.BackgroundColor = System.Drawing.Color.White;
            this.dgvRecent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRecent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRecent.ColumnHeadersHeight = 40;
            this.dgvRecent.ReadOnly = true;
            this.dgvRecent.RowHeadersVisible = false;
            this.dgvRecent.AllowUserToAddRows = false;
            this.dgvRecent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecent.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.pnlLiveGrid.Controls.Add(this.dgvRecent);
            this.pnlLiveGrid.Controls.Add(this.lblLiveTitle);

            // UcDashboardHome
            this.Controls.Add(this.pnlLiveGrid);
            this.Controls.Add(this.lblQuickTitle);
            this.Controls.Add(this.pnlQuickLinks);
            this.Controls.Add(this.lblCardsTitle);
            this.Controls.Add(this.pnlCards);
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Name = "UcDashboardHome";
            this.Size = new System.Drawing.Size(1100, 800);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecent)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.FlowLayoutPanel pnlCards;
        private System.Windows.Forms.FlowLayoutPanel pnlQuickLinks;
        private System.Windows.Forms.Panel pnlLiveGrid;
        private System.Windows.Forms.Label lblLiveTitle;
        private System.Windows.Forms.Label lblCardsTitle;
        private System.Windows.Forms.Label lblQuickTitle;
        private System.Windows.Forms.DataGridView dgvRecent;
        private System.Windows.Forms.Timer timerRefresh;
    }
}
