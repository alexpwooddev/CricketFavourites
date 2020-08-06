using CricketFavourites.Data.Repositories;
using CricketFavourites.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Controllers
{
    public class FavouriteStatisticsController : Controller
    {
        private readonly ICricApiRepository _cricApiRepository;
        private readonly IFavouriteRepository _favouriteRepository;

        public FavouriteStatisticsController(ICricApiRepository cricApiRepository, IFavouriteRepository favouriteRepository)
        {
            _cricApiRepository = cricApiRepository;
            _favouriteRepository = favouriteRepository;
        }


        public async Task<IActionResult> ShowPlayerStats(int pid)
        {
            PlayerInfo selectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid);
            return View(selectedPlayerInfo);
        }


        public IActionResult CompareStats()
        {
            TempData.Clear();
            var currentFavourites = _favouriteRepository.GetCurrentUserFavourites();
            if (currentFavourites.Count() == 0)
            {
                TempData["noFavouritesMessage"] = "You don't have any favourites yet. Do a search and add some!";
            }

            return View();
        }

        [ActionName("CompareWithPlayerSelected")]
        public async Task<IActionResult> CompareStats(int pid1, int pid2)
        {
            bool Player1JustChosenPlayer2NotChosen = pid1 != 0 && pid2 == 0 && (!TempData.ContainsKey("pid2Chosen") || TempData["pid2Chosen"].ToString() == "no");
            bool Player2JustChosenPlayer1NotChosen = pid1 == 0 && pid2 != 0 && (!TempData.ContainsKey("pid1Chosen") || TempData["pid1Chosen"].ToString() == "no");
            bool Player2JustChosenAfterAnyPreviousSelection = TempData.ContainsKey("pid1Chosen") && TempData["pid1Chosen"].ToString() != "no" && pid2 != 0;
            bool Player1JustChosenAfterAnyPreviousSelection = TempData.ContainsKey("pid2Chosen") && TempData["pid2Chosen"].ToString() != "no" && pid1 != 0;

            if (Player1JustChosenPlayer2NotChosen)
            {
                List<PlayerInfo> combinedPlayerInfo = await ShowPlayer1ChoiceWithNoPlayer2Choice(pid1);
                return View("CompareStats", combinedPlayerInfo);
            }

            else if (Player2JustChosenPlayer1NotChosen)
            {
                List<PlayerInfo> combinedPlayerInfo = await ShowPlayer2ChoiceWithNoPlayer1Choice(pid2);
                return View("CompareStats", combinedPlayerInfo);
            }

            else if (Player2JustChosenAfterAnyPreviousSelection)
            {
                List<PlayerInfo> combinedPlayerInfo = await ShowPlayer2ChoiceAndPreviousChoice(pid1, pid2);
                return View("CompareStats", combinedPlayerInfo);
            }

            else // if (Player1JustChosenAfterAnyPreviousSelection)
            {
                List<PlayerInfo> combinedPlayerInfo = await ShowPlayer1ChoiceAndPreviousChoice(pid1, pid2);
                return View("CompareStats", combinedPlayerInfo);
            }
        }


        private async Task<List<PlayerInfo>> ShowPlayer1ChoiceWithNoPlayer2Choice(int pid1)
        {
            PlayerInfo firstSelectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid1);
            PlayerInfo secondSelectedPlayerInfo = new PlayerInfo();

            List<PlayerInfo> combinedPlayerInfo = new List<PlayerInfo>();
            combinedPlayerInfo.Add(firstSelectedPlayerInfo);
            combinedPlayerInfo.Add(secondSelectedPlayerInfo);

            TempData["pid1Chosen"] = pid1.ToString();
            TempData["pid2Chosen"] = "no";
            TempData.Keep();

            return combinedPlayerInfo;
        }

        private async Task<List<PlayerInfo>> ShowPlayer2ChoiceWithNoPlayer1Choice(int pid2)
        {
            PlayerInfo secondSelectedPlayerInfo = await _cricApiRepository.GetPlayerInfo(pid2);
            PlayerInfo firstSelectedPlayerInfo = new PlayerInfo();

            List<PlayerInfo> combinedPlayerInfo = new List<PlayerInfo>();
            combinedPlayerInfo.Add(firstSelectedPlayerInfo);
            combinedPlayerInfo.Add(secondSelectedPlayerInfo);

            TempData["pid1Chosen"] = "no";
            TempData["pid2Chosen"] = pid2.ToString();
            TempData.Keep();

            return combinedPlayerInfo;
        }

        private async Task<List<PlayerInfo>> ShowPlayer2ChoiceAndPreviousChoice(int pid1, int pid2)
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

            return combinedPlayerInfo;
        }

        private async Task<List<PlayerInfo>> ShowPlayer1ChoiceAndPreviousChoice(int pid1, int pid2)
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

            return combinedPlayerInfo;
        }
    }
}
