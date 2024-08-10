// Program.cs
using System;
using System.Threading.Tasks;
using RaceElement.Broadcast;

class Program
{
    static async Task Main(string[] args)
    {
        // Load the configuration
        var config = BroadcastConfig.GetConfiguration();
        if (config == null)
        {
            Console.WriteLine("Failed to load configuration.");
            return;
        }

        // Create and initialize the UDP client
        var client = new ACCUdpRemoteClient("127.0.0.1", config.UpdListenerPort, "MyDisplayName", config.ConnectionPassword, config.CommandPassword, 9000);

        // Subscribe to the event to receive driver names
        client.OnDriverNamesReceived += DisplayDriverNames;

        // Request data (this would typically be called in response to some event in a real application)
        Console.WriteLine("Requesting data...");
        client.RequestData();

        // Keep the application running to listen for incoming data
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();

        // Shutdown the client
        await client.ShutdownAsnyc();
    }

    // Event handler to display driver names
    static void DisplayDriverNames(string[] driverNames)
    {
        Console.WriteLine("Driver Names Received:");
        foreach (var name in driverNames)
        {
            Console.WriteLine(name);
        }
    }
}
