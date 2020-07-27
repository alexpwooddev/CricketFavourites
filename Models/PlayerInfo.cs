using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Models
{
    public class PlayerInfo
    {
        public int pid { get; set; }
        public string profile { get; set; }
        public string imageURL { get; set; }
        public string battingStyle { get; set; }
        public string bowlingStyle { get; set; }
        public string majorTeams { get; set; }
        public string currentAge { get; set; }
        public string born { get; set; }
        public string fullName { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public string playingRole { get; set; }
        public Data data { get; set; }
    }
    


    public class Data
    {
        public Bowling bowling { get; set; }
        public Batting batting { get; set; }

    }

    public class Bowling
    {
        public ListA listA { get; set; }
        public FirstClass firstClass { get; set; }
        public T20Is T20Is { get; set; }
        public ODIs ODIs { get; set; }
        public Tests tests { get; set; }

    }

    public class Batting
    {
        public ListA2 listA { get; set; }
        public FirstClass2 firstClass { get; set; }
        public T20Is2 T20Is { get; set; }
        public ODIs2 ODIs { get; set; }
        public Tests2 tests { get; set; }

    }

   



    public class ListA
    {
        [JsonProperty("10")]
        public string ten { get; set; }
        [JsonProperty("5w")]
        public string fiveW { get; set; }
        [JsonProperty("4w")]
        public string fourW { get; set; }
        public string SR { get; set; }
        public string Econ { get; set; }
        public string Ave { get; set; }
        public string BBM { get; set; }
        public string BBI { get; set; }
        public string Wkts { get; set; }
        public string Runs { get; set; }
        public string Balls { get; set; }
        public string Inns { get; set; }
        public string Mat { get; set; }

    }

    public class FirstClass
    {
        [JsonProperty("10")]
        public string ten { get; set; }
        [JsonProperty("5w")]
        public string fiveW { get; set; }
        [JsonProperty("4w")]
        public string SR { get; set; }
        public string Econ { get; set; }
        public string Ave { get; set; }
        public string BBM { get; set; }
        public string BBI { get; set; }
        public string Wkts { get; set; }
        public string Runs { get; set; }
        public string Balls { get; set; }
        public string Inns { get; set; }
        public string Mat { get; set; }

    }

    public class T20Is
    {
        [JsonProperty("10")]
        public string ten { get; set; }
        [JsonProperty("5w")]
        public string fiveW { get; set; }
        [JsonProperty("4w")]
        public string SR { get; set; }
        public string Econ { get; set; }
        public string Ave { get; set; }
        public string BBM { get; set; }
        public string BBI { get; set; }
        public string Wkts { get; set; }
        public string Runs { get; set; }
        public string Balls { get; set; }
        public string Inns { get; set; }
        public string Mat { get; set; }

    }

    public class ODIs
    {
        [JsonProperty("10")]
        public string ten { get; set; }
        [JsonProperty("5w")]
        public string fiveW { get; set; }
        [JsonProperty("4w")]
        public string SR { get; set; }
        public string Econ { get; set; }
        public string Ave { get; set; }
        public string BBM { get; set; }
        public string BBI { get; set; }
        public string Wkts { get; set; }
        public string Runs { get; set; }
        public string Balls { get; set; }
        public string Inns { get; set; }
        public string Mat { get; set; }

    }

    public class Tests
    {
        [JsonProperty("10")]
        public string ten { get; set; }
        [JsonProperty("5w")]
        public string fiveW { get; set; }
        [JsonProperty("4w")]
        public string SR { get; set; }
        public string Econ { get; set; }
        public string Ave { get; set; }
        public string BBM { get; set; }
        public string BBI { get; set; }
        public string Wkts { get; set; }
        public string Runs { get; set; }
        public string Balls { get; set; }
        public string Inns { get; set; }
        public string Mat { get; set; }

    }

   

    public class ListA2
    {
        [JsonProperty("50")]
        public string fifty { get; set; }
        [JsonProperty("100")]
        public string oneHundred { get; set; }
        public string St { get; set; }
        public string Ct { get; set; }
        [JsonProperty("6s")]
        public string sixes { get; set; }
        [JsonProperty("4s")]
        public string fours { get; set; }
        public string SR { get; set; }
        public string BF { get; set; }
        public string Ave { get; set; }
        public string HS { get; set; }
        public string Runs { get; set; }
        public string NO { get; set; }
        public string Inns { get; set; }
        public string Mat { get; set; }

    }

    public class FirstClass2
    {
        [JsonProperty("50")]
        public string fifty { get; set; }
        [JsonProperty("100")]
        public string oneHundred { get; set; }
        public string St { get; set; }
        public string Ct { get; set; }
        [JsonProperty("6s")]
        public string sixes { get; set; }
        [JsonProperty("4s")]
        public string fours { get; set; }
        public string SR { get; set; }
        public string BF { get; set; }
        public string Ave { get; set; }
        public string HS { get; set; }
        public string Runs { get; set; }
        public string NO { get; set; }
        public string Inns { get; set; }
        public string Mat { get; set; }

    }

    public class T20Is2
    {
        [JsonProperty("50")]
        public string fifty { get; set; }
        [JsonProperty("100")]
        public string oneHundred { get; set; }
        public string St { get; set; }
        public string Ct { get; set; }
        [JsonProperty("6s")]
        public string sixes { get; set; }
        [JsonProperty("4s")]
        public string fours { get; set; }
        public string SR { get; set; }
        public string BF { get; set; }
        public string Ave { get; set; }
        public string HS { get; set; }
        public string Runs { get; set; }
        public string NO { get; set; }
        public string Inns { get; set; }
        public string Mat { get; set; }

    }

    public class ODIs2
    {
        [JsonProperty("50")]
        public string fifty { get; set; }
        [JsonProperty("100")]
        public string oneHundred { get; set; }
        public string St { get; set; }
        public string Ct { get; set; }
        [JsonProperty("6s")]
        public string sixes { get; set; }
        [JsonProperty("4s")]
        public string fours { get; set; }
        public string SR { get; set; }
        public string BF { get; set; }
        public string Ave { get; set; }
        public string HS { get; set; }
        public string Runs { get; set; }
        public string NO { get; set; }
        public string Inns { get; set; }
        public string Mat { get; set; }

    }

    public class Tests2
    {
        [JsonProperty("50")]
        public string fifty { get; set; }
        [JsonProperty("100")]
        public string oneHundred { get; set; }
        public string St { get; set; }
        public string Ct { get; set; }
        [JsonProperty("6s")]
        public string sixes { get; set; }
        [JsonProperty("4s")]
        public string fours { get; set; }
        public string SR { get; set; }
        public string BF { get; set; }
        public string Ave { get; set; }
        public string HS { get; set; }
        public string Runs { get; set; }
        public string NO { get; set; }
        public string Inns { get; set; }
        public string Mat { get; set; }

    }

    
}
