using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Models
{
    public class Favourite
    {
        public int Id { get; set; }
        public int Pid { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public List<ApplicationUserFavourite> ApplicationUserFavorites { get; set; }
    }
}
