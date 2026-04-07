using System.Drawing;
using System.Windows.Forms;

namespace ClinicAppointmentSystem.Controls
{
    partial class UcPatientProfile
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UcPatientProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Name = "UcPatientProfile";
            this.Size = new System.Drawing.Size(1100, 750);
            this.Load += new System.EventHandler(this.UcPatientProfile_Load);
            this.ResumeLayout(false);
        }
    }
}
