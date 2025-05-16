


using System.Net;
using System.Net.Sockets;

class program
{

    static void ConnectServer(String Host,int port)
    {
        UdpClient udpClient = new UdpClient();
        IPAddress localAddr = IPAddress.Parse(Host);
        IPEndPoint remoteEP = new IPEndPoint(localAddr, port);
        String message;
        int count = 0;
        bool done = false;
        Console.Title = "UDP Client";
        try
        {
            Console.WriteLine("Starting UDP client...");
            Console.WriteLine('*' + new string('-', 50) + '*');
           udpClient.Connect(remoteEP);
            while (!done)
            {
                message = $"Message {count + 1:D2}";
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                udpClient.Send(data, data.Length);
                Console.WriteLine("Sent: {0}", message);

                byte[] response = udpClient.Receive(ref remoteEP);
                string receivedMsg = System.Text.Encoding.ASCII.GetString(response);
                Console.WriteLine("Received: {0} at {1:t}", receivedMsg, DateTime.Now);
                string upperMsg = receivedMsg.ToUpper();
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(upperMsg);
                udpClient.Send(msg, msg.Length);

                count++;
                if (count == 10)
                {
                    done = true;
                    Console.WriteLine("Exiting...");
                }

                Thread.Sleep(1000);
            }


        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: {0}", ex.Message);
        }
        finally
        {
            udpClient.Close();
        }
    }

    static void Main(String[] args)
    {
        string host = "127.0.0.1";
        int port = 8080;
        ConnectServer(host, port);
        Console.WriteLine("Press any key to exit...");
        Console.Read();
    }
}