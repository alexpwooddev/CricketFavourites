﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Models
{
    public class NewsItems
    {
        public string _type { get; set; }
        public string readLink { get; set; }
        public int totalEstimatedMatches { get; set; }
        
        [JsonProperty("value")]
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string _type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public DateTime datePublished { get; set; }
        public string category { get; set; }

    }


}
