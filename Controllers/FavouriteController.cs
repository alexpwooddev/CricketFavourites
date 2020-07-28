using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CricketFavourites.Data;
using CricketFavourites.Data.Repositories;
using CricketFavourites.Models;
using CricketFavourites.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CricketFavourites.Controllers
{
    [Authorize]
    public class FavouriteController : Controller
    {
        private readonly ICricApiRepository _cricApiRepository;
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IServiceProvider _services;

        public FavouriteController(ICricApiRepository cricApiRepository, IFavouriteRepository favouriteRepository, 
            IServiceProvider services)
        {
            _cricApiRepository = cricApiRepository;
            _favouriteRepository = favouriteRepository;
            _services = services;
        }

        public IActionResult List()
        {
            return View();
        }


        public async Task<IActionResult> Search(string query, int pid)
        {
            //user goes to blank search page 
            if (string.IsNullOrEmpty(query) && pid == 0)
            {
                return View();
            }

            //user query produces list of possible players
            else if (!string.IsNullOrEmpty(query) && pid == 0)
            {
                var playerList = await _cricApiRepository.GetPlayers(query);

                return View(new SearchViewModel
                {
                    PlayerList = playerList
                });
            }

            else
            {
                PlayerInfo selectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid);

                return View(new SearchViewModel
                {
                    PlayerInfo = selectedPlayerInfo
                });
            }
        }

        public IActionResult AddFavouritePlayer(string fullName, int pid)
        {
            Favourite favourite = new Favourite
            {
                FullName = fullName,
                Pid = pid
            };

            _favouriteRepository.AddFavourite(_services, favourite);

            return RedirectToAction("List");
        }


        public async Task<IActionResult> Compare()
        {
            return View();
        }

        //public async Task<IActionResult> PlayerInfo(int pid)
        //{
        //    PlayerInfo selectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid);

        //    return View(new SearchViewModel
        //    {
        //        PlayerInfo = selectedPlayerInfo
        //    });
        //}
    }
}
