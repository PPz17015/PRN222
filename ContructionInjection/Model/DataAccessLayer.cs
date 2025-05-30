using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContructionInjection.Model
{
    public class BookManager
    {
        public IBookReader BookReader;

        public BookManager(IBookReader bookReader)
        {
            BookReader = bookReader;
        }
        public void ReadBook()
        {
            BookReader.readBook();
        }
    }
}
