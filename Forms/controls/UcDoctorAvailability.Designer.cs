namespace ClinicAppointmentSystem.Controls
{
    partial class UcDoctorAvailability
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatusTitle;
        private System.Windows.Forms.Button btnToggleStatus;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblBlockTitle;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.ComboBox cmbShift;
        private System.Windows.Forms.MaskedTextBox txtStartTime;
        private System.Windows.Forms.MaskedTextBox txtEndTime;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Button btnDecrease;
        private System.Windows.Forms.Button btnIncrease;
        private System.Windows.Forms.Button btnAddBlock;
        private System.Windows.Forms.DataGridView dgvBlocked;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBlockedTime;
        private System.Windows.Forms.DataGridViewButtonColumn colAction;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatusTitle = new System.Windows.Forms.Label();
            this.btnToggleStatus = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblBlockTitle = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.cmbShift = new System.Windows.Forms.ComboBox();
            this.txtStartTime = new System.Windows.Forms.MaskedTextBox();
            this.txtEndTime = new System.Windows.Forms.MaskedTextBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.btnDecrease = new System.Windows.Forms.Button();
            this.btnIncrease = new System.Windows.Forms.Button();
            this.btnAddBlock = new System.Windows.Forms.Button();
            this.dgvBlocked = new System.Windows.Forms.DataGridView();
            this.colBlockedTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlStatus.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlocked)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlStatus
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.White;
            this.pnlStatus.Controls.Add(this.btnToggleStatus);
            this.pnlStatus.Controls.Add(this.lblStatusTitle);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatus.Location = new System.Drawing.Point(0, 0);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(800, 110);
            this.pnlStatus.TabIndex = 0;
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.AutoSize = true;
            this.lblStatusTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatusTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblStatusTitle.Location = new System.Drawing.Point(30, 20);
            this.lblStatusTitle.Name = "lblStatusTitle";
            this.lblStatusTitle.Size = new System.Drawing.Size(164, 15);
            this.lblStatusTitle.TabIndex = 0;
            this.lblStatusTitle.Text = "CURRENT CLINICAL STATUS";
            // 
            // btnToggleStatus
            // 
            this.btnToggleStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToggleStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToggleStatus.Location = new System.Drawing.Point(30, 48);
            this.btnToggleStatus.Name = "btnToggleStatus";
            this.btnToggleStatus.Size = new System.Drawing.Size(280, 42);
            this.btnToggleStatus.TabIndex = 1;
            this.btnToggleStatus.Text = "🟢 AVAILABLE FOR BOOKING";
            this.btnToggleStatus.UseVisualStyleBackColor = true;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.pnlMain.Controls.Add(this.dgvBlocked);
            this.pnlMain.Controls.Add(this.btnDecrease);
            this.pnlMain.Controls.Add(this.txtEndTime);
            this.pnlMain.Controls.Add(this.btnIncrease);
            this.pnlMain.Controls.Add(this.btnAddBlock);
            this.pnlMain.Controls.Add(this.lblTo);
            this.pnlMain.Controls.Add(this.lblFrom);
            this.pnlMain.Controls.Add(this.txtStartTime);
            this.pnlMain.Controls.Add(this.cmbShift);
            this.pnlMain.Controls.Add(this.dtpDate);
            this.pnlMain.Controls.Add(this.lblBlockTitle);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 110);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(30);
            this.pnlMain.Size = new System.Drawing.Size(960, 540);
            this.pnlMain.TabIndex = 1;
            // 
            // lblBlockTitle
            // 
            this.lblBlockTitle.AutoSize = true;
            this.lblBlockTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBlockTitle.Location = new System.Drawing.Point(30, 25);
            this.lblBlockTitle.Name = "lblBlockTitle";
            this.lblBlockTitle.Size = new System.Drawing.Size(227, 21);
            this.lblBlockTitle.TabIndex = 0;
            this.lblBlockTitle.Text = "BLOCK CLINICAL TIME SLOTS";
            // 
            // dtpDate
            // 
            this.dtpDate.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(30, 70);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(130, 27);
            this.dtpDate.TabIndex = 1;
            // 
            // cmbShift
            // 
            this.cmbShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShift.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbShift.FormattingEnabled = true;
            this.cmbShift.Items.AddRange(new object[] {
            "Morning (08-02)",
            "Afternoon (02-08)",
            "Evening (06-11)"});
            this.cmbShift.Location = new System.Drawing.Point(170, 70);
            this.cmbShift.Name = "cmbShift";
            this.cmbShift.Size = new System.Drawing.Size(220, 28);
            this.cmbShift.TabIndex = 2;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblFrom.Location = new System.Drawing.Point(405, 55);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 13);
            this.lblFrom.TabIndex = 3;
            this.lblFrom.Text = "FROM";
            // 
            // txtStartTime
            // 
            this.txtStartTime.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtStartTime.Location = new System.Drawing.Point(405, 70);
            this.txtStartTime.Mask = "00:00";
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.Size = new System.Drawing.Size(110, 27);
            this.txtStartTime.TabIndex = 4;
            this.txtStartTime.Text = "0800";
            this.txtStartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStartTime.ValidatingType = typeof(System.DateTime);
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTo.Location = new System.Drawing.Point(525, 55);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(22, 13);
            this.lblTo.TabIndex = 5;
            this.lblTo.Text = "TO";
            // 
            // btnDecrease
            // 
            this.btnDecrease.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDecrease.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDecrease.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.btnDecrease.Location = new System.Drawing.Point(525, 70);
            this.btnDecrease.Name = "btnDecrease";
            this.btnDecrease.Size = new System.Drawing.Size(28, 28);
            this.btnDecrease.TabIndex = 10;
            this.btnDecrease.Text = "-";
            this.btnDecrease.UseVisualStyleBackColor = true;
            // 
            // txtEndTime
            // 
            this.txtEndTime.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtEndTime.Location = new System.Drawing.Point(555, 70);
            this.txtEndTime.Mask = "00:00";
            this.txtEndTime.Name = "txtEndTime";
            this.txtEndTime.Size = new System.Drawing.Size(110, 27);
            this.txtEndTime.TabIndex = 6;
            this.txtEndTime.Text = "0830";
            this.txtEndTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEndTime.ValidatingType = typeof(System.DateTime);
            // 
            // btnIncrease
            // 
            this.btnIncrease.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIncrease.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnIncrease.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.btnIncrease.Location = new System.Drawing.Point(665, 70);
            this.btnIncrease.Name = "btnIncrease";
            this.btnIncrease.Size = new System.Drawing.Size(28, 28);
            this.btnIncrease.TabIndex = 11;
            this.btnIncrease.Text = "+";
            this.btnIncrease.UseVisualStyleBackColor = true;
            // 
            // btnAddBlock
            // 
            this.btnAddBlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnAddBlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddBlock.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnAddBlock.ForeColor = System.Drawing.Color.White;
            this.btnAddBlock.Location = new System.Drawing.Point(715, 68);
            this.btnAddBlock.Name = "btnAddBlock";
            this.btnAddBlock.Size = new System.Drawing.Size(110, 32);
            this.btnAddBlock.TabIndex = 7;
            this.btnAddBlock.Text = "ADD BLOCK";
            this.btnAddBlock.UseVisualStyleBackColor = false;
            // 
            // dgvBlocked
            // 
            this.dgvBlocked.AllowUserToAddRows = false;
            this.dgvBlocked.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBlocked.BackgroundColor = System.Drawing.Color.White;
            this.dgvBlocked.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBlocked.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBlocked.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBlockedTime,
            this.colAction});
            this.dgvBlocked.Location = new System.Drawing.Point(30, 130);
            this.dgvBlocked.Name = "dgvBlocked";
            this.dgvBlocked.RowHeadersVisible = false;
            this.dgvBlocked.RowTemplate.Height = 40;
            this.dgvBlocked.Size = new System.Drawing.Size(740, 380);
            this.dgvBlocked.TabIndex = 8;
            // 
            // colBlockedTime
            // 
            this.colBlockedTime.HeaderText = "BLOCKED TIME SLOT";
            this.colBlockedTime.Name = "TimeSlot";
            // 
            // colAction
            // 
            this.colAction.HeaderText = "";
            this.colAction.Name = "Action";
            this.colAction.Text = "REMOVE";
            this.colAction.UseColumnTextForButtonValue = true;
            // 
            // UcDoctorAvailability
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlStatus);
            this.Name = "UcDoctorAvailability";
            this.Size = new System.Drawing.Size(800, 650);
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlocked)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
