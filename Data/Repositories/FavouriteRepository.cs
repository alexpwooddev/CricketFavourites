﻿using CricketFavourites.Areas.Identity.Data;
using CricketFavourites.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Data.Repositories
{
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly DbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public FavouriteRepository(DbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<Favourite> AllFavourites
        {
            get
            {
                return _dbContext.Favourites.Include(f => f.ApplicationUserFavorites);
            }
        }

        public List<Favourite> GetCurrentUserFavourites()
        {
            var userId = _serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
     
            return _dbContext.Favourites.FromSqlInterpolated
                ($"SELECT * FROM favourites f JOIN ApplicationUserFavourites auf ON f.Id = auf.FavouriteId WHERE auf.ApplicationUserId = {userId}").ToList();
        }

        public void AddFavourite(Favourite favourite)
        {
            var userId = _serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //int userIdInt = Int32.Parse(userId);

            //if the favourite already exists in the DB then make the connection between it and the user (assuming it's not already favourited)
            if (_dbContext.Favourites.Where(f => f.Pid == favourite.Pid).Count() > 0)
            {
                var applicationUser = _dbContext.Users.Include(u => u.ApplicationUserFavourites).FirstOrDefault(u => u.Id == userId);

                var fav = _dbContext.Favourites.FirstOrDefault(f => f.Pid == favourite.Pid);

                //if user doesn't have this favourite then add it
                if (applicationUser.ApplicationUserFavourites.FirstOrDefault(a => a.ApplicationUserId == userId && a.FavouriteId == favourite.Id) == null)
                {
                    var ufJoin = new ApplicationUserFavourite { ApplicationUserId = userId, FavouriteId = fav.Id };

                    _dbContext.Add(ufJoin);

                }
            }

            //If favourite doesn't exist in the DB, connect to the user then add it
            else
            {
                List<ApplicationUserFavourite> applicationUserFavourites = new List<ApplicationUserFavourite>();
                applicationUserFavourites.Add(new ApplicationUserFavourite { ApplicationUserId = userId });

                favourite.ApplicationUserFavorites = applicationUserFavourites;

                _dbContext.Add(favourite);

            }

            _dbContext.SaveChanges();
        }

        public void RemoveFavourite(int pid)
        {
            var userId = _serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //get relevant favouriteId
            var favouriteId = _dbContext.Favourites.FirstOrDefault(f => f.Pid == pid).Id;

            //get the join entity based on the UserId and favouriteId
            var applicationUserFavourite = _dbContext.ApplicationUserFavourites.First(row => row.ApplicationUserId == userId && row.FavouriteId == favouriteId);

            _dbContext.Remove(applicationUserFavourite);
            _dbContext.SaveChanges();
        }


        public Favourite GetFavouriteByName(string fullName)
        {
            return _dbContext.Favourites.FirstOrDefault(f => f.FullName == fullName);
        }

        public Favourite GetFavouriteByPid(int pid)
        {
            return _dbContext.Favourites.FirstOrDefault(f => f.Id == pid);
        }

    }
}