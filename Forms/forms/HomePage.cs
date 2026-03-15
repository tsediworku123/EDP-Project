using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ClinicAppointmentSystem
{
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
            SetupLogo();
            SetupButtonHoverEffects();
            this.Resize += HomePage_Resize;
        }

        private void HomePage_Resize(object sender, EventArgs e)
        {
            CenterControls();
        }

        private void CenterControls()
        {
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            // Center the panel containing all controls
            int panelWidth = 600;
            int panelHeight = 400;
            int panelX = centerX - panelWidth / 2;
            int panelY = centerY - panelHeight / 2;

            // Position all controls relative to the center
            pictureBox1.Location = new Point(panelX + (panelWidth - pictureBox1.Width) / 2, panelY + 30);
            lblWelcome.Location = new Point(panelX + (panelWidth - lblWelcome.Width) / 2, panelY + 120);
            lblTitle.Location = new Point(panelX + (panelWidth - lblTitle.Width) / 2, panelY + 150);
            lblSubtitle.Location = new Point(panelX + (panelWidth - lblSubtitle.Width) / 2, panelY + 200);

            int buttonSpacing = 20;
            int buttonWidth = 150;
            int totalButtonsWidth = buttonWidth * 2 + buttonSpacing;
            int startX = panelX + (panelWidth - totalButtonsWidth) / 2;

            btnLogin.Location = new Point(startX, panelY + 260);
            btnRegister.Location = new Point(startX + buttonWidth + buttonSpacing, panelY + 260);
        }

        private void SetupLogo()
        {
            this.pictureBox1.Paint += (s, e) => {
                using (Pen pen = new Pen(Color.White, 8))
                {
                    // Horizontal line
                    e.Graphics.DrawLine(pen, 20, 40, 60, 40);
                    // Vertical line
                    e.Graphics.DrawLine(pen, 40, 20, 40, 60);
                }
            };
            this.pictureBox1.Invalidate();
        }

        private void SetupButtonHoverEffects()
        {
            btnLogin.MouseEnter += (s, e) => {
                btnLogin.BackColor = Color.FromArgb(0, 191, 255);
                btnLogin.ForeColor = Color.White;
            };
            btnLogin.MouseLeave += (s, e) => {
                btnLogin.BackColor = Color.White;
                btnLogin.ForeColor = Color.FromArgb(0, 191, 255);
            };

            btnRegister.MouseEnter += (s, e) => {
                btnRegister.BackColor = Color.FromArgb(0, 150, 200);
            };
            btnRegister.MouseLeave += (s, e) => {
                btnRegister.BackColor = Color.FromArgb(0, 191, 255);
            };
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Pass the MainContainer reference to Login form
            Login loginForm = new Login(Program.MainForm);
            loginForm.ShowDialog();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Register registerForm = new Register();
            registerForm.ShowDialog();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Create deep blue gradient background
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(0, 105, 148),
                Color.FromArgb(0, 191, 255),
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}