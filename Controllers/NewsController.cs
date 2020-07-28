using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CricketFavourites.Data.Repositories;
using CricketFavourites.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CricketFavourites.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly INewsApiRepository _newsApiRepository;

        public NewsController(INewsApiRepository newsApiRepository)
        {
            _newsApiRepository = newsApiRepository;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ShowNews()
        {
            string query = "Steve Waugh";

            var newsItems = await _newsApiRepository.GetNews(query);

            return View(newsItems);
        }
    }
}
