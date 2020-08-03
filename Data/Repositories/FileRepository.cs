using CricketFavourites.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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

        public FileModel GetImageByFavouriteId(int id)
        {
            var userId = _serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return _dbContext.Files.FirstOrDefault(f => f.FavouriteId == id && f.ApplicationUserId == userId);
        }
    }
}
