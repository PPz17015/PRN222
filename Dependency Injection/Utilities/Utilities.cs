using Dependency_Injection.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injection.Utilities
{
    internal class Utilities
    {
        static string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
        internal static List<Book> ReadData()
        {
            var CadJson= ReadFile("Data/BookStore.json");
            return JsonConvert.DeserializeObject<List<Book>>(CadJson) ?? new List<Book>();
        }
        internal static List<Book> ReadDataExtra(String extra)
        {
            List<Book> bookList =ReadData();
            var CadJson=ReadFile("Data/Bookstore2.json");
            bookList.AddRange(JsonConvert.DeserializeObject<List<Book>>(CadJson) ?? new List<Book>());
            if (extra == "topic")
            {
                CadJson = ReadFile("Data/BookStore3.json");
bookList.AddRange(JsonConvert.DeserializeObject<List<TopicBook>>(CadJson));
            }
            return bookList;

        }
    }
}
