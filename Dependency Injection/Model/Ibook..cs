using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injection.Model
{
    interface IBook
    {
        string Title { get; set; }
        string Author { get; set; }
        decimal Price { get; set; }
    }
    
}