using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CricketFavourites.Models;

namespace CricketFavourites.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        public List<ApplicationUserFavourite> ApplicationUserFavourites { get; set; }

    }
}
