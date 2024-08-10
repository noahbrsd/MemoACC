namespace MemoPilotes
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxPilotes;
        private System.Windows.Forms.TextBox textBoxNote;
        private System.Windows.Forms.Button buttonEnregistrer;
        private System.Windows.Forms.Label labelNoteExistante;
        private System.Windows.Forms.Button buttonSpecialEvent;

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
            this.listBoxPilotes = new System.Windows.Forms.ListBox();
            this.textBoxNote = new System.Windows.Forms.TextBox();
            this.buttonEnregistrer = new System.Windows.Forms.Button();
            this.labelNoteExistante = new System.Windows.Forms.Label();
            this.buttonSpecialEvent = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxPilotes
            // 
            this.listBoxPilotes.FormattingEnabled = true;
            this.listBoxPilotes.Location = new System.Drawing.Point(12, 12);
            this.listBoxPilotes.Name = "listBoxPilotes";
            this.listBoxPilotes.Size = new System.Drawing.Size(200, 368);
            this.listBoxPilotes.TabIndex = 0;
            this.listBoxPilotes.SelectedIndexChanged += new System.EventHandler(this.listBoxPilotes_SelectedIndexChanged);
            // 
            // textBoxNote
            // 
            this.textBoxNote.Location = new System.Drawing.Point(218, 12);
            this.textBoxNote.Multiline = true;
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.Size = new System.Drawing.Size(400, 100);
            this.textBoxNote.TabIndex = 1;
            // 
            // buttonEnregistrer
            // 
            this.buttonEnregistrer.Location = new System.Drawing.Point(218, 118);
            this.buttonEnregistrer.Name = "buttonEnregistrer";
            this.buttonEnregistrer.Size = new System.Drawing.Size(100, 23);
            this.buttonEnregistrer.TabIndex = 2;
            this.buttonEnregistrer.Text = "Enregistrer";
            this.buttonEnregistrer.UseVisualStyleBackColor = true;
            this.buttonEnregistrer.Click += new System.EventHandler(this.buttonEnregistrer_Click);
            // 
            // labelNoteExistante
            // 
            this.labelNoteExistante.AutoSize = true;
            this.labelNoteExistante.Location = new System.Drawing.Point(218, 144);
            this.labelNoteExistante.Name = "labelNoteExistante";
            this.labelNoteExistante.Size = new System.Drawing.Size(109, 13);
            this.labelNoteExistante.TabIndex = 3;
            this.labelNoteExistante.Text = "Aucune note enregistrée.";
            // 
            // buttonSpecialEvent
            // 
            this.buttonSpecialEvent.Location = new System.Drawing.Point(518, 118);
            this.buttonSpecialEvent.Name = "buttonSpecialEvent";
            this.buttonSpecialEvent.Size = new System.Drawing.Size(100, 23);
            this.buttonSpecialEvent.TabIndex = 4;
            this.buttonSpecialEvent.Text = "Événement spécial";
            this.buttonSpecialEvent.UseVisualStyleBackColor = true;
            this.buttonSpecialEvent.Click += new System.EventHandler(this.buttonSpecialEvent_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 393);
            this.Controls.Add(this.buttonSpecialEvent);
            this.Controls.Add(this.labelNoteExistante);
            this.Controls.Add(this.buttonEnregistrer);
            this.Controls.Add(this.textBoxNote);
            this.Controls.Add(this.listBoxPilotes);
            this.Name = "MainForm";
            this.Text = "Memo Pilotes";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
