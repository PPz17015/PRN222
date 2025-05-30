using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Segregation_Principle_Demo.Model
{
  class Video : IVideo
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }

    }
    class TopicVideo : Video, ITopicVideo
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        public string Topic { get; set; }
    }
    class Videos : IVideo,ITopicVideo, IDurationVideo
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        public string Topic { get; set; }

        public string Duration { get; set; }
    }
}
