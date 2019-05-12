using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Newtonsoft.Json;

namespace DaaSWpf
{
    public class Joke
    {
        public string id { get; set; }
        public string joke { get; set; }
        public string status { get; set; }
        public int wordcount { get; set; }
        public string wordCountText { get { return (wordcount == 1 ? "1 word" : $"{wordcount} words"); } }
        public string size
        {
            get
            {
                if (wordcount < 10) return "S";
                if (wordcount > 19) return "L";
                else return "M";
            }
        }

        public string sizeTip
        {
            get
            {
                if (wordcount < 10) return "This joke is shorter than 10 words";
                if (wordcount > 19) return "This joke has at least 20 words";
                else return "This joke has at least 10 words, but less than 20.";
            }
        }

        public Joke() { }

        public void Update(string searchString)
        {
            wordcount = joke.Split(" .,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
            if (string.IsNullOrWhiteSpace(searchString))
                return;
            string pattern = $"({searchString})";
            joke = Regex.Replace(joke, pattern, searchString.ToUpper(), RegexOptions.IgnoreCase);
        }
    }
}

