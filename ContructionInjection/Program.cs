
using ContructionInjection.Model;

public class Program
{
    public static void Main(string[] args)
    {
        BookManager bm;
        Console.Title = "DI-Contruction Injection demo";
        Console.WriteLine("Please choose an option:");
        Console.WriteLine("1. Read book in XML format");
        Console.WriteLine("2. Read book in JSON format");
        var option = Console.ReadLine();
        if (option == "1")
        {
            bm = new BookManager(new XMLBookReader());
            bm.ReadBook();
        }
        else if (option == "2")
        {
            bm = new BookManager(new JSONBookReader());
            bm.ReadBook();
        }
        else if (option == "bg")
        {
            Console.WriteLine("        _     _");
            Console.WriteLine("      _( )_ _( )_");
            Console.WriteLine("   .-'     Y     '-.");
            Console.WriteLine("  /  _     |     _  \\");
            Console.WriteLine(" |  ( )    |    ( )  |");
            Console.WriteLine("  \\   '-._ | _.-'   /");
            Console.WriteLine("   '-.    | |    .-'");
            Console.WriteLine("      '--' ' '--'");
            Console.WriteLine("       /       \\");
            Console.WriteLine("      (         )");
            Console.WriteLine("       \\_/ \\_/");


        }
        else
        {
            Console.WriteLine("Invalid option selected.");
        }
    }
}