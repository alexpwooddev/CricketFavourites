using CricketFavourites.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Models
{
    public class ApplicationUserFavourite
    {
        public string ApplicationUserId { get; set; }
        public int FavouriteId { get; set; }
        public ApplicationUser  ApplicationUser { get; set; }
        public Favourite Favourite { get; set; }


    }
}
