using SteamMUD.Server;

public class Program
{
    public static void Main()
    {
        
        TelnetServer telnet = new();
        
        telnet.Start("127.0.0.1", 8080);

        Console.ReadLine();
        
        telnet.Stop();
    }
}