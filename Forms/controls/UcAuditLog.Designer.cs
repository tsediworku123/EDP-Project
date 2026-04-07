using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcAuditLog
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
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmbModule = new System.Windows.Forms.ComboBox();
            this.pGridWrap = new System.Windows.Forms.Panel();
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblModuleTitle = new System.Windows.Forms.Label();
            
            this.pnlFilters.SuspendLayout();
            this.pGridWrap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(25, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(434, 30);
            this.lblTitle.Text = "SYSTEM AUDIT TRAIL && COMPLIANCE LOG";
            
            // pnlFilters
            this.pnlFilters.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            this.pnlFilters.BackColor = System.Drawing.Color.White;
            this.pnlFilters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pnlFilters.Controls.Add(this.lblModuleTitle);
            this.pnlFilters.Controls.Add(this.cmbModule);
            this.pnlFilters.Controls.Add(this.txtSearch);
            this.pnlFilters.Controls.Add(this.lblSearchIcon);
            this.pnlFilters.Location = new System.Drawing.Point(25, 75);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(980, 60);
            
            // lblSearchIcon
            this.lblSearchIcon.AutoSize = true;
            this.lblSearchIcon.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSearchIcon.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblSearchIcon.Location = new System.Drawing.Point(15, 18);
            this.lblSearchIcon.Text = "\uD83D\uDD0D";
            
            // txtSearch
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSearch.Location = new System.Drawing.Point(45, 18);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(400, 22);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            
            // lblModuleTitle
            this.lblModuleTitle.AutoSize = true;
            this.lblModuleTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblModuleTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblModuleTitle.Location = new System.Drawing.Point(650, 22);
            this.lblModuleTitle.Text = "FILTER BY MODULE:";

            // cmbModule
            this.cmbModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModule.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbModule.Items.AddRange(new object[] {
            "All Modules",
            "Security",
            "Doctors",
            "Patients",
            "Appointments",
            "Clinical",
            "Settings"});
            this.cmbModule.Location = new System.Drawing.Point(780, 18);
            this.cmbModule.Name = "cmbModule";
            this.cmbModule.Size = new System.Drawing.Size(180, 25);
            this.cmbModule.SelectedIndexChanged += new System.EventHandler(this.cmbModule_SelectedIndexChanged);
            
            // pGridWrap
            this.pGridWrap.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            this.pGridWrap.BackColor = System.Drawing.Color.White;
            this.pGridWrap.Controls.Add(this.dgvLogs);
            this.pGridWrap.Location = new System.Drawing.Point(25, 155);
            this.pGridWrap.Name = "pGridWrap";
            this.pGridWrap.Size = new System.Drawing.Size(980, 505);
            
            // dgvLogs
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLogs.BackgroundColor = System.Drawing.Color.White;
            this.dgvLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLogs.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvLogs.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.dgvLogs.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvLogs.EnableHeadersVisualStyles = false;
            this.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLogs.ColumnHeadersHeight = 45;
            this.dgvLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDate,
            this.colUser,
            this.colAction,
            this.colModule,
            this.colIP});
            this.dgvLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLogs.Location = new System.Drawing.Point(0, 0);
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogs.RowTemplate.Height = 35;
            this.dgvLogs.GridColor = System.Drawing.Color.FromArgb(241, 245, 249);
            
            // colDate
            this.colDate.HeaderText = "Timestamp";
            this.colDate.Name = "Date";
            this.colDate.FillWeight = 80;
            
            // colUser
            this.colUser.HeaderText = "Authorized User";
            this.colUser.Name = "User";
            this.colUser.FillWeight = 80;
            
            // colAction
            this.colAction.HeaderText = "Operation Detail";
            this.colAction.Name = "Action";
            this.colAction.FillWeight = 150;
            
            // colModule
            this.colModule.HeaderText = "System Module";
            this.colModule.Name = "Module";
            this.colModule.FillWeight = 80;
            
            // colIP
            this.colIP.HeaderText = "Client Source";
            this.colIP.Name = "IP";
            this.colIP.FillWeight = 70;
            
            // UcAuditLog
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.pGridWrap);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.lblTitle);
            this.Name = "UcAuditLog";
            this.Size = new System.Drawing.Size(1030, 700);
            
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.pGridWrap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbModule;
        private System.Windows.Forms.Label lblModuleTitle;
        private System.Windows.Forms.Panel pGridWrap;
        private System.Windows.Forms.DataGridView dgvLogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModule;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIP;
    }
}
