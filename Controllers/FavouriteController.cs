using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CricketFavourites.Data;
using CricketFavourites.Models;
using CricketFavourites.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CricketFavourites.Controllers
{
    public class FavouriteController : Controller
    {

        public FavouriteController()
        {

        }

        public IActionResult List()
        {
            return View();
        }

        public async Task<IActionResult> Search()
        {
            string query = "steven smith";

            var playerList = await CricApiHelper.GetPlayers(query);

            return View(playerList);

        }
    }
}
