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

        public async Task<IActionResult> ShowNews(string name)
        {
            //if not routed with a specific player then just show news from mix of favourites
            if (name == null)
            {
                var newsItems = await _newsApiRepository.GetNews(name);

                return View(newsItems);
            }

            else
            {
                var newsItems = await _newsApiRepository.GetNews(name);

                return View(newsItems);
            }
            

            
        }
    }
}
