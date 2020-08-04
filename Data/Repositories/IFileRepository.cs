using CricketFavourites.Models;
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

        public Task<List<FileModel>> AllFiles();

        public void SavePlayerImage(List<Microsoft.AspNetCore.Http.IFormFile> files, int favouriteId);
    }
}
