using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using Telemetry.Broadcast;
using System.Threading.Tasks;
using System.Collections.Generic; 
using Telemetry.Broadcast.Structs;




namespace MemoPilotes

{
    
    public partial class SpeMainForm : Form
    {
        private ACCUdpRemoteClient _udpClient;  // Creation of a new UDP client

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        public SpeMainForm()
        {
            try
            {
                AllocConsole();
                InitializeComponent();    
                Console.WriteLine("MainForm: Initialition of the UDP client.");
                DatabaseHelper.InitializeDatabase();
                // Initialize the UDP client 
                string ip = "127.0.0.1"; 
                int port = 9000; // port UDP used by the server
        
                var config = BroadcastConfig.GetConfiguration();

                _udpClient = new ACCUdpRemoteClient(ip, config.UpdListenerPort, "MyDisplayName", config.ConnectionPassword, config.CommandPassword, port);
                Console.WriteLine("MainForm: Initialisation done.");        
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
            // Create a delay to wait for the UDP client to be ready
            await Task.Delay(1000);

            // Get the list of driver names
            string[] nomsDesPilotes = await GetDriverNamesAsync();
            AddDriverNamesToListBox(nomsDesPilotes);
        }

        private async Task OnEntryListComplete()
        {
            Console.WriteLine("MainForm: Entry list complete. Retrieving pilot names.");
            
            // Get the list of driver names
            string[] nomsDesPilotes = await GetDriverNamesAsync();
            listBoxPilotes.Items.AddRange(nomsDesPilotes);
        }
        private async Task ChargerPilotes()
        {
            // Get the list of driver names
            var pilotes = await GetDriverNamesAsync();
            foreach (var pilote in pilotes.Take(35))  // Add only the first 35 drivers (if you need more, use the special event or increase this number)
            {
                listBoxPilotes.Items.Add(pilote);
                Console.WriteLine($"Driver add: {pilote}");
            }
        }

        private void buttonSave_Click(object sender, EventArgs e) // Save the note of the selected driver
        {
            if (listBoxPilotes.SelectedItem != null && !string.IsNullOrEmpty(textBoxNote.Text))
            {
                string nomPilote = listBoxPilotes.SelectedItem.ToString();
                string note = textBoxNote.Text;
                DatabaseHelper.AjouterOuMettreAJourPilote(nomPilote, note);
                MessageBox.Show("Note saved !");
                textBoxNote.Clear();
                    
            }
            else
            {
                MessageBox.Show("Please select a driver and enter a note.");
            }
        }

        private void listBoxPilotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPilotes.SelectedItem != null)
            {
                string nomPilote = listBoxPilotes.SelectedItem.ToString();
                string note = DatabaseHelper.ObtenirNotePilote(nomPilote);
                labelNoteExistante.Text = note ?? "No note recorded.";
            }
        }

        private void buttonSpecialEvent_Click(object sender, EventArgs e) // Open the special event form
        {            
            var specialForm = new SpeMainForm();
            specialForm.ShowDialog();
        }

        private async Task<string[]> GetDriverNamesAsync() // Get the driver names from the UDP client
        {
            Console.WriteLine("GetDriverNamesAsync: requested UDP data.");

            // Request data from the UDP client
            _udpClient.RequestData();

            // Wait for the data to be received
            await Task.Delay(4000);

            Console.WriteLine("GetDriverNamesAsync: Data requested.");

            // Get the list of cars from the UDP client
            List<CarInfo> cars = _udpClient.GetPilots();

            if (cars == null || cars.Count == 0)
            {
                Console.WriteLine("GetDriverNamesAsync: No car find.");
                return new string[0];
            }

            Console.WriteLine($"GetDriverNamesAsync: {cars.Count} salvaged cars.");

            // Get the list of driver names from the cars list
            List<string> driverNames = new List<string>();
            foreach (var car in cars)
            {
                Console.WriteLine($"GetDriverNamesAsync: Car treatment {car.CarIndex}.");

                foreach (var driver in car.Drivers)
                {
                    string driverName = $"{driver.FirstName} {driver.LastName}";
                    // Add the driver name to the list if it doesn't already exist
                    if (!driverNames.Contains(driverName))
                    {
                        driverNames.Add(driverName);
                        Console.WriteLine($"GetDriverNamesAsync: Driver recovered - {driverName}.");
                    }
                }
            }

            return driverNames.ToArray();
        }

            private void AddDriverNamesToListBox(string[] nomsDesPilotes)
        {
            // Add the driver names to the ListBox 
            if (listBoxPilotes.InvokeRequired)
            {
                listBoxPilotes.Invoke(new Action(() => listBoxPilotes.Items.AddRange(nomsDesPilotes)));
            }
            else
            {
                listBoxPilotes.Items.AddRange(nomsDesPilotes);
            }

            Console.WriteLine("MainForm:.Driver names added to ListBox");
            foreach (var nom in nomsDesPilotes)
            {
                Console.WriteLine($"MainForm: Driver added - {nom}");
            }
        }


            private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
            {
                // Shutdown the UDP client
                _udpClient.Shutdown();
                _udpClient.Dispose();
            }
            

        // Event handler to display driver names

        private void UpdateDriverlist(string[] nomsDesPilotes)
        {
            listBoxPilotes.Items.Clear();
            listBoxPilotes.Items.AddRange(nomsDesPilotes);
            Console.WriteLine("MainForm: Driver names added to ListBox.");
        }

        private async void buttonUpdate_Click(object sender, EventArgs e)
        {
            // Get the list of driver names
            string[] nomsDesPilotes = await GetDriverNamesAsync();
            UpdateDriverlist(nomsDesPilotes);
        }

        private void buttonCopyNote_Click(object sender, EventArgs e)
        {
            string nomPilote = listBoxPilotes.SelectedItem.ToString();

            // Check if a note is selected
            if (DatabaseHelper.ObtenirNotePilote(nomPilote) != null)
            {

                // Get the note text
                string noteText = DatabaseHelper.ObtenirNotePilote(nomPilote);

                // Copy the note to the clipboard
                Clipboard.SetText(noteText);
            }
            else
            {
                // Display an error message if no note is selected
                MessageBox.Show(" No note selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void buttonAddManualDriver_Click(object sender, EventArgs e)
        {
            // Get the first and last names from the text boxes
            string firstName = textBoxFirstName.Text.Trim();
            string lastName = textBoxLastName.Text.Trim();

            // Check that both fields are filled
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Please enter both the first name and last name.");
                return;
            }

            // Combine first and last names to create the driver's full name
            string fullName = $"{firstName} {lastName}";

            //check if this driver already exists in the list
            if (listBoxPilotes.Items.Contains(fullName))
            {
                MessageBox.Show("This driver is already in the list.");
                return;
            }

            // Add the driver's name to the ListBox
            listBoxPilotes.Items.Add(fullName);

            

            // Clear the text boxes after adding
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            textBoxNote.Clear();

            MessageBox.Show("Driver added successfully!");
        }

        



    }     
}       
            
            
        
    

