

using System.Net;
using System.Net.Sockets;

class program
{

    const int port = 8080;
    const String host = "127.0.0.1";
    private static void StartListener()
    {
        string message;
        UdpClient udpServer = new UdpClient(port);
        IPAddress localAddr = IPAddress.Parse(host);
        IPEndPoint remoteEP = new IPEndPoint(localAddr, port);
        Console.Title = "UDP Server";
        Console.WriteLine("Starting UDP server...");
        Console.WriteLine('*' + new string('-', 50) + '*');
        try
        {
            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                byte[] data = udpServer.Receive(ref remoteEP);
                message = System.Text.Encoding.ASCII.GetString(data);
                Console.WriteLine("Received: {0} at {1:t}", message, DateTime.Now);
                message = $"{message.ToUpper()}";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                udpServer.Send(msg, msg.Length, remoteEP);
                Console.WriteLine("Sent: {0}", message);
            }


        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: {0}", ex.Message);
        }
        finally
        {
            udpServer.Close();
        }
    }
    public static void Main()
    {
        Thread thread = new Thread(new ThreadStart(StartListener));
        thread.Start();
    }
}
           