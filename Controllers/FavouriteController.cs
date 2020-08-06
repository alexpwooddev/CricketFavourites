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

        public FavouriteController(ICricApiRepository cricApiRepository, IFavouriteRepository favouriteRepository, IFileRepository fileRepository)
        {
            _cricApiRepository = cricApiRepository;
            _favouriteRepository = favouriteRepository;
            _fileRepository = fileRepository;
        }

        public async Task<IActionResult> ListFavourites()
        {
            var currentFavourites = _favouriteRepository.GetCurrentUserFavourites();
            Dictionary<PlayerInfo, FileModel> combinedPlayerAndImage = new Dictionary<PlayerInfo, FileModel>();
            PlayerInfo currentFavouriteInfo = new PlayerInfo();
            FileModel currentPlayerImage = new FileModel();

            //get up-to-date data on each favourite from the API and add to list
            foreach (var f in currentFavourites)
            {
                currentFavouriteInfo = await _cricApiRepository.GetPlayerInfo(f.Pid);
                currentPlayerImage = _fileRepository.GetImageByFavouriteId(f.Id);
                combinedPlayerAndImage.Add(currentFavouriteInfo, currentPlayerImage);
            }

            return View(new FavouritesViewModel
            {
                CombinedPlayersAndImages = combinedPlayerAndImage
            });
        }

        public IActionResult Search()
        {
            return View();
        }

        [ActionName("SearchWithQuery")]
        public async Task<IActionResult> Search(string query)
        {
            var playerList = await _cricApiRepository.GetPlayers(query);

            return View("Search", new SearchViewModel
            {
                PlayerList = playerList
            });
        }

        [ActionName("PlayerInfoFromPid")]
        public async Task<IActionResult> Search(int pid)
        {
            PlayerInfo selectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid);

            return View("Search", new SearchViewModel
            {
                PlayerInfo = selectedPlayerInfo
            });
        }

        public IActionResult AddFavouritePlayer(string fullName, string name, int pid)
        {
            if (_favouriteRepository.HasBeenFavouritedAlready(pid))
            {
                TempData["alreadyFavouritedMessage"] = "You have already favourited this player!";
                return RedirectToAction(nameof(ListFavourites));
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
                return RedirectToAction(nameof(ListFavourites));
            }
        }

        public IActionResult Unfavourite(int pid)
        {
            _favouriteRepository.RemoveFavourite(pid);
            return RedirectToAction(nameof(ListFavourites));
        }

    }
}
