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
                return ShowNewsFor3OrMoreFavourites(currentFavouritePlayerNames, newsItemsList);
            }
        }


        public IActionResult NewsForSelectedPlayer(string name)
        {
            var newsItems = _newsApiRepository.GetNews(name);

            TempData["newsPageTitle"] = $"Recent news related to {name}";

            return View("NewsList", newsItems);
        }


        public IActionResult ShowNewsFor2Favourites(List<string> currentFavouritePlayerNames, List<NewsItems> newsItemsList)
        {
            int numberOfFavorites = 2;
            for (int i = 0; i < numberOfFavorites; i++)
            {
                NewsItems newsItems = new NewsItems();
                newsItems = _newsApiRepository.GetNews(currentFavouritePlayerNames[i]);
                newsItemsList.Add(newsItems);
            }

            NewsItems combinedItemsFrom2Players = new NewsItems();
            combinedItemsFrom2Players.items = new List<Item>();

            Dictionary<int, bool> playerHasTwoItems = new Dictionary<int, bool>();
            playerHasTwoItems.Add(0, true);
            playerHasTwoItems.Add(1, true);

            //check that each player actually has at least 2 results
            for (int i = 0; i < numberOfFavorites; i++)
            {
                if (newsItemsList[i].items.Count() < NewsItemsThresholdWhen2FavouritesExist)
                {
                    playerHasTwoItems[i] = false;
                }
            }

            return DisplayNewsDependingOnItemsFrom2Favourites(currentFavouritePlayerNames, newsItemsList, numberOfFavorites, combinedItemsFrom2Players, playerHasTwoItems);
        }

        private IActionResult DisplayNewsDependingOnItemsFrom2Favourites(List<string> currentFavouritePlayerNames, List<NewsItems> newsItemsList, int numberOfFavorites, NewsItems combinedItemsFrom2Players, Dictionary<int, bool> playerHasTwoItems)
        {
            bool notEnoughResultsForEitherPlayer = playerHasTwoItems[0] == false && playerHasTwoItems[1] == false;
            bool onlyPlayer1HasEnoughResults = playerHasTwoItems[0] == true && playerHasTwoItems[1] == false;
            bool onlyPlayer2HasEnoughResults = playerHasTwoItems[0] == false && playerHasTwoItems[1] == true;

            if (notEnoughResultsForEitherPlayer)
            {
                return View("NewsList");
            }

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
                for (int i = 0; i < numberOfFavorites; i++)
                {
                    for (int j = 0; j < NewsItemsThresholdWhen2FavouritesExist; j++)
                    {
                        combinedItemsFrom2Players.items.Add(newsItemsList[i].items[j]);
                    }
                }

                TempData["newsPageTitle"] = "Recent news related to some of your favourite players";

                return View("NewsList", combinedItemsFrom2Players);
            }
        }

        private IActionResult ShowNewsFor3OrMoreFavourites(List<string> currentFavouritePlayerNames, List<NewsItems> newsItemsList)
        {
            NewsItems newsItems = new NewsItems();
            Dictionary<int, bool> playerHasOneItem = new Dictionary<int, bool>();
            playerHasOneItem.Add(0, true);
            playerHasOneItem.Add(1, true);
            playerHasOneItem.Add(2, true);

            int numberOfFavorites = 3;
            for (int i = 0; i < numberOfFavorites; i++)
            {
                newsItems = _newsApiRepository.GetNews(currentFavouritePlayerNames[i]);
                newsItemsList.Add(newsItems);
            }

            //check that each favourite player has at least 1 news item
            for (int i = 0; i < numberOfFavorites; i++)
            {
                if (newsItemsList[i].items.Count() < NewsItemsThresholdWhen3OrMoreFavouritesExist)
                {
                    playerHasOneItem[i] = false;
                }
            }

            return DisplayNewsDependingOnNewsItemsFrom3Favourites(currentFavouritePlayerNames, newsItemsList, playerHasOneItem, numberOfFavorites);
        }


        private IActionResult DisplayNewsDependingOnNewsItemsFrom3Favourites(List<string> currentFavouritePlayerNames, List<NewsItems> newsItemsList, Dictionary<int, bool> playerHasOneItem, int numberOfFavorites)
        {
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

            else
            {
                return MoreThan1PlayerWithResults(currentFavouritePlayerNames, newsItemsList, numberOfFavorites, onlyPlayer1HasNoResults, onlyPlayer2HasNoResults, onlyPlayer3HasNoResults);
            }
        }


        private IActionResult MoreThan1PlayerWithResults(List<string> currentFavouritePlayerNames, List<NewsItems> newsItemsList, int numberOfFavorites, bool onlyPlayer1HasNoResults, bool onlyPlayer2HasNoResults, bool onlyPlayer3HasNoResults)
        {
            //if two players have at least 1 result, but the 3rd has none, just display results for one of the players who does have some
            if (onlyPlayer3HasNoResults)
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
                return threePlayersWithResults(newsItemsList, numberOfFavorites);
            }
        }


        private IActionResult threePlayersWithResults(List<NewsItems> newsItemsList, int numberOfFavorites)
        {
            //if all 3 players have at least 1 result, take first result from each newsItems.items for each player
            NewsItems combinedItemsFrom3Favourites = new NewsItems();
            combinedItemsFrom3Favourites.items = new List<Item>();

            for (int i = 0; i < numberOfFavorites; i++)
            {
                for (int j = 0; j < NewsItemsThresholdWhen3OrMoreFavouritesExist; j++)
                {
                    combinedItemsFrom3Favourites.items.Add(newsItemsList[i].items[j]);
                }
            }

            TempData["newsPageTitle"] = "Recent news related to some of your favourite players";
            return View("NewsList", combinedItemsFrom3Favourites);
        }
    }
}
