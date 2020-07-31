using CricketFavourites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.ViewModels
{
    public class CompareStatsViewModel
    {
        public List<PlayerInfo> PlayerInfo { get; set; }
        public int LastPid1Selection { get; set; }
        public int LastPid2Selection { get; set; }

    }
}
