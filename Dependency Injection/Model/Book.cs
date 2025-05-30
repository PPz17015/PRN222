using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injection.Model
{
    public class Book : IBook 
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
    public class TopicBook : Book
    {
        public string Topic { get; set; }
    }
}
