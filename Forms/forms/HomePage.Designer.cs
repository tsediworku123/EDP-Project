namespace ClinicAppointmentSystem
{
    partial class HomePage
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.PictureBox pictureBox1;

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
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();

            this.SuspendLayout();

            // Form Properties
            this.Text = "Home Page";
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(800, 600);

            // pictureBox1
            this.pictureBox1.Size = new System.Drawing.Size(80, 80);
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;

            // lblWelcome
            this.lblWelcome.Text = "WELCOME TO";
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.BackColor = System.Drawing.Color.Transparent;
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.TabIndex = 1;

            // lblTitle
            this.lblTitle.Text = "Clinic Management";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.TabIndex = 2;

            // lblSubtitle
            this.lblSubtitle.Text = "Your Health, Our Commitment";
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(200, 230, 255);
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.TabIndex = 3;

            // btnLogin
            this.btnLogin.Text = "Login";
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(0, 191, 255);
            this.btnLogin.BackColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 2;
            this.btnLogin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 191, 255);
            this.btnLogin.Size = new System.Drawing.Size(150, 50);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.TabIndex = 4;
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // btnRegister
            this.btnRegister.Text = "Register";
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(0, 191, 255);
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.Size = new System.Drawing.Size(150, 50);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.TabIndex = 5;
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);

            // Add controls
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnRegister);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
