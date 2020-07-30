using CricketFavourites.Data.Repositories;
using CricketFavourites.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Components
{
    public class FavouritesDropDownMenu : ViewComponent
    {
        private readonly IFavouriteRepository _favouriteRepository;

        public FavouritesDropDownMenu(IFavouriteRepository favouriteRepository)
        {
            _favouriteRepository = favouriteRepository;
        }

        public IViewComponentResult Invoke()
        {
            List<Favourite> currentFavourites = _favouriteRepository.GetCurrentUserFavourites();

            return View(currentFavourites);
        }
    }
}
