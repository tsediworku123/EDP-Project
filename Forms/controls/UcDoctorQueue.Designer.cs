using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcDoctorQueue
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (refreshTimer != null) {
                refreshTimer.Stop();
                refreshTimer.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.dtpFilter = new System.Windows.Forms.DateTimePicker();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnCallNext = new System.Windows.Forms.Button();
            this.btnNoShow = new System.Windows.Forms.Button();
            this.dgvQueue = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPatient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActionStart = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ActionSkip = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ActionDone = new System.Windows.Forms.DataGridViewButtonColumn();
            
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).BeginInit();
            this.SuspendLayout();
            
            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.btnNoShow);
            this.pnlHeader.Controls.Add(this.btnCallNext);
            this.pnlHeader.Controls.Add(this.btnRefresh);
            this.pnlHeader.Controls.Add(this.dtpFilter);
            this.pnlHeader.Controls.Add(this.lblCount);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20);
            this.pnlHeader.Size = new System.Drawing.Size(1000, 80);
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(262, 25);
            this.lblTitle.Text = "LIVE PATIENT QUEUE (TODAY)";
            
            // lblCount
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblCount.Location = new System.Drawing.Point(20, 45);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(149, 15);
            this.lblCount.Text = "Total Today: 0 | Waiting: 0";
            
            // dtpFilter
            this.dtpFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFilter.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFilter.Location = new System.Drawing.Point(220, 42);
            this.dtpFilter.Name = "dtpFilter";
            this.dtpFilter.Size = new System.Drawing.Size(150, 23);
            this.dtpFilter.ValueChanged += new System.EventHandler(this.dtpFilter_ValueChanged);
            
            // btnRefresh
            this.btnRefresh.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(860, 25);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 35);
            this.btnRefresh.Text = " REFRESH";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            
            // btnCallNext
            this.btnCallNext.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.btnCallNext.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnCallNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCallNext.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnCallNext.ForeColor = System.Drawing.Color.White;
            this.btnCallNext.Location = new System.Drawing.Point(700, 25);
            this.btnCallNext.Name = "btnCallNext";
            this.btnCallNext.Size = new System.Drawing.Size(150, 35);
            this.btnCallNext.Text = " CALL NEXT PATIENT";
            this.btnCallNext.UseVisualStyleBackColor = false;
            this.btnCallNext.Click += new System.EventHandler(this.btnCallNext_Click);
            
            // btnNoShow
            this.btnNoShow.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.btnNoShow.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnNoShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNoShow.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnNoShow.ForeColor = System.Drawing.Color.White;
            this.btnNoShow.Location = new System.Drawing.Point(540, 25);
            this.btnNoShow.Name = "btnNoShow";
            this.btnNoShow.Size = new System.Drawing.Size(150, 35);
            this.btnNoShow.Text = " NO-SHOW";
            this.btnNoShow.UseVisualStyleBackColor = false;
            this.btnNoShow.Click += new System.EventHandler(this.btnNoShow_Click);
            
            // dgvQueue
            this.dgvQueue.AllowUserToAddRows = false;
            this.dgvQueue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvQueue.BackgroundColor = System.Drawing.Color.White;
            this.dgvQueue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvQueue.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.dgvQueue.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvQueue.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.dgvQueue.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.dgvQueue.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dgvQueue.EnableHeadersVisualStyles = false;
            this.dgvQueue.ColumnHeadersHeight = 45;
            this.dgvQueue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colTime,
            this.colPatient,
            this.colType,
            this.colReason,
            this.colStatus,
            this.ActionStart,
            this.ActionSkip,
            this.ActionDone});
            this.dgvQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvQueue.GridColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.dgvQueue.Location = new System.Drawing.Point(0, 80);
            this.dgvQueue.Name = "dgvQueue";
            this.dgvQueue.ReadOnly = true;
            this.dgvQueue.RowHeadersVisible = false;
            this.dgvQueue.RowTemplate.Height = 50;
            this.dgvQueue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvQueue.Size = new System.Drawing.Size(1000, 520);
            this.dgvQueue.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvQueue_CellContentClick);
            this.dgvQueue.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvQueue_CellFormatting);
            
            // colId
            this.colId.HeaderText = "ID";
            this.colId.Name = "Id";
            this.colId.Visible = false;
            
            // colTime
            this.colTime.HeaderText = "TIME";
            this.colTime.Name = "Time";
            
            // colPatient
            this.colPatient.HeaderText = "PATIENT NAME";
            this.colPatient.Name = "Patient";
            
            // colType
            this.colType.HeaderText = "APPT. TYPE";
            this.colType.Name = "Type";
            
            // colReason
            this.colReason.HeaderText = "REASON";
            this.colReason.Name = "Reason";
            
            // colStatus
            this.colStatus.HeaderText = "STATUS";
            this.colStatus.Name = "Status";
            
            // ActionStart
            this.ActionStart.HeaderText = "";
            this.ActionStart.Name = "ActionStart";
            this.ActionStart.Text = " START";
            this.ActionStart.UseColumnTextForButtonValue = true;
            this.ActionStart.Width = 90;
            
            // ActionSkip
            this.ActionSkip.HeaderText = "";
            this.ActionSkip.Name = "ActionSkip";
            this.ActionSkip.Text = " SKIP";
            this.ActionSkip.UseColumnTextForButtonValue = true;
            this.ActionSkip.Width = 90;
            
            // ActionDone
            this.ActionDone.HeaderText = "";
            this.ActionDone.Name = "ActionDone";
            this.ActionDone.Text = " COMPLETE";
            this.ActionDone.UseColumnTextForButtonValue = true;
            this.ActionDone.Width = 110;
            
            // UcDoctorQueue
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.dgvQueue);
            this.Controls.Add(this.pnlHeader);
            this.Name = "UcDoctorQueue";
            this.Size = new System.Drawing.Size(1000, 600);
            
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.DateTimePicker dtpFilter;
        private System.Windows.Forms.Button btnNoShow;
        private System.Windows.Forms.Button btnCallNext;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvQueue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReason;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewButtonColumn ActionStart;
        private System.Windows.Forms.DataGridViewButtonColumn ActionSkip;
        private System.Windows.Forms.DataGridViewButtonColumn ActionDone;
    }
}
