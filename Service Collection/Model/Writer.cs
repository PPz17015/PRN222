using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Collection.Model
{
    public class Writer : IJSonWriter
    {
        public void WriteJson()
        {
            Console.WriteLine("{'message' : 'Writing in json format'}");
        }

    }
    public class XMLWriter : IXMLWriter
    {
        public void WriteXML()
        {
            Console.WriteLine("<message>Writing in xml format</message>");
        }
    }
}
