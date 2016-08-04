using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BissnesLogic
{
   public class Classes
    {
        public class AnimeList {
            public string Name { get; set; }
            public string Poster { get; set; }
            public string Url { get; set; }
            public string Opis { get; set; }
            public string Update { get; set; }
            public string Year { get; set; }
            public string Series { get; set; }
        }
        public class AnimeRasp
        {
            public string Url { get; set; }
            public string Name { get; set; }
        }
        public class Raspisanie
        {
            public string Day { get; set; }
            public AnimeRasp[] Anime { get; set; }
        }
        public class AnimeView
        {
            public string Name { get; set; }
            public string Year { get; set; }
            public string Series { get; set; }
            public string AllSeries { get; set; }
            public string Tip { get; set; }
            public string Rej { get; set; }
            public string Genre { get; set; }
            public string Poster { get; set; }
            public string Opis { get; set; }
            public List<string> Screen { get; set; }
            public string VideoCnt { get; set; }
            public List<Video> Video { get; set; }

        }
        public class Video
        {
            public string Id { get; set; }
            public string Text { get; set; }
        }
     
    }
}
