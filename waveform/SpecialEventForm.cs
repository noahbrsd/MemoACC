using System;
using System.Linq;
using System.Windows.Forms;

namespace MemoPilotes
{
    public partial class SpecialEventForm : Form
    {
        public SpecialEventForm()
        {
            InitializeComponent();
            ChargerPilotes();
        }

        private void ChargerPilotes()
        {
            var pilotes = ObtenirNomsDesPilotes();
            foreach (var pilote in pilotes.Take(100))  // Limitation à 100 pilotes
            {
                listBoxPilotes.Items.Add(pilote);
            }
        }

        private void buttonEnregistrer_Click(object sender, EventArgs e)
        {
            if (listBoxPilotes.SelectedItem != null && !string.IsNullOrEmpty(textBoxNote.Text))
            {
                string nomPilote = listBoxPilotes.SelectedItem.ToString();
                string note = textBoxNote.Text;
                DatabaseHelper.AjouterOuMettreAJourPilote(nomPilote, note);
                MessageBox.Show("Note enregistrée !");
                textBoxNote.Clear();
                ChargerPilotes();  // Recharger les pilotes pour mettre à jour les notes
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un pilote et entrer une note.");
            }
        }

        private void listBoxPilotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPilotes.SelectedItem != null)
            {
                string nomPilote = listBoxPilotes.SelectedItem.ToString();
                string note = DatabaseHelper.ObtenirNotePilote(nomPilote);
                labelNoteExistante.Text = note ?? "Aucune note enregistrée.";
            }
        }

        private string[] ObtenirNomsDesPilotes()
        {
            // Implémentez la méthode pour obtenir les noms des pilotes à partir de vos données
            return new string[] { "Pilote 1", "Pilote 2", "Pilote 3", /* jusqu'à 100 pilotes */ };
        }
    }
}
