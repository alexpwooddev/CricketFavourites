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
        private const int NewsItemsThresholdWhen2FavouritesExist = 2;
        private const int NewsItemsThresholdWhen3OrMoreFavouritesExist = 1;

        public NewsController(INewsApiRepository newsApiRepository, IFavouriteRepository favouriteRepository)
        {
            _newsApiRepository = newsApiRepository;
            _favouriteRepository = favouriteRepository;
        }

        public IActionResult DefaultNewsList()
        {
            List<Favourite> currentUserFavourites = _favouriteRepository.GetCurrentUserFavourites();
            List<string> currentFavouritePlayerNames = new List<string>();
            List<NewsItems> newsItemsList = new List<NewsItems>();
            
            foreach (Favourite fav in currentUserFavourites)
            {
                currentFavouritePlayerNames.Add(fav.Name);
            }

            bool hasNoFavouritedPlayers = currentUserFavourites.Count == 0;
            bool has1FavouritePlayers = currentUserFavourites.Count == 1;
            bool has2FavouritePlayers = currentUserFavourites.Count == 2;

            if (hasNoFavouritedPlayers)
            {
                TempData["noFavouritesMessage"] = "You have no favourited players. Please search for players and add them to your favourites.";
                return View("NewsList");
            }

            else if (has1FavouritePlayers)
            {
                return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[0] });
            }

            else if (has2FavouritePlayers)
            {
                return RedirectToAction(nameof(ShowNewsFor2Favourites), new { currentFavouritePlayerNames = currentFavouritePlayerNames, newsItemsList = newsItemsList });                                  
            }

            //user has 3 or more favourited players
            else
            {
                return RedirectToAction(nameof(ShowNewsFor3OrMoreFavourites), new { currentFavouritePlayerNames = currentFavouritePlayerNames, newsItemsList = newsItemsList });
            }
        }




        public IActionResult ShowNewsFor2Favourites(List<string> currentFavouritePlayerNames, List<NewsItems> newsItemsList)
        {
            //get news items for 2 favourite players into a list
            for (int i = 0; i < 2; i++)
            {
                NewsItems newsItems = new NewsItems();
                newsItems = _newsApiRepository.GetNews(currentFavouritePlayerNames[i]);
                newsItemsList.Add(newsItems);
            }

            //take first 2 results from each newsItems.items for each player and add to new List
            NewsItems combinedItems = new NewsItems();
            combinedItems.items = new List<Item>();

            //check that each player actually has at least 2 results
            Dictionary<int, bool> playerHasTwoItems = new Dictionary<int, bool>();
            playerHasTwoItems.Add(0, true);
            playerHasTwoItems.Add(1, true);

            for (int i = 0; i < 2; i++)
            {
                if (newsItemsList[i].items.Count() < NewsItemsThresholdWhen2FavouritesExist)
                {
                    playerHasTwoItems[i] = false;
                }
            }

            bool notEnoughResultsForEitherPlayer = playerHasTwoItems[0] == false && playerHasTwoItems[1] == false;
            bool onlyPlayer1HasEnoughResults = playerHasTwoItems[0] == true && playerHasTwoItems[1] == false;
            bool onlyPlayer2HasEnoughResults = playerHasTwoItems[0] == false && playerHasTwoItems[1] == true;


            //if neither player has at least 2 results then display nothing
            if (notEnoughResultsForEitherPlayer)
            {
                return View("NewsList");
            }

            //if one player doesn't have at least 2 results, then just display results for the other:
            else if (onlyPlayer1HasEnoughResults)
            {
                return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[0] });
            }

            else if (onlyPlayer2HasEnoughResults)
            {
                return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[1] });
            }

            else //display 2 results for each player
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        combinedItems.items.Add(newsItemsList[i].items[j]);
                    }
                }

                TempData["newsPageTitle"] = "Recent news related to some of your favourite players";

                return View("NewsList", combinedItems);
            }
        }


        public IActionResult ShowNewsFor3OrMoreFavourites(List<string> currentFavouritePlayerNames, List<NewsItems> newsItemsList)
        {
            Dictionary<int, bool> playerHasOneItem = new Dictionary<int, bool>();
            playerHasOneItem.Add(0, true);
            playerHasOneItem.Add(1, true);
            playerHasOneItem.Add(2, true);

            NewsItems newsItems = new NewsItems();

            //check if each player has news items
            for (int i = 0; i < 3; i++)
            {
                newsItems = _newsApiRepository.GetNews(currentFavouritePlayerNames[i]);
                newsItemsList.Add(newsItems);
            }

            for (int i = 0; i < 3; i++)
            {
                if (newsItemsList[i].items.Count() < NewsItemsThresholdWhen3OrMoreFavouritesExist)
                {
                    playerHasOneItem[i] = false;
                }
            }

            bool noPlayerHasResult = playerHasOneItem[0] == false && playerHasOneItem[1] == false && playerHasOneItem[2] == false;
            bool onlyPlayer1HasResults = playerHasOneItem[0] == true && playerHasOneItem[1] == false && playerHasOneItem[2] == false;
            bool onlyPlayer2HasResults = playerHasOneItem[0] == false && playerHasOneItem[1] == true && playerHasOneItem[2] == false;
            bool onlyPlayer3HasResults = playerHasOneItem[0] == false && playerHasOneItem[1] == false && playerHasOneItem[2] == true;
            bool onlyPlayer1HasNoResults = playerHasOneItem[0] == false && playerHasOneItem[1] == true && playerHasOneItem[2] == true;
            bool onlyPlayer2HasNoResults = playerHasOneItem[0] == true && playerHasOneItem[1] == false && playerHasOneItem[2] == true;
            bool onlyPlayer3HasNoResults = playerHasOneItem[0] == true && playerHasOneItem[1] == true && playerHasOneItem[2] == false;

            if (noPlayerHasResult)
            {
                return View("NewsList");
            }

            //if only 1 player has at least 1 result, then just display results for that player            
            else if (onlyPlayer1HasResults)
            {
                return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[0] });
            }

            else if (onlyPlayer2HasResults)
            {
                return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[1] });
            }

            else if (onlyPlayer3HasResults)
            {
                return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[2] });
            }


            //if two players have at least 1 result, but the 3rd has none, just display results for one of the players who does have some
            else if (onlyPlayer3HasNoResults)
            {
                return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[0] });
            }

            else if (onlyPlayer2HasNoResults)
            {
                return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[2] });
            }

            else if (onlyPlayer1HasNoResults)
            {
                return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[1] });
            }


            else
            {
                //if all 3 players have at least 1 result, take first result from each newsItems.items for each player
                NewsItems combinedItems = new NewsItems();
                combinedItems.items = new List<Item>();

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        combinedItems.items.Add(newsItemsList[i].items[j]);
                    }
                }

                TempData["newsPageTitle"] = "Recent news related to some of your favourite players";
                return View("NewsList", combinedItems);
            }
        }

        public IActionResult NewsForSelectedPlayer(string name)
        {
            var newsItems = _newsApiRepository.GetNews(name);

            TempData["newsPageTitle"] = $"Recent news related to {name}";

            return View("NewsList", newsItems);
        }
    }
}
