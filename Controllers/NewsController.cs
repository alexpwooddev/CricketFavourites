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
            NewsItems newsItems = new NewsItems();

            foreach (Favourite fav in currentUserFavourites)
            {
                currentFavouritePlayerNames.Add(fav.Name);
            }

            //user has no favourited players
            if (currentUserFavourites == null)
            {
                TempData["newsPageTitle"] = "You have no favourited players. Please search for players and add them to your favourites.";

                return View("NewList");
            }

            //user only has 1 favourited player
            else if (currentUserFavourites.Count == 1)
            {
                return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[0] });
            }

            //user has 2 favourited players
            else if (currentUserFavourites.Count == 2)
            {
                //NewsForTwoPlayers(currentUserFavourites, currentFavouritePlayerNames, newsItemsList, newsItems);
                //get news items for 2 favourite players into a list
                for (int i = 0; i < 2; i++)
                {
                    newsItems = _newsApiRepository.GetNews(currentFavouritePlayerNames[i]);
                    newsItemsList.Add(newsItems);
                }

                //take first 2 results from each newsItems.items for each player and add to new news
                //create a blank NewsItems object, then add things to its "List<item> items"? Then return that to the view?

                NewsItems combinedItems = new NewsItems();
                combinedItems.items = new List<Item>();

                //check that each player actually has at least 2 results
                Dictionary<int, bool> playerHasTwoItems = new Dictionary<int, bool>();
                playerHasTwoItems.Add(0, true);
                playerHasTwoItems.Add(1, true);

                for (int i = 0; i < 2; i++)
                {
                    if (newsItemsList[i].items.Count() < 2)
                    {
                        playerHasTwoItems[i] = false;
                    }
                }

                //if neither player has at least 2 results then display nothing:
                if (playerHasTwoItems[0] == false && playerHasTwoItems[1] == false)
                {
                    return View("NewsList");
                }

                //if one player doesn't have at least 2 results, then just display results for the other:
                else if (playerHasTwoItems[0] == true && playerHasTwoItems[1] == false)
                {
                    return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[0] });
                }

                else if (playerHasTwoItems[0] == false && playerHasTwoItems[1] == true)
                {
                    return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[1] });
                }

                else
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


            //user has 3 or more favourited players
            else
            {
                //check that each player actually has at least 1 result
                Dictionary<int, bool> playerHasOneItem = new Dictionary<int, bool>();
                playerHasOneItem.Add(0, true);
                playerHasOneItem.Add(1, true);
                playerHasOneItem.Add(2, true);

                for (int i = 0; i < 3; i++)
                {
                    newsItems = _newsApiRepository.GetNews(currentFavouritePlayerNames[i]);
                    newsItemsList.Add(newsItems);
                }

                for (int i = 0; i < 3; i++)
                {
                    if (newsItemsList[i].items.Count() < 1)
                    {
                        playerHasOneItem[i] = false;
                    }
                }


                //if no player has at least 1 result then display nothing:
                if (playerHasOneItem[0] == false && playerHasOneItem[1] == false && playerHasOneItem[2] == false)
                {
                    return View("NewsList");
                }


                //if only 1 player has at least 1 result, then just display results for that player
                else if (playerHasOneItem[0] == true && playerHasOneItem[1] == false && playerHasOneItem[2] == false)
                {
                    return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[0] });
                }

                else if (playerHasOneItem[0] == false && playerHasOneItem[1] == true && playerHasOneItem[2] == false)
                {
                    return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[1] });
                }

                else if (playerHasOneItem[0] == false && playerHasOneItem[1] == false && playerHasOneItem[2] == true)
                {
                    return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[2] });
                }


                //if two players have at least 1 result, but the 3rd has none, just display results for one of the players who does have some
                else if (playerHasOneItem[0] == true && playerHasOneItem[1] == true && playerHasOneItem[2] == false)
                {
                    return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[0] });
                }

                else if (playerHasOneItem[0] == true && playerHasOneItem[1] == false && playerHasOneItem[2] == true)
                {
                    return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[2] });
                }

                else if (playerHasOneItem[0] == false && playerHasOneItem[1] == true && playerHasOneItem[2] == true)
                {
                    return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[1] });
                }


                else
                {
                    //if all 3 players have at least 1 result


                    //take first result from each newsItems.items for each player and add to new news
                    //create a blank NewsItems object, then add things to its "List<item> items"? Then return that to the view?

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
        }





        public IActionResult NewsForSelectedPlayer(string name)
        {
            var newsItems = _newsApiRepository.GetNews(name);

            TempData["newsPageTitle"] = $"Recent news related to {name}";

            return View("NewsList", newsItems);
        }




        //private IActionResult NewsForTwoPlayers(List<Favourite> currentUserFavourites, List<string> currentFavouritePlayerNames, List<NewsItems> newsItemsList, NewsItems newsItems)
        //{
        //    //get news items for 2 favourite players into a list
        //    for (int i = 0; i < 2; i++)
        //    {
        //        newsItems = _newsApiRepository.GetNews(currentFavouritePlayerNames[i]);
        //        newsItemsList.Add(newsItems);
        //    }

        //    //take first 2 results from each newsItems.items for each player and add to new news
        //    //create a blank NewsItems object, then add things to its "List<item> items"? Then return that to the view?

        //    NewsItems combinedItems = new NewsItems();
        //    combinedItems.items = new List<Item>();

        //    //check that each player actually has at least 2 results
        //    Dictionary<int, bool> playerHasTwoItems = new Dictionary<int, bool>();
        //    playerHasTwoItems.Add(0, true);
        //    playerHasTwoItems.Add(1, true);

        //    for (int i = 0; i < 2; i++)
        //    {
        //        if (newsItemsList[i].items.Count() < 2)
        //        {
        //            playerHasTwoItems[i] = false;
        //        }
        //    }

        //    //if neither player has at least 2 results then display nothing:
        //    if (playerHasTwoItems[0] == false && playerHasTwoItems[1] == false)
        //    {
        //        return View("NewsList");
        //    }

        //    //if one player doesn't have at least 2 results, then just display results for the other:
        //    else if (playerHasTwoItems[0] == true && playerHasTwoItems[1] == false)
        //    {
        //        return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[0] });
        //    }

        //    else if (playerHasTwoItems[0] == false && playerHasTwoItems[1] == true)
        //    {
        //        return RedirectToAction("NewsForSelectedPlayer", new { name = currentFavouritePlayerNames[1] });
        //    }

        //    else
        //    {
        //        for (int i = 0; i < 2; i++)
        //        {
        //            for (int j = 0; j < 2; j++)
        //            {
        //                combinedItems.items.Add(newsItemsList[i].items[j]);
        //            }
        //        }

        //        TempData["newsPageTitle"] = "Recent news related to some of your favourite players";

        //        return View("NewsList", combinedItems);
        //    }
        //}


        //private IActionResult NewsForThreePlayers(List<NewsItems> newsItemsList, List<string> currentFavouritePlayerNames, NewsItems newsItems)
        //{

        //}
    }
}
