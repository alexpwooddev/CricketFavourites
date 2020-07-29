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
        private readonly IFavouriteRepository _favouriteRepository;

        public NewsController(INewsApiRepository newsApiRepository, IFavouriteRepository favouriteRepository)
        {
            _newsApiRepository = newsApiRepository;
            _favouriteRepository = favouriteRepository;
        }

        public IActionResult DefaultNewsList()
        {
            //get names for 3 of user's favourites
            var currentUserFavourites = _favouriteRepository.GetCurrentUserFavourites();

            List<string> currentFavouritePlayerNames = new List<string>();

            foreach (Favourite fav in currentUserFavourites)
            {
                currentFavouritePlayerNames.Add(fav.Name);
            }
            ////////TO DOOOO/////
            
            
            return View();
        }

        public async Task<IActionResult> NewsForSelectedPlayer(string name)
        {
            var newsItems = await _newsApiRepository.GetNews(name);

            return View(newsItems);
        }
    }
}
