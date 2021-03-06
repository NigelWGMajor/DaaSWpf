﻿using System;

namespace DaaSWpf
{
    public class JokeCollection
    {
        /* expected format:
        {   "current_page":1,
            "limit":30,
            "next_page":1,
            "previous_page":1,
            "results":[{"id":"82wHlbaapzd","joke":"Me: If humans lose the ability to hear high frequency volumes as they get older, c 
        */
        public int current_page { get; set; }
        public int limit { get; set; }
        public int next_page { get; set; }
        public int previous_page { get; set; }
        public Joke[] results { get; set; }
    }
}
