

using System.Net;

class program
{
    private static void DownloadAsynchoronously()
    {
        WebClient client= new WebClient();
        client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadCompleted);
        client.DownloadStringAsync(new Uri("http://www.google.com"));
    }

    private static void DownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
        if (e.Error != null)
        {
            Console.WriteLine("Error: " + e.Error.Message);
        }
        else
        {
            Console.WriteLine(e.Result);
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Download completed successfully.");
        }
    }
    static void Main(string[] args)
    {
        Console.WriteLine("Starting download...");
        DownloadAsynchoronously();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}