using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcPatientHome
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UcPatientHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Name = "UcPatientHome";
            this.Size = new System.Drawing.Size(1100, 750);
            this.ResumeLayout(false);
        }

    }
}
