using CricketFavourites.Data;
using CricketFavourites.Data.Migrations;
using CricketFavourites.Data.Repositories;
using CricketFavourites.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly Data.DbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFavouriteRepository _favouriteRepository;

        public FileUploadController(Data.DbContext dbContext, IServiceProvider serviceProvider, IFavouriteRepository favouriteRepository)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
            _favouriteRepository = favouriteRepository;
        }


        public async Task<IActionResult> Index(int pid)
        {
            if (pid == 0)
            {
                pid = Int32.Parse(TempData["currentPid"].ToString());
            }
            var fileUploadViewModel = await LoadAllFiles();
            fileUploadViewModel.CurrentFavourite = _favouriteRepository.GetFavouriteByPid(pid);
            ViewBag.Message = TempData["Message"];
            TempData["currentPid"] = pid;

            return View(fileUploadViewModel);
        }


        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.Files = await _dbContext.Files.ToListAsync();
            return viewModel;
        }



        public async Task<IActionResult> SetAsPlayerImage(List<IFormFile> files, string description, int favouriteId)
        {
            var userId = _serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

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
                    await file.CopyToAsync(dataStream);
                    newFile.Data = dataStream.ToArray();
                }

                _dbContext.Files.Add(newFile);
                _dbContext.SaveChanges();
            }
            TempData["Message"] = "File successfully uploaded";
            return RedirectToAction("List", "Favourite");
        }

        public async Task<IActionResult> DownloadFile(int id)
        {
            var file = await _dbContext.Files.Where(f => f.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.Data, file.FileType, file.Name + file.Extension);
        }

        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await _dbContext.Files.Where(f => f.Id == id).FirstOrDefaultAsync();

            _dbContext.Files.Remove(file);
            _dbContext.SaveChanges();

            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully";
            return RedirectToAction("Index");
        }

    }
}
