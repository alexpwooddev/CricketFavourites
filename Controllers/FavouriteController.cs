using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CricketFavourites.Controllers
{
    public class FavouriteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
