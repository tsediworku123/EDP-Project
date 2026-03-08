using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicPatientSystem
{
    public partial class SearchPatientForm : Form
    {
        public SearchPatientForm()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listViewResults.Items.Clear();

            // Check if search text is empty
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                MessageBox.Show("Please enter search text.");
                return;
            }

            // Search patients (case-insensitive)
            var results = Mainform.patients
                .Where(p => p.Name.ToLower().Contains(txtSearch.Text.ToLower()))
                .ToList();

            // Display results
            foreach (var p in results)
            {
                ListViewItem item = new ListViewItem(p.Id.ToString());
                item.SubItems.Add(p.Name);
                item.SubItems.Add(p.Age.ToString());
                item.SubItems.Add(p.Gender);
                item.SubItems.Add(p.Phone);

                listViewResults.Items.Add(item);
            }

            // Show count
            if (results.Count == 0)
            {
                MessageBox.Show("No patients found.");
            }
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            var results = Mainform.patients
                .Where(p => p.Name.ToLower().Contains(textBoxSearch.Text.ToLower()))
                .ToList();

            if (results.Count == 0)
            {
                MessageBox.Show("No patient found");
                return;
            }

            foreach (var p in results)
            {
                ListViewItem item = new ListViewItem(p.Id.ToString());

                item.SubItems.Add(p.Name);
                item.SubItems.Add(p.Age.ToString());
                item.SubItems.Add(p.Gender);
                item.SubItems.Add(p.Phone);

                listView1.Items.Add(item);
            }
        }
    }
}
