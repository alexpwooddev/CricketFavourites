﻿using CricketFavourites.Data.Repositories;
using CricketFavourites.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Data
{
    public class NewsApiRepository : INewsApiRepository
    {

        public string API_KEY = null;
        public const string BASE_URL = "https://bing-news-search1.p.rapidapi.com/";
        public const string SEARCH_ENDPOINT = "news/search?textFormat=Raw&safeSearch=Off&q={0}";

        public NewsApiRepository(IConfiguration config)
        {
            API_KEY = config["BingNewsSearchApi:ApiKey"];
        }

        public NewsItems GetNews(string query)
        {
            NewsItems newsItems = new NewsItems();

            var client = new RestClient($"https://bing-news-search1.p.rapidapi.com/news/search?freshness=Day&textFormat=Raw&safeSearch=Off&q={query}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "bing-news-search1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", API_KEY);
            request.AddHeader("x-bingapis-sdk", "true");
            IRestResponse response = client.Execute(request);

            return JsonConvert.DeserializeObject<NewsItems>(response.Content);
        }

    }
}
