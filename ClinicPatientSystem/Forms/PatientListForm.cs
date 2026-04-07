using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicAppointmentSystem
{
    public partial class PatientListForm : Form
    {
        public PatientListForm()
        {
            InitializeComponent();
        }

        private void PatientListForm_Load(object sender, EventArgs e)
        {
            listViewPatients.Items.Clear();

            foreach (var p in Mainform.patients)
            {
                ListViewItem item = new ListViewItem(p.Id.ToString());

                item.SubItems.Add(p.Name);
                item.SubItems.Add(p.Age.ToString());
                item.SubItems.Add(p.Gender);
                item.SubItems.Add(p.Phone);

                listViewPatients.Items.Add(item);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewPatients.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select patient first");
                return;
            }

            int index = listViewPatients.SelectedItems[0].Index;

            Mainform.patients.RemoveAt(index);

            MessageBox.Show("Patient deleted");

            PatientListForm_Load(null, null);
        }
    }
}
