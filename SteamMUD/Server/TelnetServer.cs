using System.Net;
using System.Net.Sockets;

namespace SteamMUD.Server;

// <summary>
// Initialized the telnet server for incoming and outgoing connections
// </summary>
public class TelnetServer
{
    private TcpListener listener;
    private bool isRunning;
    
    // Starts the telnet server
    public void Start(string ipAddress, int port)
    {
        try
        {
            listener = new TcpListener(IPAddress.Parse(ipAddress), port);

            listener.Start();
            isRunning = true;

            if (isRunning)
            {
                Console.WriteLine($"Server has started at {ipAddress}:{port}");
            }

            while (isRunning)
            {
                TcpClient client = listener.AcceptTcpClient();

                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(client);
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"ERROR starting the telnet server: {ex.Message}");
        }
    }

    public void Stop()
    {
        if (isRunning)
        {
            listener.Stop();
            isRunning = false;
            
            Console.WriteLine("Telnet server has been stopped");
        }
    }
    
    // Will handle connections
    private void HandleClient()
    {
        
    }
}