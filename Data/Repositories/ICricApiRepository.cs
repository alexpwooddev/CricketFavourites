using CricketFavourites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Data.Repositories
{
    public interface ICricApiRepository
    {
        Task<PlayerList> GetPlayers(string query);
        Task<PlayerInfo> GetPlayerInfo(int pid);
    }
}
