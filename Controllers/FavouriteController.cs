using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CricketFavourites.Data;
using CricketFavourites.Data.Repositories;
using CricketFavourites.Models;
using CricketFavourites.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

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

        public async Task<IActionResult> List()
        {
            //get all favourite objects for current user
            var currentFavourites = _favouriteRepository.GetCurrentUserFavourites();

            List<PlayerInfo> currentFavouriteInfoList = new List<PlayerInfo>();
            PlayerInfo currentFavouriteInfo = new PlayerInfo();

            //get new data on each favourite from the API and add to list
            foreach (var f in currentFavourites)
            {
                currentFavouriteInfo = await _cricApiRepository.GetPlayerInfo(f.Pid);
                currentFavouriteInfoList.Add(currentFavouriteInfo);
            }

            return View(new FavouritesViewModel 
            { 
                FavouritesInfo = currentFavouriteInfoList
            });      
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

        public IActionResult AddFavouritePlayer(string fullName, string name, int pid)
        {
            Favourite favourite = new Favourite
            {
                FullName = fullName,
                Name = name,
                Pid = pid
            };

            _favouriteRepository.AddFavourite(favourite);

            return RedirectToAction("List");
        }

        public IActionResult Unfavourite(int pid)
        {
            _favouriteRepository.RemoveFavourite(pid);

            return RedirectToAction("List");
        }


        public async Task<IActionResult> ShowPlayerStats(int pid)
        {
            PlayerInfo selectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid);

            return View(selectedPlayerInfo);
        }


        public IActionResult Compare()
        {
            return View();
        }

    
    }
}
