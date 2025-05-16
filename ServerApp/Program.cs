
using System.IO;
using System.Net;
using System.Net.Sockets;
class program
{
    static void ProcessManager(object parm)
    {
        string data;
        int count;
        try
        {
            TcpClient client = parm as TcpClient;
            Byte[] bytes = new Byte[256];
            NetworkStream stream = client.GetStream();
            while ((count = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, count);
                Console.WriteLine("Received: {0} at {1:t}", data, DateTime.Now); 
                data = $"{data.ToUpper()}";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                stream.Write(msg, 0, msg.Length);
                Console.WriteLine("Sent: {0}", data);
            }
            client.Close();
        }
        catch (Exception ex) 
        {
            Console.WriteLine("Error: {0}", ex.Message);
        }
    }

    static void ExcuteServer(String host, int port)
    {
        int count = 0;
        TcpListener server = null; 
        try
        {
            Console.WriteLine("Starting server...");
            IPAddress localAddr = IPAddress.Parse(host);
            server = new TcpListener(localAddr, port); 
            server.Start();

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");
                Thread clientThread = new Thread(ProcessManager);
                clientThread.Start(client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: {0}", ex.Message);
        }
        finally
        {
            server?.Stop();
        }
    }
  
    public static void Main()
    {
        String host = "127.0.0.1";
        int port = 8080;
        ExcuteServer(host, port);   }
}
