using System;
using System.Windows.Forms;

namespace ClinicAppointmentSystem
{
    internal static class Program
    {
        public static MainContainer MainForm { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new MainContainer();
            Application.Run(MainForm);
        }
    }
}