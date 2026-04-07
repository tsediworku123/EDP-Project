using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClinicAppointmentSystem.Models;

namespace ClinicAppointmentSystem.Controls
{
    public class UcSecurityManager : UserControl
    {
        private TextBox txtOld;
        private TextBox txtNew;
        private TextBox txtConf;
        private Button btnSave;

        public UcSecurityManager()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Dock = DockStyle.Fill;

            Panel p = new Panel {
                Size = new Size(500, 450),
                Location = new Point(50, 50),
                BackColor = Color.White,
                Padding = new Padding(30)
            };
            this.Controls.Add(p);

            Label title = new Label {
                Text = "ACCOUNT SECURITY & PRIVACY",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 30)
            };
            p.Controls.Add(title);

            Label sub = new Label {
                Text = "Manage your login credentials. We recommend changing your password regularly.",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(30, 65),
                Size = new Size(440, 40)
            };
            p.Controls.Add(sub);

            int y = 120;
            txtOld = AddField(p, "CURRENT PASSWORD", ref y); txtOld.PasswordChar = '*';
            txtNew = AddField(p, "NEW PASSWORD", ref y); txtNew.PasswordChar = '*';
            txtConf = AddField(p, "CONFIRM NEW PASSWORD", ref y); txtConf.PasswordChar = '*';

            btnSave = new Button {
                Text = "UPDATE PASSWORD",
                Location = new Point(30, y + 10),
                Size = new Size(440, 48),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => {
                var user = DataManager.CurrentUser;
                if (user == null) return;

                if (!Utils.PasswordHasher.VerifyPassword(txtOld.Text, user.Password)) {
                    MessageBox.Show("Current password is incorrect.", "Security Error");
                    return;
                }
                if (txtNew.Text.Length < 6) {
                    MessageBox.Show("New password must be at least 6 characters.", "Validation");
                    return;
                }
                if (txtNew.Text != txtConf.Text) {
                    MessageBox.Show("New passwords do not match.", "Validation");
                    return;
                }

                user.Password = Utils.PasswordHasher.HashPassword(txtNew.Text);
                DataManager.SaveAllData();
                DataManager.LogAudit(user.Username, "Updated security credentials");
                
                MessageBox.Show("Your password has been successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOld.Clear(); txtNew.Clear(); txtConf.Clear();
            };
            p.Controls.Add(btnSave);
        }

        private TextBox AddField(Panel p, string label, ref int y)
        {
            Label lbl = new Label { Text = label, Location = new Point(30, y), AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            TextBox txt = new TextBox { Location = new Point(30, y + 20), Width = 440, Font = new Font("Segoe UI", 11) };
            p.Controls.AddRange(new Control[] { lbl, txt });
            y += 65;
            return txt;
        }
    }
}
