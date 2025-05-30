
using System.Text.Json.Serialization;
using Dependency_Injection.Model;
using Dependency_Injection.Utilities;
using Newtonsoft.Json;
 class Programs 
{
    static List<Book> BookList;
    static void printBook(List<Book> books)
    {
        Console.WriteLine("List of books");
            Console.WriteLine("===============");

        foreach (var book in books)
            {
            Console.WriteLine($"{book.Title.PadRight(39, ' ')} " + $"{book.Author.PadRight(15, ' ')} {book.Price} ");
        }
        Console.ReadLine();
    }
     static void Main(string[] args)
    {
       Console.WriteLine("Please, press Yes to read the first file, or No to read the second file");
        Console.WriteLine("topic to include topic books");
        var ans = Console.ReadLine();
    BookList= ans?.ToLower() switch
        {
            "yes" => Utilities.ReadData(),
            "no" => Utilities.ReadDataExtra(""),
            "topic" => Utilities.ReadDataExtra("topic"),
            _ => new List<Book>()
        };
        if (BookList.Count == 0)
        {
            Console.WriteLine("No books found.");
        }
        else
        {
            printBook(BookList);
        }
    }
}