using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using RaceElement.Broadcast;
using System.Threading.Tasks;
using System.Collections.Generic; 
using RaceElement.Broadcast.Structs;





namespace MemoPilotes

{
    
    public partial class SpeMainForm : Form
    {
        private ACCUdpRemoteClient _udpClient;  // Déclaration de _udpClient

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        public SpeMainForm()
{
    try
    {
        AllocConsole();
        InitializeComponent();
        
        Console.WriteLine("MainForm: Initialisation de l'application.");
        DatabaseHelper.InitializeDatabase();
        // Initialisation de l'UDP client avec les paramètres appropriés
        string ip = "127.0.0.1"; // Remplacez par l'adresse IP correcte
        int port = 9000; // Remplacez par le port correct
        // string displayName = "My Display Name";
        // string connectionPassword = "password";
        // string commandPassword = "cmdPassword";
        // int msRealtimeUpdateInterval = 1000;
        var config = BroadcastConfig.GetConfiguration();

        _udpClient = new ACCUdpRemoteClient(ip, config.UpdListenerPort, "MyDisplayName", config.ConnectionPassword, config.CommandPassword, port);
        Console.WriteLine("MainForm: UDP Client initialisé.");
        // _udpClient.MessageHandler.OnEntryListComplete += OnEntryListComplete; // Abonnement à l'événement
        // // Récupérez les noms des pilotes pour les afficher dans l'interface
        // string[] nomsDesPilotes = ObtenirNomsDesPilotes();
        // Console.WriteLine("MainForm: Noms des pilotes récupérés.");
        // listBoxPilotes.Items.AddRange(nomsDesPilotes);
        // Console.WriteLine("MainForm: Noms des pilotes ajoutés au ListBox.");
        // foreach (var nom in nomsDesPilotes)
        // {
        //     Console.WriteLine($"MainForm: Pilote ajouté - {nom}");
        // }
        // DatabaseHelper.InitializeDatabase();
        // ChargerPilotes();
        Task.Run(async () => await InitializePilots());
    }
    catch (Exception ex)
    {
        Console.WriteLine($"MainForm: Exception encountered - {ex.Message}");
        Console.WriteLine(ex.StackTrace);
    }
}

    public async Task InitializePilots()   
    {
        // Attendre un peu pour donner le temps à l'UDP client de se connecter et de recevoir les données
        await Task.Delay(1000);

        // Récupérer les noms des pilotes
        string[] nomsDesPilotes = await ObtenirNomsDesPilotesAsync();
        AjouterNomsDesPilotesAuListBox(nomsDesPilotes);
    }

        private async Task OnEntryListComplete()
        {
            Console.WriteLine("MainForm: Entry list complete. Retrieving pilot names.");
        
            // Appeler la méthode pour récupérer les noms des pilotes
            string[] nomsDesPilotes = await ObtenirNomsDesPilotesAsync();
            listBoxPilotes.Items.AddRange(nomsDesPilotes);
        }
        private async Task ChargerPilotes()
        {
            // Supposons que nous ayons une méthode pour obtenir les noms des pilotes
            var pilotes = await ObtenirNomsDesPilotesAsync();
            foreach (var pilote in pilotes.Take(100))  // Limitation à 35 pilotes
            {
                listBoxPilotes.Items.Add(pilote);
                Console.WriteLine($"Pilote ajouté : {pilote}");
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
                // await ChargerPilotes();  // Recharger les pilotes pour mettre à jour les notes
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

        // private void buttonSpecialEvent_Click(object sender, EventArgs e)
        // {
        //     // Ouvre l'interface spéciale pour les événements avec jusqu'à 100 pilotes
        //     var specialForm = new SpecialEventForm();
        //     specialForm.ShowDialog();
        // }

        // private string[] ObtenirNomsDesPilotes()
        // {
        //     Console.WriteLine("ObtenirNomsDesPilotes: Demande des données au client UDP.");

        //     // Demande des données à l'UDP client
        //     _udpClient.RequestData();
        //     System.Threading.Thread.Sleep(4000); 

        //     Console.WriteLine("ObtenirNomsDesPilotes: Données demandées.");

        //     // Obtient la liste des pilotes depuis le client
        //     List<CarInfo> cars = _udpClient.GetPilots();

        //     if (cars == null || cars.Count == 0)
        //     {
        //         string[] drivername = { "Aucun pilote trouvé" };
        //         Console.WriteLine("ObtenirNomsDesPilotes: Aucune voiture récupérée.");
        //         drivername = ObtenirNomsDesPilotes();
        //         return drivername;
        //     }

        //     Console.WriteLine($"ObtenirNomsDesPilotes: {cars.Count} voitures récupérées.");

        //     // Extraire les noms des pilotes à partir de la liste des voitures
        //     List<string> driverNames = new List<string>();
        //     foreach (var car in cars)
        //     {
        //         Console.WriteLine($"ObtenirNomsDesPilotes: Traitement de la voiture {car.CarIndex}.");

        //         foreach (var driver in car.Drivers)
        //         {
        //             string driverName = $"{driver.FirstName} {driver.LastName}";
        //             driverNames.Add(driverName);
        //             Console.WriteLine($"ObtenirNomsDesPilotes: Pilote récupéré - {driverName}.");
        //         }
        //     }

        //     // Retourner un tableau de noms de pilotes
        //     return driverNames.ToArray();
        // }
        private async Task<string[]> ObtenirNomsDesPilotesAsync()
    {
        Console.WriteLine("ObtenirNomsDesPilotes: Demande des données au client UDP.");

        // Demande des données à l'UDP client
        _udpClient.RequestData();

        // Attendre un délai pour que les données soient reçues
        await Task.Delay(4000);

        Console.WriteLine("ObtenirNomsDesPilotes: Données demandées.");

        // Obtient la liste des pilotes depuis le client
        List<CarInfo> cars = _udpClient.GetPilots();

        if (cars == null || cars.Count == 0)
        {
            Console.WriteLine("ObtenirNomsDesPilotes: Aucune voiture récupérée.");
            return new string[0];
        }

        Console.WriteLine($"ObtenirNomsDesPilotes: {cars.Count} voitures récupérées.");

        // Extraire les noms des pilotes à partir de la liste des voitures
        List<string> driverNames = new List<string>();
        foreach (var car in cars)
        {
            Console.WriteLine($"ObtenirNomsDesPilotes: Traitement de la voiture {car.CarIndex}.");

            foreach (var driver in car.Drivers)
            {
                string driverName = $"{driver.FirstName} {driver.LastName}";
                // Ajout uniquement si le nom n'est pas déjà dans la liste
                if (!driverNames.Contains(driverName))
                {
                    driverNames.Add(driverName);
                    Console.WriteLine($"ObtenirNomsDesPilotes: Pilote récupéré - {driverName}.");
                }
            }
        }

        return driverNames.ToArray();
    }

        private void AjouterNomsDesPilotesAuListBox(string[] nomsDesPilotes)
    {
        // Utiliser Invoke pour s'assurer que les mises à jour du contrôle UI se font sur le thread UI
        if (listBoxPilotes.InvokeRequired)
        {
            listBoxPilotes.Invoke(new Action(() => listBoxPilotes.Items.AddRange(nomsDesPilotes)));
        }
        else
        {
            listBoxPilotes.Items.AddRange(nomsDesPilotes);
        }

        Console.WriteLine("MainForm: Noms des pilotes ajoutés au ListBox.");
        foreach (var nom in nomsDesPilotes)
        {
            Console.WriteLine($"MainForm: Pilote ajouté - {nom}");
        }
    }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Arrêter l'UDP client proprement lors de la fermeture du formulaire
            _udpClient.Shutdown();
            _udpClient.Dispose();
        }
        

    // Event handler to display driver names
    
    private void MettreAJourListePilotes(string[] nomsDesPilotes)
    {
        listBoxPilotes.Items.Clear();
        listBoxPilotes.Items.AddRange(nomsDesPilotes);
        Console.WriteLine("MainForm: Noms des pilotes ajoutés au ListBox.");
    }

    private async void buttonUpdate_Click(object sender, EventArgs e)
    {
        // Appeler la méthode pour récupérer les noms des pilotes
        string[] nomsDesPilotes = await ObtenirNomsDesPilotesAsync();
        MettreAJourListePilotes(nomsDesPilotes);
    }

    private void buttonCopyNote_Click(object sender, EventArgs e){
    string nomPilote = listBoxPilotes.SelectedItem.ToString();

    // Vérifie si une note est sélectionnée dans la ListBox des notes
    if (DatabaseHelper.ObtenirNotePilote(nomPilote) != null)
    {

        // Récupère le texte de la note sélectionnée
        string noteText = DatabaseHelper.ObtenirNotePilote(nomPilote);

        // Copie le texte dans le presse-papiers
        Clipboard.SetText(noteText);

        // Optionnel: Affiche un message de confirmation
        // MessageBox.Show("La note a été copiée dans le presse-papiers.", "Note copiée", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    else
    {
        // Optionnel: Affiche un message d'erreur si aucune note n'est sélectionnée
        MessageBox.Show("Aucune note sélectionnée à copier.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}

    }     
}       
            
            
        
    

