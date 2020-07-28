using CricketFavourites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Data.Repositories
{
    public interface IFavouriteRepository
    {
        IEnumerable<Favourite> AllFavourites { get; }
        List<Favourite> GetCurrentUserFavourites();
        void AddFavourite(Favourite favourite);
        Favourite GetFavouriteByName(string fullName);
        Favourite GetFavouriteByPid(int pid);

    }
}
