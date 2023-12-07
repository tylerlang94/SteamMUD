using System.Net.Sockets;
using System.Text;
using SteamMUD.Server;

namespace SteamMUD;

public class Commands
{


    public void CommandHandler(TelnetServer telnetServer)
    {
        telnetServer.CommandReceived += OnCommandReceived;
    }

    private void OnCommandReceived(string clientAddress, int clientPort, string command)
    {
        // Process the received command here
        Console.WriteLine($"Command received from {clientAddress}:{clientPort} - {command}");

        // Example: Check if the command is a specific keyword
        if (command.Trim().ToLower() == "help")
        {
            // Perform the "help" command action
            SendHelpMessage(clientAddress, clientPort);
        }
        else
        {
            // Handle other commands or implement your own logic
            Console.WriteLine($"Unknown command: {command}");
        }
    }

    private void SendHelpMessage(string clientAddress, int clientPort)
    {
        // Implement the logic to send the help message to the client
        string helpMessage = "This is the help message. Add your help content here.";
        byte[] helpMessageBytes = Encoding.ASCII.GetBytes(helpMessage);

        // Get the client's NetworkStream
        NetworkStream stream = GetClientStream(clientAddress, clientPort);

        // Send the help message to the client
        stream.Write(helpMessageBytes, 0, helpMessageBytes.Length);
    }

    private NetworkStream GetClientStream(string clientAddress, int clientPort)
    {
        // Implement logic to get the NetworkStream for the specified client
        // You might need to maintain a collection of active clients in your TelnetServer class
        // and look up the correct NetworkStream based on clientAddress and clientPort
        // For demonstration purposes, let's assume you have a TelnetServer property in the Commands class

        // Replace the following line with your actual logic to get the NetworkStream
        // This is just a placeholder
        return null;
    }
}
