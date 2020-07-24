using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Data
{
    public class CricApiHelper
    {
        public const string BASE_URL = "https://cricapi.com/api/";
        public const string API_KEY = "?apikey=NbfhBv24s7REWqDADZzeRvGvsAF2";
        public const string PLAYER_SEARCH_ENDPOINT = "playerFinder{0}&name={1}";
        public const string PLAYER_ENDPOINT = "playerStats{0}&pid={1}";


    }
}
