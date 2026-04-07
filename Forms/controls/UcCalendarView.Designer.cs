using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcCalendarView
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
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpNavDate = new System.Windows.Forms.DateTimePicker();
            this.lblColor = new System.Windows.Forms.Label();
            this.cmbColorBy = new System.Windows.Forms.ComboBox();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPageDaily = new System.Windows.Forms.TabPage();
            this.dgvDaily = new System.Windows.Forms.DataGridView();
            this.tabPageWeekly = new System.Windows.Forms.TabPage();
            this.tabPageMonthly = new System.Windows.Forms.TabPage();
            
            this.pnlFilter.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabPageDaily.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDaily)).BeginInit();
            this.SuspendLayout();
            
            // pnlFilter
            this.pnlFilter.BackColor = System.Drawing.Color.White;
            this.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilter.Controls.Add(this.cmbColorBy);
            this.pnlFilter.Controls.Add(this.lblColor);
            this.pnlFilter.Controls.Add(this.dtpNavDate);
            this.pnlFilter.Controls.Add(this.lblDate);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Height = 60;
            this.pnlFilter.Location = new System.Drawing.Point(0, 0);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1000, 60);
            
            // lblDate
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDate.Location = new System.Drawing.Point(20, 22);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(74, 15);
            this.lblDate.Text = "VIEW DATE:";
            
            // dtpNavDate
            this.dtpNavDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNavDate.Location = new System.Drawing.Point(100, 18);
            this.dtpNavDate.Name = "dtpNavDate";
            this.dtpNavDate.Size = new System.Drawing.Size(150, 20);
            this.dtpNavDate.ValueChanged += new System.EventHandler(this.dtpNavDate_ValueChanged);
            
            // lblColor
            this.lblColor.AutoSize = true;
            this.lblColor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblColor.Location = new System.Drawing.Point(280, 22);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(65, 15);
            this.lblColor.Text = "COLOR BY:";
            
            // cmbColorBy
            this.cmbColorBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColorBy.Items.AddRange(new object[] {
            "Doctor Specialty",
            "Appt Status",
            "Urgency"});
            this.cmbColorBy.Location = new System.Drawing.Point(360, 18);
            this.cmbColorBy.Name = "cmbColorBy";
            this.cmbColorBy.Size = new System.Drawing.Size(150, 21);
            this.cmbColorBy.SelectedIndexChanged += new System.EventHandler(this.cmbColorBy_SelectedIndexChanged);
            
            // tabMain
            this.tabMain.Controls.Add(this.tabPageDaily);
            this.tabMain.Controls.Add(this.tabPageWeekly);
            this.tabMain.Controls.Add(this.tabPageMonthly);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabMain.Location = new System.Drawing.Point(0, 60);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1000, 640);
            
            // tabPageDaily
            this.tabPageDaily.Controls.Add(this.dgvDaily);
            this.tabPageDaily.Location = new System.Drawing.Point(4, 26);
            this.tabPageDaily.Name = "DAILY_GRID";
            this.tabPageDaily.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDaily.Size = new System.Drawing.Size(992, 610);
            this.tabPageDaily.Text = "Daily Vertical View";
            this.tabPageDaily.UseVisualStyleBackColor = true;
            
            // dgvDaily
            this.dgvDaily.AutoGenerateColumns = false;
            this.dgvDaily.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDaily.BackgroundColor = System.Drawing.Color.White;
            this.dgvDaily.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDaily.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDaily.ColumnHeadersHeight = 40;
            this.dgvDaily.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.dgvDaily.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvDaily.EnableHeadersVisualStyles = false;
            this.dgvDaily.Columns.Add("Time", "Time");
            this.dgvDaily.Columns.Add("Patient", "Patient");
            this.dgvDaily.Columns.Add("Reason", "Reason");
            this.dgvDaily.Columns.Add("Status", "Status");
            this.dgvDaily.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDaily.Location = new System.Drawing.Point(3, 3);
            this.dgvDaily.Name = "dgvDaily";
            this.dgvDaily.ReadOnly = true;
            this.dgvDaily.RowHeadersVisible = false;
            this.dgvDaily.AllowUserToAddRows = false;
            this.dgvDaily.RowTemplate.Height = 35;
            this.dgvDaily.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDaily.Size = new System.Drawing.Size(986, 604);
            
            // tabPageWeekly
            this.tabPageWeekly.Location = new System.Drawing.Point(4, 26);
            this.tabPageWeekly.Name = "WEEKLY_SLOTS";
            this.tabPageWeekly.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWeekly.Size = new System.Drawing.Size(992, 610);
            this.tabPageWeekly.Text = "Weekly Schedule";
            this.tabPageWeekly.UseVisualStyleBackColor = true;
            
            // tabPageMonthly
            this.tabPageMonthly.Location = new System.Drawing.Point(4, 26);
            this.tabPageMonthly.Name = "MONTHLY";
            this.tabPageMonthly.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMonthly.Size = new System.Drawing.Size(992, 610);
            this.tabPageMonthly.Text = "Month Overview";
            this.tabPageMonthly.UseVisualStyleBackColor = true;
            
            // UcCalendarView
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.pnlFilter);
            this.Name = "UcCalendarView";
            this.Size = new System.Drawing.Size(1000, 700);
            
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabPageDaily.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDaily)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpNavDate;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.ComboBox cmbColorBy;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPageDaily;
        private System.Windows.Forms.DataGridView dgvDaily;
        private System.Windows.Forms.TabPage tabPageWeekly;
        private System.Windows.Forms.TabPage tabPageMonthly;
    }
}
