﻿namespace IgdbApi.Lib.Models
{
    public class IgdbGame
    {
        public int id { get; set; }
        public int[] involved_companies { get; set; }
        public string name { get; set; }
        public int[] platforms { get; set; }
    }
}