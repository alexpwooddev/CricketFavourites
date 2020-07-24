using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Models
{
    public class PlayerList
    {
        public List<Player> data { get; set; }
        //[JsonIgnore]
        //public int ttl { get; set; }
        //[JsonIgnore]
        //public bool cache3 { get; set; }
        //[JsonIgnore]
        //public string v { get; set; }
        //[JsonIgnore]
        //public int creditsLeft { get; set; }

    }


    public class Player
    {
        public int pid { get; set; }
        public string fullName { get; set; }
        public string name { get; set; }

    }





}
