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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // use the designer ListView named listView1 (initialized in InitializeComponent)
            listView1.Items.Clear();

            var results = Mainform.patients
                .Where(p => p.Name.ToLower().Contains(txtSearch.Text.ToLower()))
                .ToList();

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
