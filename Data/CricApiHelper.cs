﻿using CricketFavourites.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public static async Task<PlayerList> GetPlayers(string query)
        {
            PlayerList players = new PlayerList();

            string url = BASE_URL + string.Format(PLAYER_SEARCH_ENDPOINT, API_KEY, query);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                string json = await response.Content.ReadAsStringAsync();

                players = JsonConvert.DeserializeObject<PlayerList>(json);
            }

            return players;
        }

        public static async Task<PlayerInfo> GetPlayerInfo (int pid)
        {
            PlayerInfo player = new PlayerInfo();

            string url = BASE_URL + string.Format(PLAYER_ENDPOINT, API_KEY, pid);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                string json = await response.Content.ReadAsStringAsync();

                player = JsonConvert.DeserializeObject<List<PlayerInfo>>(json).FirstOrDefault();
            }

            return player;
        }
    }
}
