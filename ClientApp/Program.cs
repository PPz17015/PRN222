


using System.Net.Sockets;

class program
{
    static void connectServer(String host, int port)
    {
        String message, responseData;
        int bytes;
        try
        {
            TcpClient client = new TcpClient(host, port);
            Console.Title = "Client";
            NetworkStream stream = null;
            Console.WriteLine("Connected to server");
            while (true)
            {
                Console.Write("Enter message: ");
                message = Console.ReadLine();
                if (message == "exit") break;
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", message);
                data = new Byte[256];
                bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);
            }
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: {0}", ex.Message);
        }

    }
    public static void Main(string[] args)
    {
        string server = "127.0.0.1";
        int port = 8080;
        connectServer(server, port);
    }
}