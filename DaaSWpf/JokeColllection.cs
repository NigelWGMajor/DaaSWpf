using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaaSWpf
{
    public class JokeCollection
    {
        /*
        {   "current_page":1,
            "limit":30,
            "next_page":1,
            "previous_page":1,
            "results":[{"id":"82wHlbaapzd","joke":"Me: If humans lose the ability to hear high frequency volumes as they get older, c *
        */
        public int current_page { get; set; }
        public int limit { get; set; }
        public int next_page { get; set; }
        public int previous_page { get; set; }
        public Joke[] results { get; set; }
    }
}
