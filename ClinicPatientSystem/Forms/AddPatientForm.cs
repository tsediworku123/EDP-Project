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


namespace ClinicPatientSystem
{
    public partial class AddPatientForm : Form
    {
        public AddPatientForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            int age;

            if (!int.TryParse(textBox2.Text, out age))
            {
                MessageBox.Show("Age must be a number");
                return;
            }

            Patient p = new Patient();

            p.Id = Mainform.patients.Count + 1;
            p.Name = textBox1.Text;
            p.Age = age;
            p.Gender = comboBox1.Text;
            p.Phone = textBox3.Text;

            Mainform.patients.Add(p);

            MessageBox.Show("Patient added successfully");

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }
    }
}