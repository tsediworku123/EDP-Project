using System.Windows.Forms;
using System.Drawing;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcStaffManagement
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCreateUser = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.pnlTopTools = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this.staffGrid = new System.Windows.Forms.DataGridView();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnToggleU = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.cmbRoleFilter = new System.Windows.Forms.ComboBox();
            this.cmbStatusFilter = new System.Windows.Forms.ComboBox();
            this.cmbSortMethod = new System.Windows.Forms.ComboBox();
            this.pGridWrap = new System.Windows.Forms.Panel();
            this.pnlTopTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.staffGrid)).BeginInit();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.Text = "SYSTEM USERS & ACCESS CONTROL";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Size = new System.Drawing.Size(500, 35);

            // btnCreateUser
            this.btnCreateUser.Text = " ADD NEW USER";
            this.btnCreateUser.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnCreateUser.ForeColor = System.Drawing.Color.White;
            this.btnCreateUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateUser.FlatAppearance.BorderSize = 0;
            this.btnCreateUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCreateUser.Location = new System.Drawing.Point(800, 20);
            this.btnCreateUser.Size = new System.Drawing.Size(140, 35);
            this.btnCreateUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnCreateUser.Click += new System.EventHandler(this.btnCreateUser_Click);

            // btnExport
            this.btnExport.Text = " EXPORT";
            this.btnExport.BackColor = System.Drawing.Color.White;
            this.btnExport.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(950, 20);
            this.btnExport.Size = new System.Drawing.Size(100, 35);
            this.btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // pnlTopTools
            this.pnlTopTools.BackColor = System.Drawing.Color.White;
            this.pnlTopTools.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTopTools.Controls.Add(this.cmbSortMethod);
            this.pnlTopTools.Controls.Add(this.cmbStatusFilter);
            this.pnlTopTools.Controls.Add(this.cmbRoleFilter);
            this.pnlTopTools.Controls.Add(this.txtSearch);
            this.pnlTopTools.Controls.Add(this.lblSearchIcon);
            this.pnlTopTools.Location = new System.Drawing.Point(20, 75);
            this.pnlTopTools.Size = new System.Drawing.Size(1030, 50);
            this.pnlTopTools.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // txtSearch
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSearch.Location = new System.Drawing.Point(45, 15);
            this.txtSearch.Size = new System.Drawing.Size(400, 20);
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            // cmbRoleFilter
            this.cmbRoleFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoleFilter.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbRoleFilter.FormattingEnabled = true;
            this.cmbRoleFilter.Items.AddRange(new object[] {
            "All Roles",
            "Admin",
            "Receptionist",
            "Doctor"});
            this.cmbRoleFilter.Location = new System.Drawing.Point(500, 11);
            this.cmbRoleFilter.Name = "cmbRoleFilter";
            this.cmbRoleFilter.Size = new System.Drawing.Size(200, 25);
            this.cmbRoleFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbRoleFilter.SelectedIndex = 0;
            this.cmbRoleFilter.SelectedIndexChanged += new System.EventHandler(this.cmbRoleFilter_SelectedIndexChanged);

            // cmbStatusFilter
            this.cmbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbStatusFilter.FormattingEnabled = true;
            this.cmbStatusFilter.Items.AddRange(new object[] {
            "All Status",
            "Active",
            "Locked"});
            this.cmbStatusFilter.Location = new System.Drawing.Point(710, 11);
            this.cmbStatusFilter.Name = "cmbStatusFilter";
            this.cmbStatusFilter.Size = new System.Drawing.Size(130, 25);
            this.cmbStatusFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStatusFilter.SelectedIndex = 0;
            this.cmbStatusFilter.SelectedIndexChanged += new System.EventHandler(this.cmbStatusFilter_SelectedIndexChanged);

            // cmbSortMethod
            this.cmbSortMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSortMethod.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbSortMethod.FormattingEnabled = true;
            this.cmbSortMethod.Items.AddRange(new object[] {
            "Sort: Username A-Z",
            "Sort: Username Z-A",
            "Sort: Role A-Z",
            "Sort: Role Z-A",
            "Sort: Status (Active first)",
            "Sort: Status (Locked first)"});
            this.cmbSortMethod.Location = new System.Drawing.Point(850, 11);
            this.cmbSortMethod.Name = "cmbSortMethod";
            this.cmbSortMethod.Size = new System.Drawing.Size(170, 25);
            this.cmbSortMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSortMethod.SelectedIndex = 0;
            this.cmbSortMethod.SelectedIndexChanged += new System.EventHandler(this.cmbSortMethod_SelectedIndexChanged);

            // lblSearchIcon
            this.lblSearchIcon.Text = "🔍";
            this.lblSearchIcon.Location = new System.Drawing.Point(15, 15);
            this.lblSearchIcon.Size = new System.Drawing.Size(25, 25);

            // staffGrid
            this.staffGrid.BackgroundColor = System.Drawing.Color.White;
            this.staffGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.staffGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.staffGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.staffGrid.ColumnHeadersHeight = 40;
            this.staffGrid.ReadOnly = true;
            this.staffGrid.RowHeadersVisible = false;
            this.staffGrid.AllowUserToAddRows = false;
            this.staffGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.staffGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.staffGrid_CellFormatting);

            // pnlActions - buttons added rightmost-first so they stack left from right edge
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Height = 60;
            this.pnlActions.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.pnlActions.Controls.Add(this.btnDelete);
            this.pnlActions.Controls.Add(this.btnToggleU);
            this.pnlActions.Controls.Add(this.lblInfo);

            // btnDelete (rightmost)
            this.btnDelete.Text = "  🗑  DELETE";
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.Width = 130;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);



            // btnToggleU
            this.btnToggleU.Text = "  ⚡  DEACTIVATE";
            this.btnToggleU.BackColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.btnToggleU.ForeColor = System.Drawing.Color.White;
            this.btnToggleU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleU.FlatAppearance.BorderSize = 0;
            this.btnToggleU.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnToggleU.Width = 145;
            this.btnToggleU.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnToggleU.Click += new System.EventHandler(this.btnToggleU_Click);

            // lblInfo
            this.lblInfo.Text = "Select a user to manage access.";
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInfo.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);


            // pGridWrap
            this.pGridWrap.Location = new System.Drawing.Point(20, 140);
            this.pGridWrap.Size = new System.Drawing.Size(1030, 480);
            this.pGridWrap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pGridWrap.Controls.Add(this.staffGrid);

            // UcStaffManagement
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.pGridWrap);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlTopTools);
            this.Controls.Add(this.btnCreateUser);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lblTitle);
            this.Size = new System.Drawing.Size(1070, 700);
            this.pnlTopTools.ResumeLayout(false);
            this.pnlTopTools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.staffGrid)).EndInit();
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCreateUser;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel pnlTopTools;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbRoleFilter;
        private System.Windows.Forms.ComboBox cmbStatusFilter;
        private System.Windows.Forms.ComboBox cmbSortMethod;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.DataGridView staffGrid;
        private System.Windows.Forms.Panel pGridWrap;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnToggleU;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblInfo;
    }
}
