using Interface_Segregation_Principle_Demo.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Segregation_Principle_Demo.Ultilities
{
    internal interface Ultilities
    {
        internal static  List<Videos> ReadData(String fileid)
        {
            var filename="Data/VideoStore" + fileid + ".json";
            var CadJson = ReadFile(filename);
            return JsonConvert.DeserializeObject<List<Videos>>(CadJson) ?? new List<Videos>();
        }
        internal static string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

    }
}
