    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Collection.Model
{
    internal interface IJSonWriter
    {
        void WriteJson();
    }
    public interface IXMLWriter
    {
        void WriteXML();
    }
}
