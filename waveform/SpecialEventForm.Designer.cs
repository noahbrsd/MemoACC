namespace MemoPilotes
{
    partial class SpeMainForm 
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxPilotes;
        private System.Windows.Forms.TextBox textBoxNote;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelNoteExistante;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonCopyNote;

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
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelNoteExistante = new System.Windows.Forms.Label();
            
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonCopyNote = new System.Windows.Forms.Button();
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
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(218, 118);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Enregistrer";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);

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
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(518, 118);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(100, 23);
            this.buttonUpdate.TabIndex = 5;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);

            // 
            // buttonCopyNote
            // 
            this.buttonCopyNote.Location = new System.Drawing.Point(518, 160);
            this.buttonCopyNote.Name = "buttonCopyNote";
            this.buttonCopyNote.Size = new System.Drawing.Size(100, 23);
            this.buttonCopyNote.TabIndex = 6;
            this.buttonCopyNote.Text = "Copy Note";
            this.buttonCopyNote.UseVisualStyleBackColor = true;
            this.buttonCopyNote.Click += new System.EventHandler(this.buttonCopyNote_Click);

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 393);
            this.Controls.Add(this.labelNoteExistante);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxNote);
            this.Controls.Add(this.listBoxPilotes);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.buttonCopyNote);
            this.Name = "MainForm";
            this.Text = "Memo Pilotes";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
