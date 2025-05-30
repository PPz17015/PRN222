
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        Console.Title="DI-service collection demo";
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<Service_Collection.Model.IXMLWriter, Service_Collection.Model.XMLWriter>();
        serviceCollection.AddScoped<Service_Collection.Model.IJSonWriter, Service_Collection.Model.Writer>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        Console.WriteLine("Service Collection Demo");
        Console.WriteLine("Mapping interfaces to implementations using DI container");
        Console.WriteLine("PLease choose an option:");
        Console.WriteLine("1. Write in XML format");
        Console.WriteLine("2. Write in JSon format");
        var option = Console.ReadLine();
        if (option == "1")
        {
            var xmlWriter = serviceProvider.GetService<Service_Collection.Model.IXMLWriter>();
            xmlWriter?.WriteXML();
        }
        else if (option == "2")
        {
            var jsonWriter = serviceProvider.GetService<Service_Collection.Model.IJSonWriter>();
            jsonWriter?.WriteJson();
        }
        else
        {
            Console.WriteLine("Invalid option selected.");
        }


        var registeredServices = serviceProvider.GetServices<Service_Collection.Model.IXMLWriter>();
        foreach (var svc in registeredServices)
        {
            Console.WriteLine($"Registered service: {svc.ToString()}");
        }
Console.WriteLine(new String('-', 50));
        foreach (var svc in serviceCollection)
        {
            Console.WriteLine($"Type :{svc.ImplementationType}\n" +
                $"LifeTime: {svc.Lifetime}\n" +
                $"ServiceType: {svc.ServiceType}"
                );
        }
        Console.ReadLine();
    }
}