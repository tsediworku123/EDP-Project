using ClinicPatientSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System;
using System.Windows.Forms;


namespace ClinicAppointmentSystem
{
    public partial class AddPatientForm : Form
    {
        public AddPatientForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please provide at least Name and Phone Number.");
                return;
            }

            Patient p = new Patient
            {
                FullName = textBox1.Text.Trim(),
                Phone = textBox3.Text.Trim(),
                Gender = comboBox1.Text,
                DateOfBirth = DateTime.Today.AddYears(-20), // Default age 20 if only age provided
                Username = textBox3.Text.Trim(), // Default username is phone
                Password = "patient123" // Default password for admin-created patients
            };

            // If textBox2 contains a number, use it to estimate DOB
            if (int.TryParse(textBox2.Text, out int age))
            {
                p.DateOfBirth = DateTime.Today.AddYears(-age);
            }

            DataManager.RegisterPatient(p);
            DataManager.SavePatients(); // Ensure persistent save

            MessageBox.Show($"Patient added successfully!\nDefault Password: {p.Password}");

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }
    }
}
