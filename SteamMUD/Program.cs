using SteamMUD;
using SteamMUD.Server;
public class Program
{
    public static void Main()
    {
        TelnetServer telnet = new TelnetServer();
        Commands commandHandler = new Commands();

        // Subscribe the Commands class to the CommandReceived event in TelnetServer
        commandHandler.CommandHandler(telnet);

        // Start the Telnet server
        telnet.Start("127.0.0.1", 8080);

        Console.WriteLine("Server is running. Press Enter to stop...");

        // Wait for user input (Enter key) to stop the server
        Console.ReadLine();

        // Stop the Telnet server
        telnet.Stop();
    }
}