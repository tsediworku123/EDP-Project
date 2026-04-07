using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ClinicAppointmentSystem
{
    public partial class MainContainer : Form
    {
        private bool isLoggedIn = false;
        private string userRole = "";
        private string userName = "";
        public static bool isTransitioning = false; // Static guard to prevent recursive loops

        public MainContainer()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;
            this.menuStrip.Visible = false;

            SessionManager.Instance.OnSessionTimeout += HandleSessionTimeout;
            ShowHomePage();
        }

        private void ShowHomePage()
        {
            CloseAllMdiChildren();
            
            HomePage homePage = new HomePage();
            homePage.MdiParent = this;
            homePage.WindowState = FormWindowState.Maximized;
            homePage.Show();
            UpdateMenuForRole();
        }

        private void CloseAllMdiChildren()
        {
            foreach (Form f in this.MdiChildren.ToArray())
            {
                f.Close();
                f.Dispose();
            }
        }

        public void OpenForm(Form form)
        {
            if (!isLoggedIn && !(form is HomePage) && !(form is Login) && !(form is RegisterPatientForm))
            {
                MessageBox.Show("Please login first to access this feature.", "Security Protection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CloseAllMdiChildren();

            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        public void UpdateMenuForRole()
        {
            menuStrip.Visible = false;
            menuStrip.Items.Clear();

            if (!isLoggedIn)
            {
                ToolStripMenuItem homeItem = new ToolStripMenuItem("Home");
                homeItem.Click += (s, e) => ShowHomePage();
                menuStrip.Items.Add(homeItem);
                return;
            }

            // File Menu
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += (s, e) => Application.Exit();
            fileMenu.DropDownItems.Add(exitItem);
            menuStrip.Items.Add(fileMenu);

            // Logout Menu
            ToolStripMenuItem logoutItem = new ToolStripMenuItem("Logout");
            logoutItem.Click += LogoutMenuItem_Click;
            menuStrip.Items.Add(logoutItem);

            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            lblUser.Text = isLoggedIn
                ? $"Active Session: {userName} ({userRole})"
                : "Secure Access: Not Logged In";
            lblStatus.Text = "Status: Healthy";
        }

        // ====================== ROBUST LOGIN SUCCESS HANDOFF ======================
        public void LoginSuccess(string role, string name)
        {
            // PREVENT RECURSIVE STACK OVERFLOW
            if (isTransitioning) return;

            isLoggedIn = true;
            userRole = role;
            userName = name;

            UpdateMenuForRole();
            UpdateStatusBar();
            menuStrip.Visible = false;

            try
            {
                switch (role)
                {
                    case "Admin":
                        OpenForm(new AdminDashboard());
                        break;

                    case "Doctor":
                        int docId = DataManager.CurrentUser?.DoctorId ?? 0;
                        OpenForm(new DoctorDashboard(docId));
                        break;

                    case "Receptionist":
                        // Shared dashboard for now, can be updated later
                        OpenForm(new AdminDashboard());
                        break;

                    case "Patient":
                        // TOTAL DECOUPLING: Release WinForms thread and clean MDI before WPF launch
                        isTransitioning = true;
                        DataManager.EnsureLoaded();
                        SessionManager.Instance.AutoLogin = false;

                        // Ensure all legacy forms are closed to prevent background event noise
                        CloseAllMdiChildren();

                        // Use a DispatcherTimer for safer thread-affinity (replaces async lambda)
                        var timer = new System.Windows.Threading.DispatcherTimer(
                            System.Windows.Threading.DispatcherPriority.Background,
                            System.Windows.Application.Current.Dispatcher);
                        
                        timer.Interval = TimeSpan.FromMilliseconds(100);
                        timer.Tick += (s, e) =>
                        {
                            timer.Stop();
                            try
                            {
                                if (this.IsDisposed) return;
                                var patientShell = new Views.PatientShellView();
                                patientShell.Closed += (sender, args) =>
                                {
                                    isTransitioning = false;
                                    System.Windows.Forms.Application.Exit();
                                };
                                this.Hide();
                                patientShell.Show();
                            }
                            catch (Exception ex)
                            {
                                isTransitioning = false;
                                MessageBox.Show("Hybrid Bridge Error:\n" + ex.Message, "Launch Failure", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                if (!this.IsDisposed) this.Show();
                            }
                        };
                        timer.Start();

                        return; // EXIT UI THREAD IMMEDIATELY

                    default:
                        MessageBox.Show("Access Denied: Unrecognized Role Dashboard.", "Security Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                isTransitioning = false;
                MessageBox.Show("System Launch Failure:\n" + ex.Message,
                    "Process Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Only unlock if we aren't in the middle of a delicate WPF transition
                if (role != "Patient") isTransitioning = false;
            }
        }

        private void LogoutMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to end your current session?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ForceLogout(false);
            }
        }

        private void HandleSessionTimeout(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => HandleSessionTimeout(sender, e)));
                return;
            }

            if (isLoggedIn)
            {
                MessageBox.Show("Your session has timed out for security reasons.", "Security Timeout", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ForceLogout(true);
            }
        }

        public void ForceLogout(bool toLogin = false)
        {
            isLoggedIn = false;
            userRole = "";
            userName = "";
            DataManager.CurrentUser = null;
            isTransitioning = false; // Reset security flags

            UpdateMenuForRole();
            UpdateStatusBar();
            SessionManager.Instance.StopSessionTracking();

            CloseAllMdiChildren();

            if (toLogin)
            {
                Login loginForm = new Login(this);
                loginForm.MdiParent = this;
                loginForm.WindowState = FormWindowState.Maximized;
                loginForm.Show();
            }
            else
            {
                ShowHomePage();
            }
        }
    }
}