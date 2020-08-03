using System;
using System.Collections.Generic;
using System.Text;
using CricketFavourites.Areas.Identity.Data;
using CricketFavourites.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CricketFavourites.Data
{

    public class DbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<ApplicationUserFavourite> ApplicationUserFavourites { get; set; }
        public DbSet<FileModel> Files { get; set; }


        public DbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUserFavourite>().HasKey(f => new { f.ApplicationUserId, f.FavouriteId });
        }
    }
}
