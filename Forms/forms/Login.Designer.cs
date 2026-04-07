using System.Drawing;
namespace ClinicAppointmentSystem
{
    partial class Login
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelLogin;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.LinkLabel linkRegister;
        private System.Windows.Forms.Panel panelBlue;
        private System.Windows.Forms.CheckBox chkRememberMe;
        private System.Windows.Forms.CheckBox chkAutoLogin;
        private System.Windows.Forms.CheckBox chkShowPassword;

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
            this.panelLogin = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelBlue = new System.Windows.Forms.Panel();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.linkRegister = new System.Windows.Forms.LinkLabel();
            this.chkRememberMe = new System.Windows.Forms.CheckBox();
            this.chkAutoLogin = new System.Windows.Forms.CheckBox();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();

            this.panelLogin.SuspendLayout();
            this.panelBlue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.SuspendLayout();

            // Form Properties
            this.Text = "Clinic System | Secure Login";
            this.ClientSize = new System.Drawing.Size(900, 850);
            this.MinimumSize = new System.Drawing.Size(600, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.BackColor = System.Drawing.Color.FromArgb(240, 248, 255);
            this.Resize += (s, e) => {
                panelLogin.Left = (this.Width - panelLogin.Width) / 2;
                panelLogin.Top = (this.Height - panelLogin.Height) / 2;
            };

            // panelLogin
            this.panelLogin.BackColor = System.Drawing.Color.White;
            this.panelLogin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panelLogin.Controls.Add(this.btnClose);
            this.panelLogin.Controls.Add(this.panelBlue);
            this.panelLogin.Controls.Add(this.lblUser);
            this.panelLogin.Controls.Add(this.txtUsername);
            this.panelLogin.Controls.Add(this.lblPass);
            this.panelLogin.Controls.Add(this.txtPassword);
            this.panelLogin.Controls.Add(this.chkAutoLogin);
            this.panelLogin.Controls.Add(this.chkRememberMe);
            this.panelLogin.Controls.Add(this.chkShowPassword);
            this.panelLogin.Controls.Add(this.btnLogin);
            this.panelLogin.Controls.Add(this.linkRegister);
            this.panelLogin.Location = new System.Drawing.Point(225, 100);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(450, 650);
            this.panelLogin.TabIndex = 0;
            // Add a subtle drop-shadow effect if possible (using a border for simplicity)
            this.panelLogin.Padding = new System.Windows.Forms.Padding(0);

            // btnClose
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.btnClose.Location = new System.Drawing.Point(360, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // panelBlue
            this.panelBlue.BackColor = System.Drawing.Color.FromArgb(0, 174, 219);
            this.panelBlue.Controls.Add(this.pictureBoxIcon);
            this.panelBlue.Controls.Add(this.lblTitle);
            this.panelBlue.Controls.Add(this.lblSubtitle);
            this.panelBlue.Location = new System.Drawing.Point(0, 0);
            this.panelBlue.Name = "panelBlue";
            this.panelBlue.Size = new System.Drawing.Size(450, 160);
            this.panelBlue.TabIndex = 1;

            // pictureBoxIcon
            this.pictureBoxIcon.Size = new System.Drawing.Size(60, 60);
            this.pictureBoxIcon.Location = new System.Drawing.Point(170, 15);
            this.pictureBoxIcon.BackColor = System.Drawing.Color.White;
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.TabIndex = 0;
            this.pictureBoxIcon.TabStop = false;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(120, 80);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 30);
            this.lblTitle.Text = "Welcome Back!";

            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(220, 240, 255);
            this.lblSubtitle.Location = new System.Drawing.Point(140, 110);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(120, 19);
            this.lblSubtitle.Text = "Please login to continue";

            // lblUser
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblUser.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.lblUser.Location = new System.Drawing.Point(50, 200);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(78, 20);
            this.lblUser.Text = "Username";

            // txtUsername
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtUsername.Location = new System.Drawing.Point(50, 230);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(350, 29);
            this.txtUsername.TabIndex = 1;

            // lblPass
            this.lblPass.AutoSize = true;
            this.lblPass.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPass.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.lblPass.Location = new System.Drawing.Point(50, 280);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(77, 20);
            this.lblPass.Text = "Password";

            // txtPassword
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPassword.Location = new System.Drawing.Point(50, 310);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(350, 29);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;

            // chkRememberMe
            this.chkRememberMe.AutoSize = true;
            this.chkRememberMe.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkRememberMe.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.chkRememberMe.Location = new System.Drawing.Point(50, 355);
            this.chkRememberMe.Name = "chkRememberMe";
            this.chkRememberMe.Size = new System.Drawing.Size(118, 23);
            this.chkRememberMe.TabIndex = 3;
            this.chkRememberMe.Text = "Remember Me";
            this.chkRememberMe.UseVisualStyleBackColor = true;
            this.chkRememberMe.CheckedChanged += new System.EventHandler(this.chkRememberMe_CheckedChanged);

            // chkAutoLogin
            this.chkAutoLogin.AutoSize = true;
            this.chkAutoLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkAutoLogin.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.chkAutoLogin.Location = new System.Drawing.Point(200, 355);
            this.chkAutoLogin.Name = "chkAutoLogin";
            this.chkAutoLogin.Size = new System.Drawing.Size(95, 23);
            this.chkAutoLogin.TabIndex = 4;
            this.chkAutoLogin.Text = "Auto-login";
            this.chkAutoLogin.UseVisualStyleBackColor = true;
            this.chkAutoLogin.Visible = false;

            // chkShowPassword
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkShowPassword.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.chkShowPassword.Location = new System.Drawing.Point(292, 282);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(108, 19);
            this.chkShowPassword.TabIndex = 8;
            this.chkShowPassword.Text = "Show Password";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);

            // btnLogin
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(0, 174, 219);
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(50, 420);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(350, 55);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "LOGIN";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // linkRegister
            this.linkRegister.AutoSize = true;
            this.linkRegister.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.linkRegister.LinkColor = System.Drawing.Color.FromArgb(0, 174, 219);
            this.linkRegister.Location = new System.Drawing.Point(145, 520);
            this.linkRegister.Name = "linkRegister";
            this.linkRegister.Size = new System.Drawing.Size(160, 19);
            this.linkRegister.Text = "Create a New Account";
            this.linkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRegister_LinkClicked);


            // Decorative Line
            System.Windows.Forms.Panel line = new System.Windows.Forms.Panel();
            line.BackColor = System.Drawing.Color.FromArgb(200, 200, 200);
            line.Location = new System.Drawing.Point(50, 360);
            line.Size = new System.Drawing.Size(300, 1);
            this.panelLogin.Controls.Add(line);

            this.Controls.Add(this.panelLogin);

            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            this.panelBlue.ResumeLayout(false);
            this.panelBlue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
