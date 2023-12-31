using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SteamMUD.Server;

// <summary>
// Initialized the telnet server for incoming and outgoing connections
// </summary>
public class TelnetServer
{
    private TcpListener listener;
    private bool isRunning;
    private string steamModBanner = @" __ _                                      ___ 
/ _\ |_ ___  __ _ _ __ ___   /\/\  /\ /\  /   \
\ \| __/ _ \/ _` | '_ ` _ \ /    \/ / \ \/ /\ /
_\ \ ||  __/ (_| | | | | | / /\/\ \ \_/ / /_// 
\__/\__\___|\__,_|_| |_| |_\/    \/\___/___,'";
    private string welcomeMessage = "\nWelcome to the start of your adventure in the world of SteamMOD!\nTo exit the program, type 'quit'!\n";

    public event Action<string, int, string> CommandReceived;
    
    /// <summary>
    /// Starts the telnet server on the IP and Port specified
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <param name="port"></param>
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

                Thread clientThread = new Thread(HandleClient!);
                clientThread.Start(client);
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"ERROR starting the telnet server: {ex.Message}");
        }
    }

    /// <summary>
    /// Stops the telnet server
    /// </summary>
    public void Stop()
    {
        if (isRunning)
        {
            Console.WriteLine("Telnet server has been stopped");
            listener.Stop();
            isRunning = false;
        }
    }
    
    /// <summary>
    /// Handles clients communication
    /// </summary>
    /// <param name="clientObj"></param>
    private void HandleClient(object clientObj)
    {
        TcpClient client = (TcpClient)clientObj;

        try
        {
            // Get client IP and port
            string clientAddress = (((IPEndPoint)client.Client.RemoteEndPoint)).Address.ToString();
            int clientPort = ((IPEndPoint)client.Client.RemoteEndPoint).Port;

            Console.WriteLine($"New client connected: IP {clientAddress} Port: {clientPort}");

            NetworkStream stream = client.GetStream();

            byte[] steamModBannerByte = Encoding.ASCII.GetBytes(steamModBanner);
            byte[] welcomeMessageByte = Encoding.ASCII.GetBytes(welcomeMessage);

            stream.Write(steamModBannerByte, 0, steamModBannerByte.Length);
            stream.Write(welcomeMessageByte, 0, welcomeMessageByte.Length);

            // Processes incoming chats
            while (isRunning)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                // If no bytes coming in, the client has disconnected
                if (bytesRead == 0)
                {
                    Console.WriteLine($"Client Disconnected from {clientAddress}:{clientPort}");
                    break;
                }

                string command = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                if (command.Trim().ToLower() == "quit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Received command from {clientAddress}:{clientPort} - {command}");
                    OnCommandReceived(clientAddress, clientPort, command);
                }
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"ERROR handling the client connection: {ex.Message}");
        }
        finally
        {
            client.Close();
        }
    }
    
    /// <summary>
    /// Invokes the Commands from the console.
    /// TODO: Get this working!!!
    /// </summary>
    /// <param name="clientAddress"></param>
    /// <param name="clientPort"></param>
    /// <param name="command"></param>
    protected virtual void OnCommandReceived(string clientAddress, int clientPort, string command)
    {
        CommandReceived?.Invoke(clientAddress, clientPort, command);
    }
}