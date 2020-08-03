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
        private readonly IFileRepository _fileRepository;
        private readonly IFavouriteRepository _favouriteRepository;

        public FileUploadController(Data.DbContext dbContext, IFileRepository fileRepository, 
            IFavouriteRepository favouriteRepository)
        {
            _dbContext = dbContext;

            _fileRepository = fileRepository;
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
            viewModel.Files = await _fileRepository.AllFiles();
            return viewModel;
        }


        public IActionResult SetAsPlayerImage(List<IFormFile> files, string description, int favouriteId)
        {
            _fileRepository.SavePlayerImage(files, description, favouriteId);
            TempData["Message"] = "File successfully uploaded";
            return RedirectToAction("List", "Favourite");
        }


    }
}
