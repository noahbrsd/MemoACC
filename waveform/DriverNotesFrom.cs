using System;
using System.Windows.Forms;

namespace MemoPilotes
{
    public partial class DriverNotesForm : Form
    {
        public DriverNotesForm(string[] driverNotes)
        {
            InitializeComponent();

            // Populate the ListBox with driver notes
            listBoxDriverNotes.Items.AddRange(driverNotes);
        }

        private void InitializeComponent()
        {
            this.listBoxDriverNotes = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            
            // 
            // listBoxDriverNotes
            // 
            this.listBoxDriverNotes.FormattingEnabled = true;
            this.listBoxDriverNotes.Location = new System.Drawing.Point(12, 12);
            this.listBoxDriverNotes.Name = "listBoxDriverNotes";
            this.listBoxDriverNotes.Size = new System.Drawing.Size(360, 290);
            this.listBoxDriverNotes.TabIndex = 0;
            
            // 
            // DriverNotesForm
            // 
            this.ClientSize = new System.Drawing.Size(384, 311);
            this.Controls.Add(this.listBoxDriverNotes);
            this.Name = "DriverNotesForm";
            this.Text = "Driver Notes";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ListBox listBoxDriverNotes;
    }
}
