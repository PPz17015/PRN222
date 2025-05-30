using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContructionInjection.Model
{
   public interface IBookReader
    {
        void readBook();
    }
    public class XMLBookReader : IBookReader
    {
        public void readBook()
        {
            Console.WriteLine("Reading book in XML format");
        }
    }
    public class JSONBookReader : IBookReader
    {
        public void readBook()
        {
            Console.WriteLine("Reading book in JSON format");
        }
    }
}
