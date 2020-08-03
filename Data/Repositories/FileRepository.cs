using CricketFavourites.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Data.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly DbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public FileRepository(DbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<FileModel>> AllFiles()
        {
            var allFiles = await _dbContext.Files.ToListAsync();
            return allFiles;       
        }

        public void SavePlayerImage(List<IFormFile> files, string description, int favouriteId)
        {
            var userId = _serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingFile = _dbContext.Files.FirstOrDefault(f => f.ApplicationUserId == userId && f.FavouriteId == favouriteId);

            if (existingFile != null)
            {
                _dbContext.Remove(existingFile);
            }
            
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var newFile = new CricketFavourites.Models.FileModel
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    Description = description,
                    ApplicationUserId = userId,
                    FavouriteId = favouriteId
                };

                using (var dataStream = new MemoryStream())
                {
                    file.CopyTo(dataStream);
                    newFile.Data = dataStream.ToArray();
                }

                _dbContext.Files.Add(newFile);
                _dbContext.SaveChanges();
            }
        }


    public FileModel GetImageByFavouriteId(int id)
    {
        var userId = _serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        return _dbContext.Files.FirstOrDefault(f => f.FavouriteId == id && f.ApplicationUserId == userId);
    }
}
}
