using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DaaSWpf
{
    public class Joke
    {
        public string id { get; set; }
        public string joke { get; set; }
        public string status { get; set; }
        public Joke()
        {
        }
    }
}
