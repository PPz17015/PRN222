using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Segregation_Principle_Demo.Model
{
    interface IVideo
    {
        String Title { get; set; }
        String Author { get; set; }
        Double Price { get; set; }
    }
    interface ITopicVideo : IVideo
    {
        String Topic { get; set; }
    }
    interface IDurationVideo
    {
        String Duration { get; set; }
    }
}