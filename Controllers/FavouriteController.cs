using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CricketFavourites.Data;
using CricketFavourites.Data.Repositories;
using CricketFavourites.Models;
using CricketFavourites.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CricketFavourites.Controllers
{
    public class FavouriteController : Controller
    {
        private readonly ICricApiRepository _cricApiRepository;

        public FavouriteController(ICricApiRepository cricApiRepository)
        {
            _cricApiRepository = cricApiRepository;
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

        public async Task<IActionResult> PlayerInfo(int pid)
        {
            PlayerInfo selectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid);

            return View(new SearchViewModel
            {
                PlayerInfo = selectedPlayerInfo
            });
        }
    }
}
