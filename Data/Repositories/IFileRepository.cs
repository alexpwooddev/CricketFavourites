﻿using CricketFavourites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Data.Repositories
{
    public interface IFileRepository
    {
        FileModel GetImageByFavouriteId(int id);
    }
}
