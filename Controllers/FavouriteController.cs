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
        private readonly IFileRepository _fileRepository;
        private readonly IServiceProvider _services;

        public FavouriteController(ICricApiRepository cricApiRepository, IFavouriteRepository favouriteRepository, IFileRepository fileRepository,
            IServiceProvider services)
        {
            _cricApiRepository = cricApiRepository;
            _favouriteRepository = favouriteRepository;
            _fileRepository = fileRepository;
            _services = services;
        }

        public async Task<IActionResult> List()
        {
            //get all favourite objects for current user
            var currentFavourites = _favouriteRepository.GetCurrentUserFavourites();

            Dictionary<PlayerInfo, FileModel> combinedPlayerAndImage = new Dictionary<PlayerInfo, FileModel>();

            //List<PlayerInfo> currentFavouriteInfoList = new List<PlayerInfo>();

            PlayerInfo currentFavouriteInfo = new PlayerInfo();
            FileModel currentPlayerImage = new FileModel();

            //get up-to-date data on each favourite from the API and add to list
            foreach (var f in currentFavourites)
            {
                currentFavouriteInfo = await _cricApiRepository.GetPlayerInfo(f.Pid);
                currentPlayerImage = _fileRepository.GetImageByFavouriteId(f.Id);
                combinedPlayerAndImage.Add(currentFavouriteInfo, currentPlayerImage);

                //currentFavouriteInfoList.Add(currentFavouriteInfo);
            }

            return View(new FavouritesViewModel
            {
                CombinedPlayersAndImages = combinedPlayerAndImage
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
            //check if this user has already favourited this player and return a message if so
            if (_favouriteRepository.HasBeenFavouritedAlready(pid))
            {
                TempData["alreadyFavouritedMessage"] = "You have already favourited this player!";
                return RedirectToAction("List");
            }

            else
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


        public async Task<IActionResult> CompareStats(int pid1, int pid2)
        {
            //default page where no player is selected when it loads
            if (pid1 == 0 && pid2 == 0)
            {
                TempData.Clear();
                return View();
            }

            //has just chosen player 1 - no player 2 selection has been made
            else if (pid1 != 0 && pid2 == 0 && (!TempData.ContainsKey("pid2Chosen") || TempData["pid2Chosen"].ToString() == "no"))
            {
                PlayerInfo firstSelectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid1);
                PlayerInfo secondSelectedPlayerInfo = new PlayerInfo();

                List<PlayerInfo> combinedPlayerInfo = new List<PlayerInfo>();
                combinedPlayerInfo.Add(firstSelectedPlayerInfo);
                combinedPlayerInfo.Add(secondSelectedPlayerInfo);

                TempData["pid1Chosen"] = pid1.ToString();
                TempData["pid2Chosen"] = "no";
                TempData.Keep();

                return View(combinedPlayerInfo);
            }

            //has just chosen player 2 - no player 1 selection has been made
            else if (pid1 == 0 && pid2 != 0 && (!TempData.ContainsKey("pid1Chosen") || TempData["pid1Chosen"].ToString() == "no"))
            {
                PlayerInfo secondSelectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid2);
                PlayerInfo firstSelectedPlayerInfo = new PlayerInfo();

                List<PlayerInfo> combinedPlayerInfo = new List<PlayerInfo>();
                combinedPlayerInfo.Add(firstSelectedPlayerInfo);
                combinedPlayerInfo.Add(secondSelectedPlayerInfo);
                
                TempData["pid1Chosen"] = "no";
                TempData["pid2Chosen"] = pid2.ToString();
                TempData.Keep();

                return View(combinedPlayerInfo);
            }

            //USED having previously made a selection of any type, has now selected player 2
            else if (TempData["pid1Chosen"].ToString() != "no" && pid2 != 0)
            {
                pid1 = Int32.Parse(TempData["pid1Chosen"] as string);
                PlayerInfo firstSelectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid1);
                PlayerInfo secondSelectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid2);

                List<PlayerInfo> combinedPlayerInfo = new List<PlayerInfo>();
                combinedPlayerInfo.Add(firstSelectedPlayerInfo);
                combinedPlayerInfo.Add(secondSelectedPlayerInfo);

                TempData["pid2Chosen"] = pid2.ToString();
                TempData["pid1Chosen"] = pid1.ToString();
                TempData.Keep();

                return View(combinedPlayerInfo);
            }


            //USED previously made a selection of any type has now selected Player 1
            else if (TempData["pid2Chosen"].ToString() != "no" && pid1 != 0)
            {
                pid2 = Int32.Parse(TempData["pid2Chosen"] as string);
                PlayerInfo firstSelectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid1);
                PlayerInfo secondSelectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid2);

                List<PlayerInfo> combinedPlayerInfo = new List<PlayerInfo>();
                combinedPlayerInfo.Add(firstSelectedPlayerInfo);
                combinedPlayerInfo.Add(secondSelectedPlayerInfo);

                TempData["pid2Chosen"] = pid2.ToString();
                TempData["pid1Chosen"] = pid1.ToString();
                TempData.Keep();

                return View(combinedPlayerInfo);
            }

            else
            {
                return View();
            }

        }


    }
}
