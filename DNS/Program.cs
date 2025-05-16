using System.Net;


class DnsTestProgram
{
    static void Main(string[] args)
    {
        Console.WriteLine(new String('*', 50));
        var domainEntry = Dns.GetHostEntry("www.google.com");
        Console.WriteLine($"HostName: {domainEntry.HostName}");
        foreach (var ip in domainEntry.AddressList)
        {
            Console.WriteLine($"IP Address: {ip}");
        }
        Console.WriteLine(new String('*', 50));
        var domainEntryByAdress = Dns.GetHostEntry("127.0.0.1");
        Console.WriteLine($"HostName: {domainEntryByAdress.HostName}");
        foreach (var ip in domainEntryByAdress.AddressList)
        {
            Console.WriteLine($"IP Address: {ip}");
        }
        Console.ReadLine();
    }
}